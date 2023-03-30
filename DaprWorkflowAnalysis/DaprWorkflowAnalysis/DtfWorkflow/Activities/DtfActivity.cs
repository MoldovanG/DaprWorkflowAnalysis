using DaprWorkflowAnalysis.Exceptions;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DaprWorkflowAnalysis.DtfWorkflow.Activities
{
    public class DtfActivity : TaskActivity<string, string>
    {
        private readonly ILogger<DtfActivity> _logger;

        public DtfActivity(ILogger<DtfActivity> logger)
        {
            _logger = logger;
        }

        protected override string Execute(TaskContext context, string input)
        {
            throw new NotImplementedException();
        }

        protected override Task<string> ExecuteAsync(TaskContext context, string input)
        {
            _logger.LogInformation($"Executed Activity started by orchestration with Id : {context.OrchestrationInstance.InstanceId}");
            return Task.FromResult("Completed");
        }
    }
}
