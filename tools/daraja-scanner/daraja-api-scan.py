#!/usr/bin/env python3
"""Compare Safaricom's public Daraja catalog with the checked-in SDK inventory."""

from __future__ import annotations

import argparse
import json
import os
import sys
import re
import time
import urllib.error
import urllib.parse
import urllib.request
from datetime import datetime, timezone
from pathlib import Path
from typing import Any


ROOT = Path(__file__).resolve().parents[2]
SPEC_PATH = ROOT / "tools" / "daraja-scanner" / "daraja-spec.json"
CHANGELOG_PATH = ROOT / "docs" / "CHANGELOG_DARAJA.md"
GRAPHQL_URL = "https://developer.safaricom.co.ke/api/graphql"
CATALOG_QUERY = """
query GetAPIs {
  getApis {
    body { alias category endpoint id name operation parent purpose subsidiaries subsidiary }
    message
    status
  }
}
"""
DETAIL_QUERY = """
query GetApiDocumentation($apiName: String!) {
  populateApi(apiName: $apiName) {
    status
    mess
    body {
      apiDetails { alias category id name subsidiaries subsidiary parent color operation endpoint purpose }
      dataset { operation icon color headers mapping }
      details { name operation endpoint purpose alias parent }
      documentation {
        id priority parentTitle parentContent childTitle childContent grandChildTitle grandChildContent
        postmanCollectionLink addedBy addedOn modifiedBy modifiedOn apiName
      }
    }
    hasMarkdown
    markdown
    sections
  }
}
"""


class ScanError(Exception):
    """A source or input failure that means no change decision is trustworthy."""


def load_json(path: Path) -> dict[str, Any]:
    try:
        value = json.loads(path.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError) as exc:
        raise ScanError(f"Could not read JSON file {path}: {exc}") from exc
    if not isinstance(value, dict):
        raise ScanError(f"Expected a JSON object in {path}")
    return value


def post_graphql(query: str, variables: dict[str, str] | None = None) -> dict[str, Any]:
    body = json.dumps({"query": query, "variables": variables or {}}).encode("utf-8")
    request = urllib.request.Request(
        GRAPHQL_URL,
        data=body,
        headers={
            "Accept": "application/json",
            "Content-Type": "application/json",
            "User-Agent": "Mpesa-Sdk-Net-Daraja-Change-Scanner/1.0",
        },
        method="POST",
    )
    for attempt in range(3):
        try:
            with urllib.request.urlopen(request, timeout=30) as response:
                payload = json.loads(response.read().decode("utf-8"))
            if not isinstance(payload, dict):
                raise ScanError("Daraja GraphQL returned a non-object response")
            if payload.get("errors"):
                messages = "; ".join(
                    str(error.get("message", error)) for error in payload["errors"]
                )
                raise ScanError(f"Daraja GraphQL query failed: {messages}")
            return payload
        except urllib.error.HTTPError as exc:
            details = exc.read().decode("utf-8", errors="replace").strip()
            message = f"Daraja GraphQL returned HTTP {exc.code}"
            if details:
                message += f": {details[:500]}"
            if exc.code in (429, 500, 502, 503, 504) and attempt < 2:
                time.sleep(2**attempt)
                continue
            raise ScanError(message) from exc
        except (urllib.error.URLError, TimeoutError, json.JSONDecodeError) as exc:
            if attempt < 2:
                time.sleep(2**attempt)
                continue
            raise ScanError(f"Could not retrieve Daraja GraphQL metadata: {exc}") from exc
    raise ScanError("Daraja GraphQL query exhausted retries")


def fetch_catalog() -> list[dict[str, Any]]:
    payload = post_graphql(CATALOG_QUERY)
    catalog = payload.get("data", {}).get("getApis", {}).get("body")
    if not isinstance(catalog, list) or not catalog:
        raise ScanError("Daraja returned no API catalog entries")
    for api in catalog:
        if not isinstance(api, dict) or not api.get("alias") or not api.get("name"):
            raise ScanError("Daraja API catalog contains an entry without an alias or name")
    return catalog


