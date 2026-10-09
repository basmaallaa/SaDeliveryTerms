# AxiomSys: Sales Delivery Terms (Assessment)

Candidate solution for Part D of the Junior Developer Assessment.

The original repository was not provided, so this repo recreates the
four-layer structure from the assessment pack. The work is in the branch
`feature/sales-delivery-terms`.

## Fixed development user (IMPORTANT)

The real login system was not available, so the API has a **dev-only login**
with a **single fixed user**. It exists only to demonstrate the JWT, the 401
check and the sign-in redirect.

| Item | Value |
|---|---|
| Username / password | Read from configuration: `DevLogin:Username` and `DevLogin:Password` (user-secrets, never committed) |
| User id inside the token (`Id` claim) | **1** (hard-coded) |
| Company code inside the token (`CompanyCode` claim) | **1** (hard-coded) |
| Endpoint | `POST api/Auth/Login` returns `{ "token": "..." }` |
| Token lifetime | `Jwt:ExpiryMinutes` (480) |

Consequences:
- Every record added or edited gets `ENTRY_USER` / `CHANGE_USER` = **1**.
- Anyone who knows the dev username and password is "user 1".
- This must be removed and replaced by the existing authentication in the real repository.

## Structure

| Project | Role |
|---|---|
| DeliveryTermsDL | Entity, EF Core context (Oracle), UnitOfWork |
| DeliveryTermsBL | Models, services, ResponseModel, JwtTokenService |
| DeliveryTermsSL | Web API (JWT, Swagger, Sales group) |
| DeliveryTermsPortal | Razor Pages UI (en / ar, RTL, DataTable) |

## Setup

1. Run the `CREATE TABLE SA_DELIVERY_TERMS` script from the assessment pack in your Oracle schema.
2. Set the secrets for the API (nothing is committed):

```bash
dotnet user-secrets init --project DeliveryTermsSL
dotnet user-secrets set "ConnectionStrings:SupplyChain" "User Id=<user>;Password=<password>;Data Source=localhost:1521/<service>" --project DeliveryTermsSL
dotnet user-secrets set "Jwt:Key" "<at least 32 characters>" --project DeliveryTermsSL
dotnet user-secrets set "DevLogin:Username" "<dev user>" --project DeliveryTermsSL
dotnet user-secrets set "DevLogin:Password" "<dev password>" --project DeliveryTermsSL
```

3. In `DeliveryTermsPortal/appsettings.json`, set `Uri` to the API base, for example `https://localhost:<api-port>/api/`.
4. Run both `DeliveryTermsSL` and `DeliveryTermsPortal` (Multiple startup projects).
5. Open `/en/Accounts/Login` or `/ar/Accounts/Login`, sign in with the dev user, then go to `/en/SALES/DELIVERYTERMS/Index`.

## Notes and deviations

- `AuthorizeCompany` is a simple stand-in (the table has no `COMPANY_CODE`).
- Label keys 32, 5583, 18, 3, 4, 5 are used as requested; keys from 9001 are my own because the real label table was not available.
