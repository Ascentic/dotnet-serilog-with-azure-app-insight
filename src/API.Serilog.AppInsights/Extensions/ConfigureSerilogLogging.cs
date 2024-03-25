// <copyright file="ConfigureSerilogLogging.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

namespace API.Extensions;

using Microsoft.ApplicationInsights.Extensibility;

using API.Common;
using API.Constants;
using API.Services.Interface;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Slack;
using Serilog.Sinks.Slack.Models;

public static class ConfigureSerilogLogging
{
	public static IHostBuilder AddSerilogLogging(this IHostBuilder builder, IConfiguration configuration, bool isAppInsightSettingsConfigured)
	{
		builder.UseSerilog((hostingContext, services, loggerConfiguration) =>
		{
			var levelSwitch = services.GetRequiredService<ILoggingLevelSwitchService>().LevelSwitch;

			loggerConfiguration
				.MinimumLevel.ControlledBy(levelSwitch)
				.Enrich.With(new ThreadIdEnricher())
				.Enrich.WithProperty("Version", "1.0.0")
				.WriteTo.Console(
					outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
				.WriteTo.Slack(new SlackSinkOptions
				{
					WebHookUrl = "https://hooks.slack.com/services/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",
					MinimumLogEventLevel = LogEventLevel.Warning,
				}).WriteTo.Debug();

			var logLevelConfigurations = configuration.GetSection(AppSettingsConstants.LogLevel).GetChildren();

			foreach (IConfigurationSection logLevelConfiguration in logLevelConfigurations)
			{
				var configKey = logLevelConfiguration.Key;

				// Azure App Service replaces . with _ in the configuration key so we need to put the . back
				if (configKey.Contains("_"))
				{
					configKey = configKey.Replace("_", ".");
				}

				if (Enum.TryParse(logLevelConfiguration.Value, ignoreCase: true, out LogEventLevel result))
				{
					if (configKey == AppSettingsConstants.Default)
					{
						levelSwitch.MinimumLevel = result;
						continue;
					}

					loggerConfiguration.MinimumLevel.Override(configKey, result);
				}
			}

			if (isAppInsightSettingsConfigured)
			{
				loggerConfiguration.WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces);
			}
		});

		return builder;
	}
}
