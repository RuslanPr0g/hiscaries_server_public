cd ../src
dotnet ef migrations add %1 --startup-project HC.API/HC.API.csproj --project HC.Infrastructure/HC.Infrastructure.csproj -c HiscaryContext
cd ../scripts