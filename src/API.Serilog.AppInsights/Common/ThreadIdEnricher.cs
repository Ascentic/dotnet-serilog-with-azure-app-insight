// <copyright file="ThreadIdEnricher.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

namespace API.Common;

using Serilog.Core;
using Serilog.Events;

public class ThreadIdEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
				"ThreadId", Thread.CurrentThread.ManagedThreadId));
	}
}
