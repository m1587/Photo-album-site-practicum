# בחר .NET SDK כבסיס לבנייה
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# העתק את קבצי הפרויקט לקונטיינר
COPY . .

# הרץ את פקודת ה-restore
RUN dotnet restore

# בנה את הפרויקט
RUN dotnet publish -c Release -o /app/out

# בחר .NET ASP.NET כבסיס להפעלת האפליקציה
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/out .

# הגדר את הפורט
EXPOSE 7123

# הפעל את האפליקציה
ENTRYPOINT ["dotnet", "WebApi.dll"]
