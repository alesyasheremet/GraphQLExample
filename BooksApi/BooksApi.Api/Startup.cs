using BooksApi.Api.Models;
using BooksApi.Infra.Repositories;
using BooksApi.Models.Books;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BooksApi.Api
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
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddMvc(o => o.EnableEndpointRouting =false).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<BookApiQuery>();
            services.AddSingleton<BookApiSchema>();
            services.AddSingleton<IBookRepository, BookRepository>();
            services.AddSingleton<BookType>();
            services.AddSingleton<SpecificationType>();
            services.AddSingleton<ISchema, BookApiSchema>(services => new BookApiSchema(new SelfActivatingServiceProvider(services))).AddGraphQL((options, provider) =>
            {
                // options.EnableMetrics = Environment.IsDevelopment();
                //var logger = provider.GetRequiredService<ILogger<Startup>>();
                //options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occurred", ctx.OriginalException.Message);
            })
        // Add required services for GraphQL request/response de/serialization
        .AddSystemTextJson()
        .AddNewtonsoftJson()
        .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true);//.AddGraphTypes(typeof(BookApiSchema)); ;
         // For .NET Core 3+
     
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {                
                app.UseHsts();
            }
            
                app.UseWebSockets();
            app.UseHttpsRedirection();
            app.UseGraphQL<BookApiSchema>();
            app.UseGraphQLPlayground("/api/graphql");
            app.UseMvc(routes =>
                routes.MapRoute(
                name: "default",
                template: "api"
            ));
        }
    }
}
