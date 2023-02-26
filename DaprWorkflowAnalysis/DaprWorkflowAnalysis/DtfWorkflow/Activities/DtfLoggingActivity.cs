using System.Threading.Tasks;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFDemo.DtfWorkflow.Activities
{
    public class DtfLoggingActivity : TaskActivity<string, string>
    {
        private readonly ILogger<DtfLoggingActivity> _logger;

        public DtfLoggingActivity(ILogger<DtfLoggingActivity> logger)
        {
            _logger = logger;
        }
        
        protected override string Execute(TaskContext context, string input)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<string> ExecuteAsync(TaskContext context, string input)
        {
            _logger.LogInformation(input);

            return Task.FromResult("Completed");
        }
    }
}