def fetch_details(alias: str) -> dict[str, Any]:
    payload = post_graphql(DETAIL_QUERY, {"apiName": alias})
    result = payload.get("data", {}).get("populateApi")
    if not isinstance(result, dict) or not isinstance(result.get("body"), dict):
        raise ScanError(f"Daraja documentation for {alias} did not include a body")
    return result


def normalize_schema_candidate(value: Any) -> dict[str, Any] | None:
    """Accept only explicit JSON Schema objects; never infer a schema from prose/examples."""
    if isinstance(value, str):
        try:
            value = json.loads(value)
        except json.JSONDecodeError:
            return None
    if not isinstance(value, dict):
        return None
    if (
        value.get("type") == "object"
        and isinstance(value.get("properties"), dict)
        and all(isinstance(prop, dict) for prop in value["properties"].values())
    ):
        annotate_required_fields(value)
        return value
    return None


def annotate_required_fields(schema: dict[str, Any]) -> None:
    required_value = schema.get("required", [])
    required = set(required_value) if isinstance(required_value, list) else set()
    properties = schema.get("properties", {})
    if not isinstance(properties, dict):
        properties = {}
    for name, prop in properties.items():
        prop["requiredField"] = name in required
        annotate_required_fields(prop)
    items = schema.get("items")
    if isinstance(items, dict):
        annotate_required_fields(items)


def explicit_schemas(details: dict[str, Any]) -> dict[str, Any]:
    found: dict[str, Any] = {}
    body = details.get("body", {})
    for dataset in body.get("dataset") or []:
        if not isinstance(dataset, dict):
            continue
        mapping = dataset.get("mapping")
        if isinstance(mapping, str):
            try:
                mapping = json.loads(mapping)
            except json.JSONDecodeError:
                continue
        if not isinstance(mapping, dict):
            continue
        for name in ("requestSchema", "responseSchema", "callbackSchema", "timeoutSchema"):
            schema = normalize_schema_candidate(mapping.get(name))
            if schema:
                found[name] = schema
    return found


def normalize_headers(value: Any) -> set[str] | None:
    if isinstance(value, str):
        try:
            value = json.loads(value)
        except json.JSONDecodeError:
            return None
    if isinstance(value, dict):
        if isinstance(value.get("headers"), (dict, list)):
            return normalize_headers(value["headers"])
        names = set()
        for key, header in value.items():
            if isinstance(header, dict):
                name = header.get("name") or header.get("key")
                if name:
                    names.add(str(name).strip().lower())
            else:
                names.add(str(key).strip().lower())
        return names or None
    if isinstance(value, list):
        names = set()
        for header in value:
            if isinstance(header, str):
                names.add(header.strip().lower())
            elif isinstance(header, dict):
                name = header.get("name") or header.get("key") or header.get("header")
                if name:
                    names.add(str(name).strip().lower())
        return names or None
    return None


def explicit_headers(details: dict[str, Any]) -> set[str] | None:
    for dataset in details.get("body", {}).get("dataset") or []:
        if not isinstance(dataset, dict) or "headers" not in dataset:
            continue
        headers = normalize_headers(dataset["headers"])
        if headers:
            return headers
    return None


def find_csharp_type(type_name: str) -> tuple[str, str] | None:
    short_name = type_name.rsplit(".", 1)[-1]
    for path in (ROOT / "MpesaSdk").rglob("*.cs"):
        text = path.read_text(encoding="utf-8-sig")
        match = re.search(rf"\bclass\s+{re.escape(short_name)}\b", text)
        if not match:
            continue
        open_brace = text.find("{", match.end())
        if open_brace < 0:
            continue
        depth = 0
        for index in range(open_brace, len(text)):
            if text[index] == "{":
                depth += 1
            elif text[index] == "}":
                depth -= 1
                if depth == 0:
                    return str(path), text[open_brace + 1 : index]
    return None


def csharp_type_to_json(type_name: str) -> str:
    value = type_name.replace("?", "").strip()
    if value in ("string", "char", "Guid", "DateTime", "DateTimeOffset"):
        return "string"
    if value in ("byte", "sbyte", "short", "ushort", "int", "uint", "long", "ulong"):
        return "integer"
    if value in ("float", "double", "decimal"):
        return "number"
    if value == "bool":
        return "boolean"
    if value.startswith(("List<", "IList<", "IEnumerable<", "ICollection<")):
        return "array"
    return "object"


