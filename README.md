

# Clean Architecture Template

## Visual Studio Windows Setup - local
1. Set docker-compose as the startup project and press start

## Docker - local
1. Pull latest postgres container
2. Run postgres container on port `5432` with `postgres` user with `postgres` password
3. Connect to container and create `template` and `template_identity` databases
4. Grant all privileges on database `template` and `template_identity` to `postgres` user
5. Configure env variables, build api docker image and run it

## Using in-memory database
To use in memory database set either appsetting or .env variables to:

    UseInMemoryDatabase = true
    Migrate = false
    Seed = true

## Using the real database
To use real database set either appsetting or .env variables to:

    UseInMemoryDatabase = false
    Migrate = true
    Seed = true

## Database migrations
Installing the tools:

    dotnet tool install --global dotnet-ef

Updating the tools:

    dotnet tool update --global dotnet-ef

Initial migration example that will output the migrations inside Infrastructure/Data/Application for App context:

    dotnet ef migrations add InitialCreate --context ApplicationContext --output-dir Data/Migrations/Application -p Infrastructure -s Api

Migrations can be applied automatically on startup by setting  `Migrate = true` or they can be applied manually using the following commands for specific context:

    dotnet ef database update --context ApplicationContext -p Api

## Environment configuration
.env file example:

    API_EXPOSED_PORT=
    API_PORT=
    POSTGRES_USERNAME=
    POSTGRES_PASSWORD=
    POSTGRES_EXPOSED_PORT=
    POSTGRES_PORT=
    REDIS_EXPOSED_PORT=
    REDIS_PORT=
    DATABASE__USEINMEMORYDATABASE=
    DATABASE__MIGRATE=
    DATABASE__SEED=
    CONNECTIONSTRINGS__DEFAULTCONNECTION=
    BASEURL__APIBASE=
    BASEURL__WEBBASE=
    AMADEUS__AUTHCLIENTID=
    AMADEUS__AUTHCLIENTSECRET=