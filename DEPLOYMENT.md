# Railway production configuration

Set these Railway variables; do not place their values in `appsettings.json`.

```text
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<Supabase PostgreSQL connection string>
Jwt__Key=<long random secret>
Jwt__Issuer=LostAndFoundAPI
Jwt__Audience=LostAndFoundAPIUsers
EmailSettings__Email=<Gmail sender address>
EmailSettings__AppPassword=<Gmail app password>
EmailSettings__Host=smtp.gmail.com
EmailSettings__Port=587
Cors__AllowedOrigins__0=https://lost-and-found-client-eight.vercel.app
```

For Vercel, set `VITE_API_URL` to:

```text
https://lostandfoundapi-production.up.railway.app/api
```
