using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Workflow;
using DTFDemo.DaprWorkflow.Activities;
using DTFDemo.DaprWorkflow.SubWorkflows;
using DTFDemo.WorkflowApp.Dto;
using DateTime = System.DateTime;

namespace DTFDemo.DaprWorkflow
{
    public class MainDaprOrchestration :  Workflow<WorkflowRequest, string>
    {
        public override async Task<string> RunAsync(WorkflowContext context,WorkflowRequest input)
        {
            DateTime initialStartTime = context.CurrentUtcDateTime;
            var activityIteration = 0;
            var scheduledSubOrchestrations = new List<Task>();
            while (activityIteration < input.NumberOfParallelActivities)
            {
                scheduledSubOrchestrations.Add( context.CallChildWorkflowAsync<string>(nameof(DaprSubOrchestration),input));
                activityIteration++;
            }
            await Task.WhenAll(scheduledSubOrchestrations);
            TimeSpan ts = DateTime.UtcNow - initialStartTime;
            await context.CallActivityAsync(nameof(DaprLoggingActivity),$"The Dapr Workflow finished in {ts.TotalMilliseconds} ");
            return "Completed";
        }
    }
}
