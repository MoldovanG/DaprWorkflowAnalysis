using DurableTask.Core;

namespace DTFDemo.DtfWorkflow.Utils
{
    public interface IWorkflowClient
    {
        public TaskHubClient Client { get; }
    }
}
