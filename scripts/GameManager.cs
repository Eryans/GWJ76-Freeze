using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	private readonly List<DestinationPoint> destinationPoints = new();
	private RandomNumberGenerator rng = new();

	public static event Action<DestinationPoint> DestinationPointChanged;
	public static event Action ReachedLastPoint;

	public override void _Ready()
	{
		GD.Randomize();
		var tmpDestinationPoints = GetTree().GetNodesInGroup("destination_points");
		foreach (DestinationPoint point in tmpDestinationPoints)
		{
			point.PointReachedByPlayer += OnPlayerReachedPoint;
			destinationPoints.Add(point);
		}
		destinationPoints.Sort((a, b) => a.PointIndex.CompareTo(b.PointIndex));
		GD.Print(destinationPoints);
		CallDeferred("SetNewDestination", 0);
	}
	public void OnPlayerReachedPoint(DestinationPoint newDestination)
	{
		if (newDestination.PointIndex > 0)
		{
			DestinationPoint oldPoint = (DestinationPoint)destinationPoints[newDestination.PointIndex - 1];
			oldPoint.isTargetToReach = false;
		}
		SetNewDestination(newDestination.PointIndex + 1);
	}
	private void SetNewDestination(int pointIndex)
	{
		if (pointIndex == destinationPoints.Count)
		{
			GD.Print("Reached last point");
			ReachedLastPoint?.Invoke();
			return;
		}
		DestinationPoint newPoint = (DestinationPoint)destinationPoints[pointIndex];
		newPoint.isTargetToReach = true;
		GD.Print("New destination at point : " + newPoint.GlobalPosition);
		DestinationPointChanged?.Invoke(newPoint);
	}
}
