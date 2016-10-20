using Microsoft.AspNetCore.Builder;
using Nancy;
using Nancy.Owin;
using Nancy.Configuration;
using Nancy.Diagnostics;
using NoteCloud.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace NoteCloud
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var nancyOptions = new NancyOptions();
            nancyOptions.Bootstrapper = new Bootstrapper();
            app.UseOwin(x => x.UseNancy(nancyOptions));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<NoteCloudContext>();
        }

        private class Bootstrapper : DefaultNancyBootstrapper {
            public override void Configure(INancyEnvironment environment)
            {
                environment.Tracing(enabled: false, displayErrorTraces: true);
            }
        }
    }
}