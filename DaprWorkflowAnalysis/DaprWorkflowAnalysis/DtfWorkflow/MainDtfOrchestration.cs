using DaprWorkflowAnalysis.DtfWorkflow.SubOrchestrators;
using DTFDemo.DtfWorkflow.Activities;
using DTFDemo.WorkflowApp.Dto;
using DurableTask.Core;

namespace DaprWorkflowAnalysis.DtfWorkflow
{
    public class MainDtfOrchestration : TaskOrchestration<string, WorkflowRequest, string, string>
    {
        public override async Task<string> RunTask(OrchestrationContext context, WorkflowRequest input)
        {
            DateTime initialStartTime = context.CurrentUtcDateTime;
            var activityIteration = 0;
            var scheduledSubOrchestrations = new List<Task>();
            while (activityIteration < input.NumberOfParallelSubOrchestration)
            {
                scheduledSubOrchestrations.Add( context.CreateSubOrchestrationInstance<string>(typeof(DtfSubOrchestration),input));
                activityIteration++;
            }
            await Task.WhenAll(scheduledSubOrchestrations);

            TimeSpan ts = DateTime.UtcNow - initialStartTime;
            await context.ScheduleTask<string>(typeof(DtfLoggingActivity),$"The DTF Workflow finished in {ts.TotalMilliseconds} ");
            return "Completed";
        }
    }
}
