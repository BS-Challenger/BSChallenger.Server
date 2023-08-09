using BSChallenger.Server.Configuration;
using BSChallenger.Server.Models;
using BSChallenger.Server.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BSChallenger.Server.Filters;
using Microsoft.OpenApi.Models;
using BSChallenger.Server.Discord;
using System.Threading;
using System;

namespace BSChallenger.Server
{
    public static class Program
    {
        public static Version Version = new Version(1, 0, 0);
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder
                        .ConfigureServices((hostBuilderContext, services) => {
                            services.AddScoped<SecretProvider, SecretProvider>();
							services
                                .AddHostedService<DiscordBot>()
                                .AddOptions()
                                .AddConfiguration<AppConfiguration>("App")
                                .AddDbContext<Database>()
                                .AddSingleton<BeatleaderAPI>()
                                .AddSingleton<BPListParser>()
                                .AddSingleton<TokenProvider>()
                                .AddSingleton<PasswordProvider>()
                                .AddSwaggerGen(c =>
                                {
                                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challenger API", Version = "v1" });
                                })
                                .AddControllers(options =>
                                    options.Filters.Add(new HttpResponseExceptionFilter())
                                );
                        })
                        .Configure(applicationBuilder =>
                            applicationBuilder
								.UseSwagger()
		                        .UseSwaggerUI(c =>
                                {
                                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenger API V1");
                                })
                                .UseRouting()
                                .UseEndpoints(endPointRouteBuilder => endPointRouteBuilder.MapControllers())
                        )
                )
                .UseSerilog();
    }
}