def local_type_fields(type_name: str, visited: set[str] | None = None) -> dict[str, Any] | None:
    visited = visited or set()
    short_name = type_name.rsplit(".", 1)[-1]
    if short_name in visited:
        return None
    source = find_csharp_type(short_name)
    if not source:
        return None
    visited.add(short_name)
    path, body = source
    del path
    properties: dict[str, Any] = {}
    pattern = re.compile(
        r'\[JsonPropertyName\("(?P<json>[^"]+)"\)\]\s*'
        r'(?P<attributes>(?:(?:\[[^\]]+\])\s*)*)public\s+'
        r'(?P<type>[\w.<>,?\[\]]+)\s+(?P<name>\w+)\s*\{',
        re.MULTILINE,
    )
    for match in pattern.finditer(body):
        declared_type = match.group("type")
        property_schema: dict[str, Any] = {
            "type": csharp_type_to_json(declared_type),
            "requiredField": bool(
                re.search(r"\[(?:Required|JsonRequired)(?:\]|\()", match.group("attributes"))
            ),
        }
        nested_type = declared_type.rsplit(".", 1)[-1].replace("?", "")
        is_collection = property_schema["type"] == "array"
        if is_collection:
            nested_type = declared_type[declared_type.find("<") + 1 : -1]
            property_schema["items"] = {"type": csharp_type_to_json(nested_type)}
        if property_schema["type"] == "object" and nested_type not in ("object", "JsonElement"):
            nested = local_type_fields(nested_type, visited.copy())
            if nested:
                nested["requiredField"] = property_schema["requiredField"]
                property_schema = nested
        elif is_collection and property_schema["items"]["type"] == "object":
            nested = local_type_fields(nested_type, visited.copy())
            if nested:
                property_schema["items"] = nested
        properties[match.group("json")] = property_schema
    if not properties:
        return None
    return {"type": "object", "properties": properties}


def schema_field_diff(remote: dict[str, Any], local: dict[str, Any]) -> list[str]:
    differences: list[str] = []

    def compare(remote_schema: dict[str, Any], local_schema: dict[str, Any], prefix: str) -> None:
        remote_properties = remote_schema.get("properties", {})
        local_properties = local_schema.get("properties", {})
        for name in sorted(remote_properties.keys() - local_properties.keys()):
            differences.append(f"added field `{prefix}{name}`")
        for name in sorted(local_properties.keys() - remote_properties.keys()):
            differences.append(f"field no longer documented `{prefix}{name}`")
        for name in sorted(remote_properties.keys() & local_properties.keys()):
            remote_property = remote_properties[name]
            local_property = local_properties[name]
            property_path = f"{prefix}{name}"
            remote_type = schema_type_signature(remote_property)
            local_type = schema_type_signature(local_property)
            if remote_type != local_type:
                differences.append(
                    f"type changed for `{property_path}`: `{local_type}` -> `{remote_type}`"
                )
            if remote_property.get("requiredField", False) != local_property.get("requiredField", False):
                local_required = "required" if local_property.get("requiredField", False) else "optional"
                remote_required = "required" if remote_property.get("requiredField", False) else "optional"
                differences.append(
                    f"requiredness changed for `{property_path}`: "
                    f"`{local_required}` -> `{remote_required}`"
                )
            if remote_type == "object":
                compare(
                    remote_property,
                    local_property,
                    f"{property_path}.",
                )
            elif remote_type.startswith("array:object"):
                compare(
                    remote_property.get("items", {}),
                    local_property.get("items", {}),
                    f"{property_path}[].",
                )

    compare(remote, local, "")
    return differences


def schema_type_signature(schema: dict[str, Any]) -> str:
    schema_type = schema.get("type", "object")
    if schema_type == "array":
        return f"array:{schema_type_signature(schema.get('items', {}))}"
    return str(schema_type)


