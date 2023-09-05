using BSChallenger.Server.Configuration;
using BSChallenger.Server.Discord;
using BSChallenger.Server.Extensions;
using BSChallenger.Server.Filters;
using BSChallenger.Server.Jobs;
using BSChallenger.Server.Models;
using BSChallenger.Server.Providers;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

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
                        .ConfigureServices((hostBuilderContext, services) =>
                        {
                            services.AddSingleton<SecretProvider>();
                            services
							    .AddDbContext<Database>(contextLifetime: ServiceLifetime.Singleton, optionsLifetime: ServiceLifetime.Singleton)
								.AddRateLimiter(_ => _
                                    .AddSlidingWindowLimiter("slidingPolicy", options =>
                                    {
                                        options.PermitLimit = 50;
                                        options.Window = TimeSpan.FromSeconds(10);
                                        options.SegmentsPerWindow = 4;
                                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                                        options.QueueLimit = 5;
                                    }))
                                .AddSingleton<DiscordSocketClient>()
                                .AddSingleton<InteractionService>()
                                .AddHostedService<DiscordBot>()
                                .AddOptions()
                                .AddConfiguration<AppConfiguration>("App")
                                .AddSingleton<BeatLeaderApiProvider>()
                                .AddSingleton<BPListParserProvider>()
                                .AddSwaggerGen(c =>
                                {
                                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challenger API", Version = "v1" });
                                })
                                .AddQuartz(q =>
                                {
                                    var jobKey = new JobKey("WeeklyScanHistoryJob");
                                    q.AddJob<WeeklyScanHistoryJob>(opts => opts.WithIdentity(jobKey));

                                    q.AddTrigger(opts => opts
                                        .ForJob(jobKey)
                                        .WithIdentity("WeeklyScanHistoryJob-trigger")
                                        .WithCronSchedule("0 0 0 ? * SUN *"));
                                })
                                .AddQuartzHostedService(q => q.WaitForJobsToComplete = true)
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
                                .UseRateLimiter()
                                .UseForwardedHeaders()
                        )
                )
                .UseSerilog();
    }
}
