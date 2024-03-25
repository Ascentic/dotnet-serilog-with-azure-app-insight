// <copyright file="LoggingLevelSwitchService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

namespace API.Services;

using API.Services.Interface;
using Serilog.Core;
using Serilog.Events;

public class LoggingLevelSwitchService : ILoggingLevelSwitchService
{
	private readonly LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();

	public LoggingLevelSwitch LevelSwitch => this.levelSwitch;

	public void SetLogLevel(LogEventLevel level)
	{
		this.levelSwitch.MinimumLevel = level;
	}
}
