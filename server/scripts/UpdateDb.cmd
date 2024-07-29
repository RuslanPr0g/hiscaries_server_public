cd ../src
dotnet ef database update -p HC.Infrastructure/HC.Infrastructure.csproj -s HC.API/HC.API.csproj -c HiscaryContext
cd ../scripts