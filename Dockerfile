FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/DuAnCNPM.dll"]