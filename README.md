# .NET Project with Serilog and Azure Application Insights Integration

This repository contains a .NET project set up with Serilog for logging, integrated with Azure Application Insights for centralized logging and monitoring.

## Getting Started

To get started with this project, follow these steps:

1. Clone the repository to your local machine:

    ```bash
    git clone https://github.com/yourusername/dotnet-serilog-with-azure-app-insight.git
    ```

2. Navigate to the project directory:

    ```bash
    cd dotnet-serilog-with-azure-app-insight
    ```

3. Update `appsettings.json`:

    Update the `appsettings.json` file with your Azure Application Insights Instrumentation Key and Connection String. You can find placeholders `YOUR_INSTRUMENTATION_KEY` and `YOUR_CONNECTION_STRING` in the file.

    ```json
      "IsUseDefaultLogger": false,
      "APPINSIGHTS_INSTRUMENTATIONKEY": "xxxxxxxx-b05a-41ed-a8ed-xxxxxxxxxx",
      "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=xxxxxx-b05a-41ed-a8ed-xxxxxxxx;IngestionEndpoint=https://xxxxxxxxxx-0.in.applicationinsights.azure.com/;LiveEndpoint=https://xxxxxxxxxx.livediagnostics.monitor.azure.com/"
    }
    ```

4. Build and run the application:

    ```bash
    dotnet build
    dotnet run
    ```

5. Visit `https://localhost:7085/swagger/index.html` in your web browser access the API.
 
## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, feel free to open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).