def update_endpoint_constants(changes: list[dict[str, Any]], spec: dict[str, Any]) -> None:
    mappings = {
        entry["alias"]: entry.get("endpointConstant")
        for entry in spec.get("apis", [])
    }
    updates: dict[str, str] = {}
    for change in changes:
        if change["status"] != "MODIFIED_ENDPOINT":
            continue
        alias = canonical_alias(change["alias"], spec)
        constant = mappings.get(alias)
        if not constant:
            raise ScanError(f"No endpoint constant mapping exists for changed API {alias}")
        endpoint = normalize_endpoint(change["endpoint"])
        if not endpoint:
            raise ScanError(f"Portal supplied an empty endpoint for changed API {alias}")
        previous = updates.get(constant)
        if previous is not None and previous != endpoint:
            raise ScanError(f"Conflicting portal endpoints map to MpesaRequestEndpoint.{constant}")
        updates[constant] = endpoint

    if not updates:
        return
    path = ROOT / "MpesaSdk" / "MpesaRequestEndpoint.cs"
    with path.open("r", encoding="utf-8-sig", newline="") as source:
        content = source.read()
    for constant, endpoint in updates.items():
        pattern = re.compile(
            rf'(?m)(public\s+static\s+string\s+{re.escape(constant)}\s*\{{\s*get;\s*set;\s*\}}\s*=\s*")[^"]*(";)'
        )
        content, substitutions = pattern.subn(
            lambda match: match.group(1) + endpoint + match.group(2), content
        )
        if substitutions != 1:
            raise ScanError(
                f"Expected exactly one MpesaRequestEndpoint.{constant} declaration; "
                f"found {substitutions}"
            )
    with path.open("w", encoding="utf-8-sig", newline="") as destination:
        destination.write(content)


def normalize_endpoint(value: Any) -> str:
    endpoint = str(value or "").strip()
    if not endpoint:
        return ""
    parsed = urllib.parse.urlsplit(endpoint)
    if parsed.scheme and parsed.netloc:
        return parsed.path.lstrip("/") + (f"?{parsed.query}" if parsed.query else "")
    return endpoint.lstrip("/")


def canonical_alias(alias: str, spec: dict[str, Any]) -> str:
    for group in spec.get("targetAliasGroups", []):
        if alias in group.get("aliases", []) and group.get("canonicalAlias"):
            return group["canonicalAlias"]
    return alias


def classify_changes(
    catalog: list[dict[str, Any]],
    spec: dict[str, Any],
    scoped_aliases: set[str] | None = None,
) -> list[dict[str, Any]]:
    expected = {
        canonical_alias(entry["alias"], spec): entry
        for entry in spec.get("apis", [])
        if scoped_aliases is None or entry["alias"] in scoped_aliases
    }
    observed = {
        canonical_alias(entry["alias"], spec): entry
        for entry in catalog
        if scoped_aliases is None or entry["alias"] in scoped_aliases
    }
    changes: list[dict[str, Any]] = []
    for alias, actual in observed.items():
        local = expected.get(alias)
        endpoint = normalize_endpoint(actual.get("endpoint"))
        method = (actual.get("operation") or "").upper()
        if local is None:
            if not endpoint:
                continue
            changes.append(
                {
                    "alias": alias,
                    "name": actual["name"],
                    "status": "NEW_ENDPOINT",
                    "endpoint": endpoint,
                    "method": method,
                    "field_changes": "Endpoint is not mapped in the local inventory.",
                }
            )
            continue
        fields: list[str] = []
        local_endpoint = normalize_endpoint(local.get("endpoint"))
        if endpoint and endpoint != local_endpoint:
            fields.append(f"endpoint: `{local_endpoint}` -> `{endpoint}`")
        if method != local.get("method", "").upper():
            fields.append(f"method: `{local.get('method', '')}` -> `{method}`")
        if fields:
            changes.append(
                {
                    "alias": alias,
                    "name": actual["name"],
                    "status": "MODIFIED_ENDPOINT",
                    "endpoint": endpoint,
                    "method": method,
                    "field_changes": "; ".join(fields),
                }
            )
    for alias, local in expected.items():
        if alias not in observed:
            changes.append(
                {
                    "alias": alias,
                    "name": local.get("name", alias),
                    "status": "REMOVED_OR_HIDDEN",
                    "endpoint": normalize_endpoint(local.get("endpoint")),
                    "method": local.get("method", ""),
                    "field_changes": "Previously cataloged API is absent from the portal catalog.",
                }
            )
    return sorted(changes, key=lambda item: (item["status"], item["alias"]))


