# EShopMicroservices

A Udemy .NET Microservices tutorial app.

## Ports

The port numbers for microservices are as follows:

| Microservice | Local Env   | Docker Env  | Docker Inside |
| ------------ | ----------- | ----------- | ------------- |
| Catalogue    | `5000` / `5050` | `6000` / `6060` | `8080` / `8081`   |
| Basket       | `5001` / `5051` | `6001` / `6061` | `8080` / `8081`   |
| Discount     | `5002` / `5052` | `6002` / `6062` | `8080` / `8081`   |
| Ordering     | `5003` / `5053` | `6003` / `6063` |               |

ASP.NET Core ports are listed as HTTP / HTTPS for running application.

In local development, for example, the catalog microservices will expose services on port `5000` for HTTP and `5050` for HTTPS.

Once local development is complete, we'll shift to a Docker environment. In Docker, the HTTP service will be exposed on port `6000`, and HTTPS on port `6060`.

This setup remains consistent across microservices as these port numbers are specified in the Dockerfile's environment variables for any ASP.NET application. Internally, Docker will use ports `80`, `8080`, and `8081` for HTTP and HTTPS, while externally, we expose the services on ports starting from `60000` for `access`.