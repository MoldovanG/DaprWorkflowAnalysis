using DaprWorkflowAnalysis.DtfWorkflow.Activities;
using DTFDemo.DtfWorkflow.Activities;
using DTFDemo.WorkflowApp.Dto;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DaprWorkflowAnalysis.DtfWorkflow.SubOrchestrators
{
    public class DtfSubOrchestration : TaskOrchestration<string, WorkflowRequest, string, string>
    {
        private readonly ILogger<DtfSubOrchestration> _logger;

        public DtfSubOrchestration(ILogger<DtfSubOrchestration> logger)
        {
            _logger = logger;
        }
        
        public override async Task<string> RunTask(OrchestrationContext context, WorkflowRequest input)
        {
            var activityIteration = 0;
            var scheduledActivities = new List<Task>();
            try
            {
                while (activityIteration < input.NumberOfParallelActivities)
                {
                    scheduledActivities.Add(context.ScheduleTask<string>(typeof(DtfActivity), "execute"));
                    activityIteration++;
                }

                await Task.WhenAll(scheduledActivities.ToArray());
                await context.ScheduleTask<string>(typeof(DtfLoggingActivity),
                    $"Finished executing sub-orchestration with InstanceId : {context.OrchestrationInstance.InstanceId}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Caught Exception in parent orchestration of type: " + e.GetType() );
                throw;
            }

            return "Completed";

        }
    }
}
