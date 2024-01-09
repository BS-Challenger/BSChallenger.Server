using BSChallenger.Server.Configuration;
using BSChallenger.Server.Discord;
using BSChallenger.Server.Extensions;
using BSChallenger.Server.Filters;
using BSChallenger.Server.Models;
using BSChallenger.Server.Providers;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.RateLimiting;

namespace BSChallenger.Server
{
	public class LogApp
	{

	}

	public static class Program
	{
		private static readonly ILogger _logger = Log.ForContext<LogApp>();
		public static readonly Version Version = new Version(1, 0, 0);
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					_logger.Information("Testing");
					var secretProvider = new SecretProvider();
					webBuilder
						.ConfigureServices((_, services) =>
						{
							services.AddSingleton(secretProvider);
							services
								.AddAuthorization()
								.AddAuthentication(options =>
								{
									options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
									options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
									options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
								}).AddJwtBearer(options =>
									{
										options.TokenValidationParameters = new TokenValidationParameters
										{
											ValidateIssuer = false,
											ValidateAudience = false,
											ValidateLifetime = true,
											ValidateIssuerSigningKey = true,
											IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretProvider.Secrets.Jwt.Key)),
										};
									});
							services.AddDbContext<Database>(contextLifetime: ServiceLifetime.Singleton, optionsLifetime: ServiceLifetime.Singleton)
								.AddCors(options =>
								{
									options.AddPolicy(name: "website",
													  policy =>
													  {
														  policy.WithOrigins("http://localhost:8080",
																			  "http://bschallenger.xyz");
														  policy.AllowAnyHeader();
														  policy.AllowAnyMethod();
													  });
								})
								.AddRateLimiter(_ => _
									.AddSlidingWindowLimiter("slidingPolicy", options =>
									{
										options.PermitLimit = 50;
										options.Window = TimeSpan.FromSeconds(10);
										options.SegmentsPerWindow = 4;
										options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
										options.QueueLimit = 5;
									}))
								.AddSingleton<AuthBuilder>()
								.AddSingleton<JwtProvider>()
								.AddSingleton<UserProvider>()
								.AddSingleton<DiscordSocketClient>()
								.AddSingleton<InteractionService>()
								.AddSingleton<BeatLeaderApiProvider>()
								.AddSingleton<BPListParserProvider>()
								.AddHostedService<DiscordBot>()
								.AddOptions()
								.AddConfiguration<AppConfiguration>("App")
								.AddSwaggerGen(c =>
									c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challenger API", Version = "v1" })
								)
								.AddControllers(options =>
										options.Filters.Add(new HttpResponseExceptionFilter())
								);
						})
						.Configure(applicationBuilder =>
							applicationBuilder
								.UseSwagger(c =>
								{
									c.RouteTemplate = "swagger/{documentName}/swagger.json";
									c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
										swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{secretProvider.Secrets.URLBase}" } });
								})
								.UseSwaggerUI(c =>
								{
									c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenger API V1");
								})
								.UseRouting()
								.UseAuthentication()
								.UseAuthorization()
								.UseCors("website")
								.UseEndpoints(endPointRouteBuilder => endPointRouteBuilder.MapControllers())
								.UseRateLimiter()
								.UseForwardedHeaders()
						);
				}
				)
				.UseSerilog();
	}
}
