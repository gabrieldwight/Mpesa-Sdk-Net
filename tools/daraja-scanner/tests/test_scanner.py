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
        self.assertEqual(
            [],
            [change for change in changes if change.get("alias") == "PullTransactions"],
        )

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

    def test_pull_transactions_catalog_entry_maps_to_registration_operation(self):
        spec = scanner.load_json(Path(__file__).parents[1] / "daraja-spec.json")
        changes = scanner.classify_changes(
            [
                {
                    "alias": "PullTransactions",
                    "name": "Pull Transactions",
                    "operation": "POST",
                    "endpoint": "pulltransactions/v1/register",
                }
            ],
            spec,
        )
        self.assertEqual(
            [],
            [change for change in changes if change.get("alias") == "PullTransactions"],
        )

    def test_optional_scalar_schema_addition_generates_partial_model(self):
        with tempfile.TemporaryDirectory() as temporary_directory:
            root = Path(temporary_directory)
            source_file = root / "MpesaSdk" / "Dtos" / "Payment.cs"
            source_file.parent.mkdir(parents=True)
            source_file.write_text(
                'namespace MpesaSdk.Dtos { public class Payment { '
                '[JsonPropertyName("Amount")] public string Amount { get; set; } '
                "} }",
                encoding="utf-8",
            )
            remote = {
                "type": "object",
                "properties": {
                    "Amount": {"type": "string", "requiredField": False},
                    "Request-Reference": {"type": "string", "requiredField": False},
                },
            }
            local = {
                "type": "object",
                "properties": {"Amount": {"type": "string", "requiredField": False}},
            }
            pending = {}
            with patch.object(scanner, "ROOT", root):
                fields, error = scanner.generate_optional_schema_fields(
                    pending, "MpesaSdk.Dtos.Payment", remote, local
                )
                additional_remote = {
                    "type": "object",
                    "properties": {
                        "Amount": {"type": "string", "requiredField": False},
                        "Request-Reference": {"type": "string", "requiredField": False},
                        "Receipt-Reference": {"type": "string", "requiredField": False},
                    },
                }
                additional_local = {
                    "type": "object",
                    "properties": {
                        "Amount": {"type": "string", "requiredField": False},
                        "Request-Reference": {"type": "string", "requiredField": False},
                    },
                }
                _, additional_error = scanner.generate_optional_schema_fields(
                    pending,
                    "MpesaSdk.Dtos.Payment",
                    additional_remote,
                    additional_local,
                )
                colliding_remote = {
                    "type": "object",
                    "properties": {
                        "Amount": {"type": "string", "requiredField": False},
                        "Request-Reference": {"type": "string", "requiredField": False},
                        "Receipt-Reference": {"type": "string", "requiredField": False},
                        "Receipt_Reference": {"type": "string", "requiredField": False},
                    },
                }
                collision_fields, collision_error = scanner.generate_optional_schema_fields(
                    pending,
                    "MpesaSdk.Dtos.Payment",
                    colliding_remote,
                    additional_remote,
                )

            self.assertIsNone(error)
            self.assertIsNone(additional_error)
            self.assertEqual([], collision_fields)
            self.assertIn("generated C# property already exists", collision_error)
            self.assertEqual(["Request-Reference"], fields)
            self.assertIn("partial class Payment", pending[source_file])
            generated_file = (
                root
                / "MpesaSdk"
                / "Generated"
                / "MpesaSdk"
                / "Dtos"
                / "Payment.Daraja.Generated.cs"
            )
            generated = next(
                content
                for path, content in pending.items()
                if path.name == "Payment.Daraja.Generated.cs"
            )
            self.assertIn('[JsonPropertyName("Request-Reference")]', generated)
            self.assertIn('[JsonPropertyName("Receipt-Reference")]', generated)
            self.assertNotIn('[JsonPropertyName("Receipt_Reference")]', generated)
            self.assertIn("public string? RequestReference", generated)
            self.assertEqual(1, generated.count("partial class Payment"))
            self.assertNotIn("partial class Payment", source_file.read_text(encoding="utf-8"))
            self.assertFalse(generated_file.exists())

    def test_required_or_complex_schema_additions_are_not_generated(self):
        required = {
            "type": "object",
            "properties": {
                "Reference": {"type": "string", "requiredField": True}
            },
        }
        local = {"type": "object", "properties": {}}
        properties, error = scanner.render_optional_properties(required, local)
        self.assertEqual([], properties)
        self.assertIn("is required", error)

        complex_schema = {
            "type": "object",
            "properties": {
                "Metadata": {
                    "type": "object",
                    "properties": {},
                    "requiredField": False,
                }
            },
        }
        properties, error = scanner.render_optional_properties(complex_schema, local)
        self.assertEqual([], properties)
        self.assertIn("unsupported non-scalar", error)

    def test_type_changes_and_generated_identifier_collisions_are_rejected(self):
        remote = {
            "type": "object",
            "properties": {"Amount": {"type": "number", "requiredField": False}},
        }
        local = {
            "type": "object",
            "properties": {"Amount": {"type": "string", "requiredField": False}},
        }
        pending = {}
        fields, error = scanner.generate_optional_schema_fields(
            pending, "Payment", remote, local
        )
        self.assertEqual([], fields)
        self.assertIn("type changes", error)
        self.assertEqual({}, pending)

        colliding = {
            "type": "object",
            "properties": {
                "request-id": {"type": "string", "requiredField": False},
                "RequestId": {"type": "string", "requiredField": False},
            },
        }
        properties, error = scanner.render_optional_properties(colliding, local)
        self.assertEqual([], properties)
        self.assertIn("map to the C# property", error)

    def test_run_scan_commits_generated_models_only_for_complete_scans(self):
        catalog = [
            {
                "alias": "Authorization",
                "name": "Authorization",
                "operation": "POST",
                "endpoint": "oauth/v1/token",
            }
        ]
        remote = {
            "requestSchema": {
                "type": "object",
                "properties": {
                    "Amount": {"type": "string", "requiredField": False},
                    "Request-Reference": {"type": "string", "requiredField": False},
                },
            }
        }
        spec = {
            "targetAliases": ["Authorization"],
            "headerProfiles": {"bearer": ["Authorization"]},
            "apis": [
                {
                    "alias": "Authorization",
                    "name": "Authorization",
                    "method": "POST",
                    "endpoint": "oauth/v1/token",
                    "headersProfile": "bearer",
                    "requestType": "MpesaSdk.Dtos.Payment",
                }
            ],
        }

        for detail_fails in (False, True):
            with self.subTest(detail_fails=detail_fails), tempfile.TemporaryDirectory() as tmp:
                root = Path(tmp)
                source_file = root / "MpesaSdk" / "Dtos" / "Payment.cs"
                source_file.parent.mkdir(parents=True)
                original_source = (
                    'namespace MpesaSdk.Dtos { public class Payment { '
                    '[JsonPropertyName("Amount")] public string Amount { get; set; } '
                    "} }"
                )
                source_file.write_text(original_source, encoding="utf-8")
                detail_error = (
                    scanner.ScanError("portal schema resolver is unavailable")
                    if detail_fails
                    else None
                )
                with (
                    patch.object(scanner, "ROOT", root),
                    patch.object(scanner, "fetch_catalog", return_value=catalog),
                    patch.object(
                        scanner,
                        "fetch_details",
                        side_effect=detail_error,
                        return_value=remote,
                    ),
                    patch.object(scanner, "explicit_headers", return_value={"Authorization"}),
                    patch.object(scanner, "explicit_schemas", return_value=remote),
                ):
                    status, changes, _ = scanner.run_scan(spec)

                generated_file = (
                    root
                    / "MpesaSdk"
                    / "Generated"
                    / "MpesaSdk"
                    / "Dtos"
                    / "Payment.Daraja.Generated.cs"
                )
                if detail_fails:
                    self.assertEqual("SCAN_INCOMPLETE", status)
                    self.assertEqual(original_source, source_file.read_text(encoding="utf-8"))
                    self.assertFalse(generated_file.exists())
                else:
                    self.assertEqual("UPDATES_DETECTED", status)
                    self.assertEqual("GENERATED_SCHEMA_UPDATE", changes[0]["status"])
                    self.assertTrue(generated_file.exists())
                    self.assertIn("partial class Payment", source_file.read_text(encoding="utf-8"))

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
