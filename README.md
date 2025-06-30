This repository contains a small ASP.NET Core backend that demonstrates basic sales tax calculations.
Items are parsed from a simple text format, taxes are applied according to their type and whether they are imported, and the API returns the resulting receipt data.

## Running the project

1. Ensure you have the [.NET SDK 9](https://dotnet.microsoft.com/) installed.
2. Build and start the API:

   ```
      I Advice starting CSharpSales with IIS Express
   ```

   The application listens on `https://localhost:44310` by default (check the console output for the exact port).

## Testing the API

The service exposes a single POST endpoint:

`POST /GetCartResponse`

Several options are available to exercise the API:

* **Swagger UI** – browse to `https://localhost:44310/swagger` after the app starts and invoke the endpoint directly from the documentation page.
* **Postman** – send a POST request to `https://localhost:44310/GetCartResponse` then select raw body and finally select JSON.
* **AWS** - http://sales-develop.us-east-1.elasticbeanstalk.com/GetCartResponse
* **AWS** - http://sales-develop.us-east-1.elasticbeanstalk.com/swagger
* **VM** - http://sales.raphp.net/GetCartResponse
* **VM** - https://sales.raphp.net/swagger

* **HTTP Client** – Or visit https://angular.raphp.net/ to use the Angular frontend client.

Example:

```json
{
  "items": [
    "2 book at 12.49",
    "1 music CD at 14.99",
    "1 chocolate bar at 0.85"
  ]
}
```

And this should be returned:

```json
{
    "items": [
        "2 book: 24,98",
        "1 music CD: 16,49",
        "1 chocolate bar: 0,85"
    ],
    "salesTaxes": 1.5000,
    "total": 42.3200
}
```

### Automated Deployment

This application is automatically deployed via a GitHub Actions pipeline configured in `.github/workflows/deploy.yml` and `.github/workflows/deploy-aws.yml`.

The pipeline handles the build and deployment processes for both AWS Elastic Beanstalk and a private VM.
Each push to the `master` branch triggers the build and deploy steps:

1. **Build and Publish:** Compiles the .NET9 application.
2. **Deploy via SSH:** Copies the published binaries to the production server.
3. **Service Restart:** Restarts the application on the server via SSH.

The deployment pipeline ensures continuous delivery of the latest version directly to the production environment.

## Extra

An ulterior solution written in PHP is available at https://github.com/lurgoyf82/PhpSales, it is a simple PHP script 
that runs on docker ... it was more of a proof of concept than a real solution, but it is there for reference.

## License

This project is released under the MIT License. See `LICENSE.txt` for details.
