using System.Threading.Tasks;
using Dapr.Workflow;
using Google.Type;
using Microsoft.Extensions.Logging;

namespace DTFDemo.DaprWorkflow.Activities
{

    public class DaprLoggingActivity : WorkflowActivity<string, string>
    {
        private readonly ILogger<DaprActivity> _logger;

        public DaprLoggingActivity(ILogger<DaprActivity> logger)
        {
            _logger = logger;
        }

        public override Task<string> RunAsync(WorkflowActivityContext context, string input)
        {
            _logger.LogInformation(input);

            return Task.FromResult("Completed");
        }
    }
}