// <copyright file="LeaderboardController.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>

namespace API.Controllers;

using API.Models;
using API.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;

/// <summary>
/// Controller for managing the leaderboard and player scores.
/// </summary>
[ApiController]
public class LeaderboardController : ControllerBase
{
	private readonly ILogger<LeaderboardController> logger;
	private readonly Dictionary<string, int> scores;
	private readonly ILoggingLevelSwitchService loggingLevelSwitchService;

	/// <summary>
	/// Initializes a new instance of the <see cref="LeaderboardController"/> class.
	/// </summary>
	/// <param name="logger">The logger.</param>
	/// <param name="loggingLevelSwitchService">The logging level switch service.</param>
	public LeaderboardController(
		ILogger<LeaderboardController> logger,
		ILoggingLevelSwitchService loggingLevelSwitchService)
	{
		this.logger = logger;
		this.loggingLevelSwitchService = loggingLevelSwitchService;
		this.scores = new Dictionary<string, int>
		{
			{ "Player1", 1000 },
			{ "Player2", 850 },
			{ "Player3", 750 },
			{ "Player4", 600 },
			{ "Player5", 500 },
		};
	}

	/// <summary>
	/// Retrieves the leaderboard.
	/// </summary>
	/// <returns>The leaderboard.</returns>
	[HttpGet(nameof(GetLeaderboard))]
	public IActionResult GetLeaderboard()
	{
		this.logger.LogInformation("Retrieving leaderboard...");

		var sortedScores = this.scores.OrderByDescending(kv => kv.Value).ToList();

		return this.Ok(sortedScores);
	}

	/// <summary>
	/// Submits a score for a player.
	/// </summary>
	/// <param name="submission">The score submission.</param>
	/// <returns>The result of the submission.</returns>
	[HttpPost(nameof(SubmitScore))]
	public IActionResult SubmitScore([FromBody] ScoreSubmission submission)
	{
		this.logger.LogInformation("Submitting score submission {@submission}", submission);

		// Simulate potential issue: Duplicate score submissions
		if (this.scores.ContainsKey(submission.PlayerName))
		{
			this.logger.LogWarning("Duplicate score submission detected for player {Player}", submission.PlayerName);

			// Instead of updating, just return Ok as it's a duplicate submission
			return this.Ok();
		}

		this.scores.Add(submission.PlayerName, submission.Score);
		this.logger.LogInformation("Added new score for player {Player} succeeded.", submission.PlayerName);

		return this.Ok();
	}

	/// <summary>
	/// Retrieves the score of a player from the leaderboard.
	/// </summary>
	/// <param name="playerName">The name of the player whose score is being retrieved.</param>
	/// <returns>
	/// An HTTP response containing the player's score if found,
	/// or an appropriate error response if the player is not found or if a simulated failure occurs.
	/// </returns>
	[HttpGet("player/{playerName}")]
	public IActionResult GetPlayerScore(string playerName)
	{
		// Simulate failure randomly
		if (this.ShouldSimulateFailure())
		{
			// Code enable debug logs
			// Subsequent requests will have debug logs enabled
			// This helps in diagnosing the issue
			this.loggingLevelSwitchService.SetLogLevel(LogEventLevel.Debug);
			this.logger.LogDebug("Simulated failure occurred. Debug logs enabled.");

			// Return an internal server error status code with a message
			return this.StatusCode(500, "Internal server error occurred.");
		}

		// Check if the player exists in the leaderboard
		if (!this.scores.ContainsKey(playerName))
		{
			// Log a warning if player not found and return a Not Found status code
			this.logger.LogWarning("Player {Player} not found in the leaderboard.", playerName);
			return this.NotFound();
		}

		// Log accessing datastore to retrieve player's score
		this.logger.LogDebug("Accessing datastore to retrieve score for player {Player}...", playerName);

		// Retrieve the player's score from the leaderboard
		var playerScore = this.scores[playerName];

		// Log the successful retrieval of the player's score
		this.logger.LogInformation("Retrieved score for player {Player}: {Score}", playerName, playerScore);

		// Return the player's score along with their name
		return this.Ok(new { PlayerName = playerName, Score = playerScore });
	}

	// Method to simulate failure randomly
	private bool ShouldSimulateFailure()
	{
		// Simulate failure randomly with a 20% chance
		return new Random().Next(1, 101) <= 20;
	}
}
