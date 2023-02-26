using Dapr.Workflow;
using DTFDemo.DaprWorkflow;
using DTFDemo.DaprWorkflow.Activities;
using DTFDemo.DaprWorkflow.SubWorkflows;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DTFDemo.DtfWorkflow;
using DTFDemo.DtfWorkflow.Activities;
using DTFDemo.DtfWorkflow.SubOrchestrators;
using DTFDemo.DtfWorkflow.Utils;

namespace DTFDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IWorkflowClient,  WorkflowClient>();
            services.AddTransient(typeof(MainDtfOrchestration));
            services.AddTransient(typeof(DtfSubOrchestration));
            services.AddTransient(typeof(DtfActivity));
            services.AddTransient(typeof(DtfLoggingActivity));
            
            services.AddDaprWorkflow(options =>
            {
                // Note that it's also possible to register a lambda function as the workflow
                // or activity implementation instead of a class.
                options.RegisterWorkflow<MainDaprOrchestration>();
                options.RegisterWorkflow<DaprSubOrchestration>();

                // These are the activities that get invoked by the workflow(s).
                options.RegisterActivity<DaprActivity>();
                options.RegisterActivity<DaprLoggingActivity>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
