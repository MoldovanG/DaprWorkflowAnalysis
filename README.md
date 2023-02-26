# Dapr Workflow vs DTF workflow with ASP.NET analysis

This Dapr workflow example shows how to create a Dapr workflow (`Workflow`) and invoke it using the console.

## Prerequisites

- [.NET 6+](https://dotnet.microsoft.com/download) installed
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
- An MSSQL instance started locally or in the cloud. 
- [Initialized Dapr environment](https://docs.dapr.io/getting-started/install-dapr-selfhost/) with an MSSQL state store configured
- [Dapr .NET SDK](https://github.com/dapr/dotnet-sdk/)
- Replace the YOUR_CONNECTION_STRING_HERE string in the appsettings files. 
## Scope of the project

This demo project contains the capability to execute 2 types of workflows (DTF based, or Dapr based) based on a given configuration:
- Number of parallel activities per suborchestration
- Number of parallel suborchestrations


Every workflow will print out the elapsed execution time when the workflow finishes. 

## Running the console app example

To run the workflow web app locally, two separate terminal windows are required.
In the first terminal window, from the `WorkflowConsoleApp` directory, run the following command to start the program itself:

```sh
dotnet run
```

Next, in a separate terminal window, start the dapr sidecar:

```sh
dapr run --app-id wfapp --dapr-grpc-port 4001 --dapr-http-port 3500
```

Dapr listens for HTTP requests at `http://localhost:3500`.

To start a  dapr workflow you have to : 

```curl
curl --location --request POST  -k 'https://localhost:5004/api/daprworkflow' --header 'Content-Type: application/json' --data-raw '{ \"numberOfParallelSubOrchestration\" : 10, \"numberOfParallelActivities\"  : 10}'
```

To start a DTF workflow you have to :

```curl
curl --location --request POST  -k 'https://localhost:5004/api/dtfworkflow' --header 'Content-Type: application/json' --data-raw '{ \"numberOfParallelSubOrchestration\" : 10, \"numberOfParallelActivities\"  : 10}'
```