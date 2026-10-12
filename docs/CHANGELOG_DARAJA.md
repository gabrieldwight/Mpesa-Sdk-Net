# Daraja API Change Log

- **Date of Scan:** 2026-10-12
- **Detected Status:** `SCAN_INCOMPLETE`

## Detailed Diff

| Endpoint/API | Status | Method | Field changes |
|---|---|---:|---|
| Business To Customer (B2C) (`BusinessToCustomer`) - `mpesa/b2c/v3/paymentrequest` | MODIFIED_ENDPOINT | POST | endpoint: `mpesa/b2c/v1/paymentrequest` -> `mpesa/b2c/v3/paymentrequest` |
| Customer To Business (C2B) (`CustomerToBusiness`) - `mpesa/c2b/v2/registerurl` | MODIFIED_ENDPOINT | POST | endpoint: `mpesa/c2b/v1/registerurl` -> `mpesa/c2b/v2/registerurl` |

### Scan notes

- Coverage gap: Mini Apps are presented on a separate portal page and are not represented by the public getApis catalog; no machine-readable endpoint/schema source has been verified for that suite.
- Could not inspect request/response/callback schemas for Authorization: Daraja GraphQL returned HTTP 400: {"errors":[{"message":"Something went wrong. Please try again later."}]}
- Endpoint metadata changes were not applied because detailed API documentation could not be fully verified.
