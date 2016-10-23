using Microsoft.AspNetCore.Builder;
using Nancy;
using Nancy.Owin;
using Nancy.Configuration;
using Nancy.TinyIoc;
using Microsoft.Extensions.Options;
using NoteCloud.Middleware;
using NoteCloud.DataAccess;
using NoteCloud.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace NoteCloud
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizeMiddleware>();

            var nancyOptions = new NancyOptions();
            nancyOptions.Bootstrapper = new Bootstrapper(app);
            app.UseOwin(x => x.UseNancy(nancyOptions));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddDbContext<NoteCloudContext>();
            services.AddOptions();
            // Add our Config object so it can be injected
            services.Configure<Secrets>(Configuration);
        }

        private class Bootstrapper : DefaultNancyBootstrapper {
            private IApplicationBuilder _app;
            public Bootstrapper(IApplicationBuilder app) {
                _app = app;
            }
            public override void Configure(INancyEnvironment environment)
            {
                environment.Tracing(enabled: false, displayErrorTraces: true);
            }

            protected override void ConfigureApplicationContainer(TinyIoCContainer container)
            {
                base.ConfigureApplicationContainer(container);

                container.Register<IOptions<Secrets>>(_app.ApplicationServices.GetService<IOptions<Secrets>>());
            }
        }
    }
}