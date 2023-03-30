using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DaprWorkflowAnalysis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Dapr uses a random port for gRPC by default. If we don't know what that port
// is (because this app was started separate from dapr), then assume 4001.
                    if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DAPR_GRPC_PORT")))
                    {
                        Environment.SetEnvironmentVariable("DAPR_GRPC_PORT", "4001");
                    }
                    webBuilder.UseStartup<Startup>();
                });
    }
}
