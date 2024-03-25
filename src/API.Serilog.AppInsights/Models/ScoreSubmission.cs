// <copyright file="ScoreSubmission.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

namespace API.Models;

/// <summary>
/// Represents a score submission by a player.
/// </summary>
public class ScoreSubmission
{
	/// <summary>
	/// Gets or sets the name of the player submitting the score.
	/// </summary>
	required public string PlayerName { get; set; }

	/// <summary>
	/// Gets or sets the score submitted by the player.
	/// </summary>
	public int Score { get; set; }
}
