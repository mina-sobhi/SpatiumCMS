# Use the .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
# Set the working directory in the container
WORKDIR /app
# Copy the .NET project file and restore dependencies
COPY *.csproj   .
RUN dotnet restore
# Copy the remaining source code and build the application
COPY . .
RUN dotnet publish -c Release -o out
# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
# Expose the port the app runs on
EXPOSE 80
# Start the application
ENTRYPOINT ["dotnet", "Spatium-CMS.dll"]
