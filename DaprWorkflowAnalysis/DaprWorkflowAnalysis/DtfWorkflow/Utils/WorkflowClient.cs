using System;
using DTFDemo.DtfWorkflow.Activities;
using DTFDemo.DtfWorkflow.SubOrchestrators;
using DTFDemo.WorkflowApp.Utils;
using DurableTask.Core;
using DurableTask.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace DTFDemo.DtfWorkflow.Utils
{
    public class WorkflowClient : IWorkflowClient
    {
        public TaskHubClient Client { get; }

        public WorkflowClient (IConfiguration config, IServiceProvider serviceProvider)
        {
            var loggerFactory = LoggerFactory.Create(
                builder =>
                {
                    builder.AddNLogWeb(LogManager.Setup().LoadConfigurationFromAppSettings().LogFactory.Configuration);
                });
            string storageConnectionString = config.GetValue<string>("SqlConnectionString");
            var sqlOrchestrationSettings = new SqlOrchestrationServiceSettings(storageConnectionString, "TaskHub1");
            var orchestrationServiceAndClient = new SqlOrchestrationService(sqlOrchestrationSettings);

            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient, loggerFactory: loggerFactory);
            orchestrationServiceAndClient.CreateIfNotExistsAsync().Wait();
            var taskHub = new TaskHubWorker(orchestrationServiceAndClient, loggerFactory);
            taskHub.AddTaskOrchestrations(
                           new ServiceProviderObjectCreator<TaskOrchestration>(typeof(MainDtfOrchestration), serviceProvider),
                           new ServiceProviderObjectCreator<TaskOrchestration>(typeof(DtfSubOrchestration), serviceProvider));
            taskHub.AddTaskActivities(
                new ServiceProviderObjectCreator<TaskActivity>(typeof(DtfActivity), serviceProvider),
                new ServiceProviderObjectCreator<TaskActivity>(typeof(DtfLoggingActivity), serviceProvider));
            taskHub.StartAsync().Wait();
            Client = taskHubClient;
        }
    }
}
