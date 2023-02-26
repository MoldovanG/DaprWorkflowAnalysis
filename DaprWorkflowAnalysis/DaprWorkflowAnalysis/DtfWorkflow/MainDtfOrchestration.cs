using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTFDemo.DtfWorkflow.Activities;
using DTFDemo.DtfWorkflow.SubOrchestrators;
using DTFDemo.WorkflowApp.Dto;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFDemo.DtfWorkflow
{
    public class MainDtfOrchestration : TaskOrchestration<string, WorkflowRequest, string, string>
    {
        private readonly ILogger<MainDtfOrchestration> _logger;

        public MainDtfOrchestration(ILogger<MainDtfOrchestration> logger)
        {
            _logger = logger;
        }

        public override async Task<string> RunTask(OrchestrationContext context, WorkflowRequest input)
        {
            DateTime initialStartTime = context.CurrentUtcDateTime;
            var activityIteration = 0;
            var scheduledSubOrchestrations = new List<Task>();
            while (activityIteration < input.NumberOfParallelActivities)
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
