import importlib.util
import tempfile
import unittest
from pathlib import Path
from unittest.mock import patch


SCRIPT_PATH = Path(__file__).parents[1] / "daraja-api-scan.py"
FIXTURES = Path(__file__).parent / "fixtures"
SPEC = importlib.util.spec_from_file_location("daraja_api_scan", SCRIPT_PATH)
scanner = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(scanner)


class ScannerTests(unittest.TestCase):
    def setUp(self):
        self.spec = {
            "apis": [
                {
                    "alias": "Authorization",
                    "name": "Authorization",
                    "method": "GET",
                    "endpoint": "oauth/v1/generate?grant_type=client_credentials",
                }
            ]
        }

    def test_catalog_reports_new_endpoint(self):
        fixture = scanner.load_json(FIXTURES / "catalog-change.json")
        changes = scanner.classify_changes(
            [fixture["catalog"][0], fixture["catalog"][1]], self.spec
        )
        self.assertEqual(
            {"MODIFIED_ENDPOINT", "NEW_ENDPOINT"},
            {change["status"] for change in changes},
        )
        self.assertIn("MiniApps", {change["alias"] for change in changes})

    def test_absolute_endpoint_matches_local_relative_path(self):
        fixture = scanner.load_json(FIXTURES / "catalog-no-change.json")
        changes = scanner.classify_changes(fixture["catalog"], self.spec)
        self.assertEqual([], changes)

    def test_catalog_reports_modified_endpoint_and_removed_api(self):
        changes = scanner.classify_changes(
            [
                {
                    "alias": "Authorization",
                    "name": "Authorization",
                    "operation": "POST",
                    "endpoint": "oauth/v2/token",
                },
            ],
            {
                "apis": self.spec["apis"]
                + [
                    {
                        "alias": "PriorApi",
                        "name": "Prior API",
                        "method": "POST",
                        "endpoint": "api/v1/prior",
                    }
                ]
            },
        )
        self.assertEqual(
            {"MODIFIED_ENDPOINT", "REMOVED_OR_HIDDEN"},
            {change["status"] for change in changes},
        )

    def test_suite_group_preserves_distinct_b2b_aliases(self):
        spec = {
            "targetAliasGroups": [
                {
                    "suite": "B2B",
                    "aliases": ["BusinessPayBill", "BusinessBuyGoods", "TaxRemittance"],
                }
            ],
            "apis": [
                {
                    "alias": "BusinessPayBill",
                    "name": "Business Pay Bill",
                    "method": "POST",
                    "endpoint": "mpesa/b2b/v1/paymentrequest",
                },
                {
                    "alias": "BusinessBuyGoods",
                    "name": "Business Buy Goods",
                    "method": "POST",
                    "endpoint": "mpesa/b2b/v1/paymentrequest",
                },
                {
                    "alias": "TaxRemittance",
                    "name": "Tax Remittance",
                    "method": "POST",
                    "endpoint": "mpesa/b2b/v1/remittax",
                },
            ],
        }
        catalog = [
            {
                "alias": "BusinessPayBill",
                "name": "Business Pay Bill",
                "operation": "POST",
                "endpoint": "mpesa/b2b/v1/paymentrequest",
            },
            {
                "alias": "BusinessBuyGoods",
                "name": "Business Buy Goods",
                "operation": "POST",
                "endpoint": "mpesa/b2b/v1/paymentrequest",
            },
            {
                "alias": "TaxRemittance",
                "name": "Tax Remittance",
                "operation": "POST",
                "endpoint": "mpesa/b2b/v1/remittax",
            },
        ]
        self.assertEqual([], scanner.classify_changes(catalog, spec))

    def test_endpoint_update_requires_and_changes_one_mapped_constant(self):
        with tempfile.TemporaryDirectory() as temporary_directory:
            endpoint_file = Path(temporary_directory) / "MpesaSdk" / "MpesaRequestEndpoint.cs"
            endpoint_file.parent.mkdir()
            with endpoint_file.open("w", encoding="utf-8", newline="") as endpoint_source:
                endpoint_source.write(
                    '\ufeffpublic static string RegisterC2BUrl { get; set; } = "old/path";\r\n'
                )
            spec = {
                "apis": [
                    {
                        "alias": "CustomerToBusiness",
                        "endpointConstant": "RegisterC2BUrl",
                    }
                ]
            }
            changes = [
                {
                    "alias": "CustomerToBusiness",
                    "status": "MODIFIED_ENDPOINT",
                    "endpoint": "mpesa/c2b/v2/registerurl",
                }
            ]
            with patch.object(scanner, "ROOT", Path(temporary_directory)):
                scanner.update_endpoint_constants(changes, spec)

            with endpoint_file.open("r", encoding="utf-8-sig", newline="") as updated_source:
                updated = updated_source.read()
            self.assertIn('= "mpesa/c2b/v2/registerurl";', updated)
            self.assertTrue(updated.endswith("\r\n"))

    def test_schema_candidate_requires_explicit_object_schema(self):
        self.assertIsNone(scanner.normalize_schema_candidate({"sample": {"Amount": 25}}))
        self.assertIsNone(scanner.normalize_schema_candidate("not json"))
        self.assertEqual(
            {
                "type": "object",
                "properties": {
                    "Amount": {"type": "number", "requiredField": False}
                },
            },
            scanner.normalize_schema_candidate(
                {"type": "object", "properties": {"Amount": {"type": "number"}}}
            ),
        )

    def test_header_mapping_accepts_only_structured_header_names(self):
        self.assertEqual(
            {"authorization", "content-type"},
            scanner.normalize_headers(
                '[{"key":"Authorization","value":"Bearer token"},'
                '{"name":"Content-Type","value":"application/json"}]'
            ),
        )
        self.assertIsNone(scanner.normalize_headers("Authorization: Bearer token"))

    def test_rendered_report_escapes_no_data_as_incomplete_note(self):
        report = scanner.render_changelog(
            "2026-10-12",
            "SCAN_INCOMPLETE",
            [],
            ["Documentation request failed with HTTP 500."],
        )
        self.assertIn("**Detected Status:** `SCAN_INCOMPLETE`", report)
        self.assertIn("Documentation request failed with HTTP 500.", report)

    def test_local_schema_fields_are_read_from_json_attributes(self):
        schema = scanner.local_type_fields("MpesaSdk.Dtos.MpesaTransactionStatus")
        self.assertIsNotNone(schema)
        self.assertIn("SecurityCredential", schema["properties"])

    def test_schema_diff_names_added_and_changed_fields(self):
        local = {
            "type": "object",
            "properties": {
                "Amount": {"type": "string"},
                "OldField": {"type": "integer"},
            },
        }
        remote = {
            "type": "object",
            "properties": {
                "Amount": {"type": "number"},
                "NewField": {"type": "string"},
            },
        }
        self.assertEqual(
            [
                "added field `NewField`",
                "field no longer documented `OldField`",
                "type changed for `Amount`: `string` -> `number`",
            ],
            scanner.schema_field_diff(remote, local),
        )

    def test_schema_diff_detects_requiredness_changes(self):
        remote = {
            "type": "object",
            "required": ["Amount"],
            "properties": {"Amount": {"type": "number"}},
        }
        local = {
            "type": "object",
            "properties": {"Amount": {"type": "number", "requiredField": False}},
        }
        scanner.annotate_required_fields(remote)
        self.assertEqual(
            ["requiredness changed for `Amount`: `optional` -> `required`"],
            scanner.schema_field_diff(remote, local),
        )


if __name__ == "__main__":
    unittest.main()
