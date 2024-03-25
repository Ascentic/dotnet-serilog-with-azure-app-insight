// <copyright file="Program.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

#pragma warning disable SA1200 // Using directives should be placed correctly

using API.Constants;
using API.Extensions;
using API.Services;
using API.Services.Interface;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

bool isUseDefaultLogger = configuration.GetValue<bool>(AppSettingsConstants.IsUseDefaultLogger);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILoggingLevelSwitchService, LoggingLevelSwitchService>();

if (!isUseDefaultLogger)
{
	bool isAppInsightSettingsConfigured =
		!string.IsNullOrEmpty(configuration[AppSettingsConstants.AppInsightConnectionStringEnvironmentVariable]) &&
		!string.IsNullOrEmpty(configuration[AppSettingsConstants.AppInsightInstrumentationKeyForWebSites]);

	if (isAppInsightSettingsConfigured)
	{
		builder.Services.AddApplicationInsightsTelemetry();
	}

	builder.Host.AddSerilogLogging(configuration, isAppInsightSettingsConfigured);
}

var app = builder.Build();

if (!isUseDefaultLogger)
{
	app.UseSerilogRequestLogging(options =>
	{
		// Customize the message template
		options.MessageTemplate = "Handled RequestPath {RequestPath} TraceIdentifier {TraceIdentifier}";

		// Emit debug-level events instead of the defaults
		options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;

		// Attach additional properties to the request completion event
		options.EnrichDiagnosticContext = (IDiagnosticContext diagnosticContext, HttpContext httpContext) =>
		{
			diagnosticContext.Set("RequestPath", httpContext.Request.Path);
			diagnosticContext.Set("TraceIdentifier", httpContext.TraceIdentifier);
		};
	});
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
