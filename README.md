# authentication-web-api

> 1. Clone the project

> 2. Modify AuthenticationWebApi/appsettings.json
  change ConnectionStrings.DefaultConnection to your db_postgres_connection_string

> 3. Add middleware to your project, you can copy and modify according your project the AuthenticationWebApi/Helpers/Middleware/ErrorHandlerMiddleware.cs
>> Copy all dependencies you need for your middleware from this authentication project, also custom attributes from AuthenticationWebApi/Helpers/Attribute/ and use them on your project(e.g. on controller) to enable identification

> 4. To work correctly, you also need to have the following tables in your database:
>> ![image](https://github.com/Akhmedzhanov-Shakhzod/authentication-web-api/assets/80168982/5a6affed-c164-4ba3-aef4-5168a9697f56)

> 5. Run authentication project and your
