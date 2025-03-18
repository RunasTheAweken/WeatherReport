# WeatherReport
 
# Weather Report API

This project is an ASP.NET Core Web API that fetches weather data from the Visual Crossing Weather API and caches responses using Redis.

## Features
- Fetches weather data based on a city name and optional date range.
- Uses Redis caching to store API responses for improved performance.
- Implements dependency injection for better maintainability.

## Prerequisites
- .NET SDK (version 7.0 or higher)
- Redis server running locally or remotely
- Visual Crossing Weather API key

## Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/your-username/WeatherReport.git
   cd WeatherReport
   ```

2. Set up environment variables for the API key:
   ```sh
   export API_KEY="your_visual_crossing_api_key"
   ```
   Or, for Windows PowerShell:
   ```powershell
   [System.Environment]::SetEnvironmentVariable("API_KEY", "your_visual_crossing_api_key", "User")
   ```

3. Update `appsettings.json` (optional):
   ```json
   {
     "Redis": {
       "ConnectionString": "localhost"
     }
   }
   ```

4. Restore dependencies:
   ```sh
   dotnet restore
   ```

5. Run the application:
   ```sh
   dotnet run
   ```

## Usage
### Fetch Weather Data for a City
```
GET /api/WeatherReport/{city}
```
Example:
```
GET /api/WeatherReport/London
```

### Fetch Weather Data with Date Range
```
GET /api/WeatherReport/{city}/{dateStart}/{dateStop}
```
Example:
```
GET /api/WeatherReport/London/2024-03-01/2024-03-07
```

## Deployment
To deploy the project, you can use Docker, Azure, or any cloud hosting service that supports .NET applications.



https://roadmap.sh/projects/weather-api-wrapper-service
