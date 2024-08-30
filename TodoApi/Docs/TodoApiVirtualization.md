# 1 Dockerfile
The Dockerfile used:
```Dockerfile title:TodoApp/Dockerfile
# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore "./TodoApi.csproj"
COPY [".", "."]
RUN dotnet build "./TodoApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "./TodoApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
```

[Dockerfile reference](https://docs.docker.com/reference/dockerfile/) contains the description of the instructions used.
## 1.1 Base Image
Relevant Dockerfile section:
```Dockerfile title:TodoApp/Dockerfile#1_BaseImage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
```

The `aspnet:8.0` is the foundational image that the application will use to run. It includes the ASP.NET Core runtime (which is necessary for running .NET applications), it is lightweight and optimized for executing .NET applications in a production environment.

The `AS` instruction names this stage as `base` so that reference to it can be made later in the Dockerfile. This allows for multi-stage builds, which help in keeping the final image smaller and more efficient.

Instruction `USER` sets the user and group ID, since by default, if no `USER` is specified, Docker will run commands as the root user, which can pose significant security risks. Registered users can be viewed in the container with the `cat ../etc/passwd` command. 

Instruction `WORKDIR` changes the working directory.

Exposing a Docker port with the `EXPOSE` instruction means advertising it as actively used by the containerized workload. It’s important to realize that exposing a port *does not automatically publish it* to your host. Exposing ports can appear redundant, as ports can be published without being exposed first. However, `EXPOSE` remains an important documentation mechanism that makes the containers easier to manage and reason about.
## 1.2 Build Image
Relevant Dockerfile section:
```Dockerfile title:TodoApp/Dockerfile#2-BuildImage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore "./TodoApi.csproj"
COPY [".", "."]
RUN dotnet build "./TodoApi.csproj" -c $BUILD_CONFIGURATION -o /app/build
```

The `sdk:8.0` image includes the full .NET SDK, which contains all the tools and libraries necessary to build and compile the .NET application. This image compiles the source code into an executable application, which will later be transferred to the base image for runtime.

The `ARG` instruction defines an argument `BUILD_CONFIGURATION` with a default value of `Release`. This argument can be passed when building the Docker image to control the build configuration, typically either `Release` or `Debug`. The `Release` configuration is optimized for production, providing better performance and smaller binaries.

The following `COPY` command copies the project file `TodoApi.csproj` from the local machine into the current working directory (`/src`) inside the Docker container. The *reason for copying only the `.csproj` file* is that the `.csproj` file contains the list of dependencies required for the project, and by copying only the `.csproj` file initially, it is possible to take advantage of Docker's layer caching. If the dependencies in the `.csproj` file haven't changed, *Docker will cache* the results of the `dotnet restore` step, *speeding up subsequent builds*.

The `RUN dotnet restore` command restores all the NuGet packages (dependencies) listed in the `TodoApi.csproj` file. It downloads and installs these packages into the Docker container. The restore process ensures that all the necessary libraries and tools that your project depends on are available. This step effectively *prepares the environment for the application to be built*.

The second `COPY` command copies all the remaining files from the local directory (where the Docker build is being executed) into the current working directory inside the Docker container (`/src`). The purpose of this is that, after restoring dependencies, it is necessary to compile the rest of the application's source code. This command transfers the entire project codebase into the container.
## 1.3 Publish Image
Relevant Dockerfile section:
```Dockerfile title:TodoApp/Dockerfile#3-PublishImage
FROM build AS publish
RUN dotnet publish "./TodoApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
```

The `FROM...` line begins a new stage in the Dockerfile, labeled `publish`, which uses the output of the previous `build` stage as its base. The separation into different stages (`build` and `publish`) helps keep the final image clean and smaller by including only what’s necessary for running the application.

The `RUN...` instruction uses the `dotnet publish` command to compile the application and prepare it for deployment. Details:
- **`dotnet publish`**: This command compiles the code, copies the necessary dependencies, and packages the application into a self-contained format ready for deployment.
- **`-c $BUILD_CONFIGURATION`**: Specifies the build configuration (`Release` or `Debug`) defined earlier as an argument.
- **`-o /app/publish`**: Defines the output directory (`/app/publish`) where the published files will be stored inside the container.
- **`/p:UseAppHost=false`**: This option instructs the .NET CLI to skip generating the app host (executable) for the platform. This is typically used in containerized environments where you don’t need a platform-specific executable.
## 1.4 Final Image
Relevant Dockerfile section:
```Dockerfile title:TodoApp/Dockerfile#4-FinalImage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
```

The `COPY` instruction copies the output of the `publish` stage (i.e., the compiled application files) from `/app/publish` in the `publish` stage to the current directory (`/app`) in the `final` stage. The `--from=publish` directive indicates that the files should be copied from the `publish` stage.

The `ENTRYPOINT` line sets the entry point for the container, which specifies the command that will be run when the container starts. Here, it runs the `dotnet` command with `TodoApi.dll` as the argument, effectively launching the ASP.NET Core application.

# 2 Docker Compose File
The `docker-compose.yml` file orchestrates the deployment of your application, defining how the container runs and interacts with the host.

The `docker-compose.yml` file used:
```yaml title:docker-compose.yml
version: '3.8'

services:
  todoapi:
    image: todoapi:latest
    build:
      context: ../TodoApi  # Path to the project relative to docker-compose.yml path
      dockerfile: Dockerfile # name of the Dockerfile
    ports:
      - "8080:8080"  # Host:Container
      - "8081:8081"  # Host:Container
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ASPNETCORE_Kestrel__Certificates__Default__Path: "/https/aspnetapp.pfx"
      ASPNETCORE_Kestrel__Certificates__Default__Password: "password123"
    volumes:
      - ${USERPROFILE}/.aspnet/https/aspnetapp.pfx:/https/aspnetapp.pfx
```

Lines 3-8 define a service named `todoapi` and specify that it should use the `todoapi:latest` image. If the image doesn’t exist, it builds it from the provided context and Dockerfile.

Lines 9-11 map ports from the host to the container, allowing one to access the application via `localhost:8080` and `localhost:8081`.

Lines 12-15 set up the environment inside the container. This includes binding the application to specific URLs and providing paths and passwords for SSL certificates. Environment variables in a sorted list can be viewed inside the container with the following command:
```shell title:env
env -0 | sort -z | tr '\0' '\n'
```

Lines 16-17 mount the SSL certificate from the local machine into the container at runtime, ensuring that the application can access it securely.
# 3 Interaction between the Dockerfile and the Docker Compose File
As far as the *building and running* are concerned, the `docker-compose.yml` file uses the Dockerfile to build the image and then runs it according to the specifications you’ve defined. It handles the entire lifecycle of the container—from building, configuring, and starting it, to mapping ports and handling volumes.

Regarding *environment and configuration*, the environment variables and configurations set in the `docker-compose.yml` file ensure that when the container runs, it does so in a controlled and reproducible environment. For instance, the SSL certificate paths and URLs are configured so that the application runs with HTTPS enabled.

In short:
- *Dockerfile* focuses on creating a Docker image by compiling and packaging the application, while
- *Docker Compose* manages the configuration and orchestration of containers, defining how the Docker image should run, including network ports, environment variables, and volumes.

Together, they allow one to easily build, configure, and deploy a .NET application in a consistent environment.