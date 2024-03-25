// <copyright file="ILoggingLevelSwitchService.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

namespace API.Services.Interface;

using Serilog.Core;
using Serilog.Events;

public interface ILoggingLevelSwitchService
{
	LoggingLevelSwitch LevelSwitch { get; }

	void SetLogLevel(LogEventLevel level);
}
