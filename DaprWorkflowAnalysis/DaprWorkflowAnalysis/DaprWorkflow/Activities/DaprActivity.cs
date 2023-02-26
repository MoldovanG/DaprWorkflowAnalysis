using System.Threading.Tasks;
using Dapr.Workflow;
using Microsoft.Extensions.Logging;

namespace DTFDemo.DaprWorkflow.Activities
{

    public class DaprActivity : WorkflowActivity<string, string>
    {
        private readonly ILogger<DaprActivity> _logger;

        public DaprActivity(ILogger<DaprActivity> logger)
        {
            _logger = logger;
        }

        public override Task<string> RunAsync(WorkflowActivityContext context, string input)
        {
            _logger.LogInformation($"Executed Activity started by orchestration with Id : {context.InstanceId}");

            return Task.FromResult("Completed");
        }
    }
}
