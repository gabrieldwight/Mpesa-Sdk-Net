# Daraja API scanner

The scanner compares Safaricom's public Daraja GraphQL catalog with
`daraja-spec.json`. It checks endpoint and HTTP-method metadata, structured
request-header names, then compares explicit JSON Schemas from the
documentation resolver with the mapped C# properties. It does not infer
schemas from prose or example payloads.

Run it locally with:

```powershell
python -m unittest discover -s tools\daraja-scanner\tests -v
dotnet run --project tools\daraja-scanner\smoke\MpesaSdkSmoke.csproj
python tools\daraja-scanner\daraja-api-scan.py
```

Exit code `2` means the scan is incomplete and the changelog records the
blocker. Unverified schemas, unsupported changes, and missing catalog entries
fail closed so the workflow cannot present them as successful parity or open a
PR. A successful endpoint-only change updates exactly one mapped endpoint
constant; schema, method, and new-API changes require implementation support
before a PR is opened.

The GitHub Actions workflow runs weekly and on manual dispatch. It builds the
SDK and ASP.NET sample before opening or updating a reviewable PR and never
auto-merges. `NO_CHANGES` is included in the workflow summary but does not
rewrite the dated changelog or create a PR.

The portal's documentation resolver currently fails during live scans, and
Mini Apps are not present in the public API catalog. Those limitations are
reported in `docs/CHANGELOG_DARAJA.md`; the scanner does not claim full
integration parity while either remains unresolved.
