# Stock Web App

This project is created for learning and practicing technologies. A server and the corresponding client is implemented to 
simulate a stock exchange website.

| Dashboard                | Stock details             |
|--------------------------|---------------------------|
|![pic1](./screenshot1.png) | ![pic2](./screenshot2.png) |

# Technologies
- Server is an ASP.NET WebApi (.NET6)
- Client is a ReactJS application (TypeScript, React Hooks, SCSS)
- Both client and server can be run in docker
- For live stock data Websocket connection is used

# Setting up

## Setting up locally

You can run the server from Visual Studio. On the first run accept the creation of a self-signed certificate.
The server will run on `https://localhost:3001`. A Swagger is also accessible at `https://localhost:3001/swagger/index.html`.

For the client npm packages have to be installed with the `npm install` command.
After that you can start the client with the `npm run start` command. The client will run at `http://localhost:3000/`

## Setting up for docker
For running the server in docker an SSL certificate is needed. You can create a self-signed sertificate for local development 
with the `dotnet dev-certs` command.

1. Clean existing certificates if any: `dotnet dev-certs https --clean`
2. Create a certificate under the `httpcert` directory with the following command:
    
    `dotnet dev-certs https -ep .\httpcert\StockWebApp.pfx -p <YOUR PASSWORD>`

3. Trust the certificate: `dotnet dev-certs https --trust`
4. Update the `ASPNETCORE_Kestrel__Certificates__Default__Password` environment variable in the `docker-compose.yml` to the password given for the certificate.

For running the instances in docker just simply run the `docker-compose up` command (from the directory of `docker-compose.yml`).
Both client and server will run in a container. The server will run on `https://localhost:3001` while the client on `http://localhost:3000`
The certificate created previously will be mounted to the server directory `/https/` via docker volume.


# Architecture
