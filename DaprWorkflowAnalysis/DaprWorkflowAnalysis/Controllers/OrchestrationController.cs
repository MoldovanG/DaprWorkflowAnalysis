using System;
using System.Threading.Tasks;
using DurableTask.Core;
using Microsoft.AspNetCore.Mvc;
using Dapr.Client;
using Dapr.Workflow;
using DaprWorkflowAnalysis.DtfWorkflow;
using DTFDemo.DaprWorkflow;
using DTFDemo.DtfWorkflow;
using DTFDemo.DtfWorkflow.Utils;
using DTFDemo.WorkflowApp.Dto;

namespace DTFDemo.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrchestrationController : ControllerBase
    {
        private readonly IWorkflowClient _workflowClient;
        private readonly WorkflowEngineClient _daprClient;

        public OrchestrationController(IWorkflowClient workflowClient, WorkflowEngineClient daprClient)
        {
            _workflowClient = workflowClient;
            _daprClient = daprClient;
        }
        
        [HttpPost]
        [Route("dtfworkflow")]
        public OrchestrationInstance StartDtfWorkflow( [FromBody] WorkflowRequest data)
        {
            return _workflowClient.Client.CreateOrchestrationInstanceAsync(typeof(MainDtfOrchestration), data).Result;
        }

        [HttpPost]
        [Route("daprworkflow")]
        public async Task<string> StartDaprWorkflow( [FromBody] WorkflowRequest data)
        {
            return await _daprClient.ScheduleNewWorkflowAsync(
                name: nameof(MainDaprOrchestration),
                instanceId: Guid.NewGuid().ToString(),
                input: data);
        }
    }
}
