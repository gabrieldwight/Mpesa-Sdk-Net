# Daraja API audit log

Scans compare the Safaricom portal with the repository-derived endpoint inventory in `src/daraja/schema.json`. The inventory is not an authoritative Safaricom schema. A change is considered verified only when parsed from a machine-readable document fetched from an official `safaricom.co.ke` host.

## 2026-11-25 — `PORTAL_UNAVAILABLE`

- **Date of scan:** 2026-11-25
- **Source:** <https://developer.safaricom.co.ke/apis>
- **Detected status:** `PORTAL_UNAVAILABLE`
- **Result:** The portal could not be fetched in this session because web and direct HTTP requests were denied pending interactive permission. This is **not** a `NO_CHANGES` result. No API differences or missing endpoint schemas are marked verified.
- **Local inventory:** 18 endpoint operations mapped from the current `MpesaSdk` implementation. The endpoint and field details are repository-derived and remain unverified against the live portal.

| API suite / local operations | Local SDK coverage | Portal diff | Field changes |
|---|---|---|---|
| Authorization / OAuth token | `GetAuthTokenAsync` / `GetAuthToken` | UNKNOWN | Not fetched |
| M-Pesa Express / STK Push and query | `MakeLipaNaMpesaOnlinePayment*`, `QueryLipaNaMpesaTransaction*` | UNKNOWN | Not fetched |
| C2B / register URL and simulate | `RegisterC2BUrl*`, `MakeC2BPayment*` | UNKNOWN | Not fetched |
| B2C | `MakeB2CPayment*` | UNKNOWN | Not fetched |
| B2B | `MakeB2BPayment*` | UNKNOWN | Not fetched |
| Transaction status and account balance | `QueryMpesaTransactionStatus*`, `QueryAccountBalance*` | UNKNOWN | Not fetched |
| Reversal | `ReverseMpesaTransaction*` | UNKNOWN | Not fetched |
| Dynamic QR | `GenerateDynamicMpesaQR*` | UNKNOWN | Not fetched |
| Pull Transactions / register and query | `RegisterPullTransaction*`, `QueryPullTransaction*` | UNKNOWN | Not fetched |
| Bill Manager / onboarding, invoicing, reconciliation | Onboarding, single/bulk invoice, update, reconciliation calls | UNKNOWN | Not fetched |
| Mini Apps / Daraja 3.0 | No corresponding local mapping found in the scoped inventory | UNKNOWN | Not fetched |

No SDK implementation changes or code previews were generated because no portal differences could be verified.
