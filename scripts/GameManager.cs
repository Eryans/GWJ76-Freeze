using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class GameManager : Node
{
	public static event Action ReachedLastPoint;
	public static event Action<DestinationPoint> DestinationPointChanged;

	private readonly List<DestinationPoint> destinationPoints = new();
	private RandomNumberGenerator rng = new();
	private Player player;
	private bool canRestartGame = false;

	public override void _Ready()
	{
		InitGame();
		GetTree().Root.MeshLodThreshold = 2.0f;
	}
	private void InitGame()
	{
		GD.Randomize();
		Player.GameOver += OnGameOver;
		var tmpDestinationPoints = GetTree().GetNodesInGroup("destination_points");
		foreach (DestinationPoint point in tmpDestinationPoints.Cast<DestinationPoint>())
		{
			point.PointReachedByPlayer += OnPlayerReachedPoint;
			destinationPoints.Add(point);
		}
		destinationPoints.Sort((a, b) => a.PointIndex.CompareTo(b.PointIndex));
		GD.Print(destinationPoints);
		CallDeferred("SetNewDestination", 0);
	}
	public override void _Process(double delta)
	{
		if (canRestartGame && Input.IsActionJustPressed("restart"))
		{
			canRestartGame = false;
			GetTree().ReloadCurrentScene();
		}
	}
	public void OnPlayerReachedPoint(DestinationPoint reachedPoint)
	{
		if (reachedPoint.PointIndex > 0)
		{
			reachedPoint.isTargetToReach = false;
		}
		SetNewDestination(reachedPoint.PointIndex + 1);
	}
	private void SetNewDestination(int pointIndex)
	{
		if (pointIndex == destinationPoints.Count)
		{
			GD.Print("Reached last point");
			ReachedLastPoint?.Invoke();
			return;
		}
		DestinationPoint newPoint = destinationPoints[pointIndex];
		newPoint.isTargetToReach = true;
		GD.Print("New destination at point : " + newPoint.GlobalPosition);
		DestinationPointChanged?.Invoke(newPoint);
	}
	private void OnGameOver()
	{
		canRestartGame = true;
	}
}
