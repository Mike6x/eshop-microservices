docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d   
docker-compose down
dotnet dev-certs https --trust     
dotnet ef migrations add initial --project Identity.App --output-dir Data/Migrations


7000 yarp proxy
7001 Identity
7002 FontEnd UI
7003 Backend UI