def render_changelog(
    scanned_at: str, status: str, changes: list[dict[str, Any]], notes: list[str]
) -> str:
    rows = [
        "| Endpoint/API | Status | Method | Field changes |",
        "|---|---|---:|---|",
    ]
    if changes:
        rows.extend(
            f"| {item['name']} (`{item['alias']}`) - `{item['endpoint'] or 'endpoint unavailable'}` "
            f"| {item['status']} | {item['method'] or 'unknown'} | {item['field_changes']} |"
            for item in changes
        )
    else:
        rows.append("| - | - | - | No endpoint or method changes detected. |")
    if notes:
        rows.extend(["", "### Scan notes", ""])
        rows.extend(f"- {note}" for note in notes)
    return "\n".join(
        [
            "# Daraja API Change Log",
            "",
            f"- **Date of Scan:** {scanned_at}",
            f"- **Detected Status:** `{status}`",
            "",
            "## Detailed Diff",
            "",
            *rows,
            "",
        ]
    )


def update_github_summary(report: str) -> None:
    summary_path = os.environ.get("GITHUB_STEP_SUMMARY")
    if summary_path:
        with open(summary_path, "a", encoding="utf-8") as stream:
            stream.write(report)


def run_scan(spec: dict[str, Any]) -> tuple[str, list[dict[str, Any]], list[str]]:
    catalog = fetch_catalog()
    groups = spec.get("targetAliasGroups", [])
    scoped_aliases = set(spec.get("targetAliases", []))
    for group in groups:
        scoped_aliases.update(group.get("aliases", []))
    changes = classify_changes(catalog, spec, scoped_aliases)
    notes: list[str] = []
    local_apis = {entry["alias"]: entry for entry in spec.get("apis", [])}
    target_aliases = set(spec.get("targetAliases", []))
    observed_aliases = {entry["alias"] for entry in catalog}
    missing_targets = sorted(target_aliases.difference(observed_aliases))
    if missing_targets:
        notes.append("Target APIs not returned by the public catalog: " + ", ".join(missing_targets))
    for group in groups:
        aliases = set(group.get("aliases", []))
        if aliases and not aliases.intersection(observed_aliases):
            notes.append(
                f"Target suite {group.get('suite', 'unknown')} has no matching public catalog entry "
                f"(expected one of: {', '.join(sorted(aliases))})."
            )

    selected = [entry for entry in catalog if entry["alias"] in target_aliases]
    for group in groups:
        group_aliases = set(group.get("aliases", []))
        selected.extend(
            entry
            for entry in catalog
            if entry["alias"] in group_aliases and entry not in selected
        )
    scope_gaps = spec.get("scopeGaps", [])
    notes.extend(f"Coverage gap: {gap}" for gap in scope_gaps)
    for api in selected:
        try:
            documentation = fetch_details(api["alias"])
        except ScanError as exc:
            notes.append(f"Could not inspect request/response/callback schemas for {api['alias']}: {exc}")
            break
        local = local_apis.get(api["alias"], {})
        local = local_apis.get(canonical_alias(api["alias"], spec), local)
        header_profile = local.get("headersProfile")
        expected_headers = set(spec.get("headerProfiles", {}).get(header_profile, []))
        actual_headers = explicit_headers(documentation)
        if expected_headers and actual_headers is None:
            notes.append(
                f"{api['alias']} has no machine-readable request-header mapping in the "
                "public documentation response."
            )
        elif actual_headers is not None and expected_headers != actual_headers:
            differences = [
                f"header added `{name}`"
                for name in sorted(actual_headers - expected_headers)
            ] + [
                f"header no longer documented `{name}`"
                for name in sorted(expected_headers - actual_headers)
            ]
            changes.append(
                {
                    "alias": api["alias"],
                    "name": api["name"],
                    "status": "MODIFIED_HEADERS",
                    "endpoint": normalize_endpoint(api.get("endpoint")),
                    "method": (api.get("operation") or "").upper(),
                    "field_changes": "; ".join(differences),
                }
            )
        schemas = explicit_schemas(documentation)
        if not schemas:
            notes.append(
                f"{api['alias']} has no explicit JSON Schema in the public documentation response; "
                "prose/examples are not treated as machine-verifiable schemas."
            )
            continue

        type_names = {
            "requestSchema": "requestType",
            "responseSchema": "responseType",
            "callbackSchema": "callbackType",
            "timeoutSchema": "timeoutType",
        }
        for schema_name, remote_schema in schemas.items():
            type_key = type_names[schema_name]
            local_type = local.get(type_key)
            local_schema = local_type_fields(local_type) if local_type else None
            if local_schema is None:
                notes.append(
                    f"{api['alias']} has an explicit {schema_name} but no parseable local "
                    f"{type_key} mapping; code generation requires review."
                )
                continue
            field_changes = schema_field_diff(remote_schema, local_schema)
            if field_changes:
                changes.append(
                    {
                        "alias": api["alias"],
                        "name": api["name"],
                        "status": "MODIFIED_SCHEMA",
                        "endpoint": normalize_endpoint(api.get("endpoint")),
                        "method": (api.get("operation") or "").upper(),
                        "field_changes": "; ".join(field_changes),
                    }
                )
    changes = sorted(changes, key=lambda item: (item["status"], item["alias"]))
    has_noncoverage_notes = any(not note.startswith("Coverage gap:") for note in notes)
    if has_noncoverage_notes and any(
        change["status"] == "MODIFIED_ENDPOINT" for change in changes
    ):
        notes.append(
            "Endpoint metadata changes were not applied because detailed API documentation "
            "could not be fully verified."
        )
    if missing_targets or has_noncoverage_notes:
        status = "SCAN_INCOMPLETE"
    elif any(
        change["status"] != "MODIFIED_ENDPOINT"
        or "method:" in change["field_changes"]
        for change in changes
    ):
        unsupported = sorted(
            {
                change["status"]
                if change["status"] != "MODIFIED_ENDPOINT"
                else "HTTP_METHOD_CHANGE"
                for change in changes
                if change["status"] != "MODIFIED_ENDPOINT"
                or "method:" in change["field_changes"]
            }
        )
        notes.append(
            "Verified changes need code-generation support before an SDK PR can be opened: "
            + ", ".join(unsupported)
        )
        status = "SCAN_INCOMPLETE"
    elif changes:
        status = "PARTIAL_UPDATES_DETECTED" if scope_gaps else "UPDATES_DETECTED"
    elif scope_gaps:
        status = "SCAN_INCOMPLETE"
    else:
        status = "NO_CHANGES"
    if status in ("UPDATES_DETECTED", "PARTIAL_UPDATES_DETECTED"):
        try:
            update_endpoint_constants(changes, spec)
        except ScanError as exc:
            notes.append(str(exc))
            status = "SCAN_INCOMPLETE"
    return status, changes, notes


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--spec", type=Path, default=SPEC_PATH)
    parser.add_argument("--report", type=Path, default=CHANGELOG_PATH)
    parser.add_argument("--offline", type=Path, help="Read a saved catalog fixture instead of the portal.")
    args = parser.parse_args()

    try:
        spec = load_json(args.spec)
        if args.offline:
            fixture = load_json(args.offline)
            catalog = fixture.get("catalog")
            if not isinstance(catalog, list):
                raise ScanError("Offline fixture must contain a catalog array")
            changes = classify_changes(catalog, spec)
            notes = fixture.get("notes", [])
            status = fixture.get("status", "SCAN_INCOMPLETE" if notes else ("UPDATES_DETECTED" if changes else "NO_CHANGES"))
        else:
            status, changes, notes = run_scan(spec)
    except ScanError as exc:
        status = "SCAN_INCOMPLETE"
        changes = []
        notes = [str(exc)]

    scanned_at = datetime.now(timezone.utc).strftime("%Y-%m-%d")
    report = render_changelog(scanned_at, status, changes, notes)
    update_github_summary(report)
    print(report)
    if status != "NO_CHANGES":
        args.report.parent.mkdir(parents=True, exist_ok=True)
        args.report.write_text(report, encoding="utf-8")
    if status == "SCAN_INCOMPLETE":
        return 2
    return 0


if __name__ == "__main__":
    sys.exit(main())
