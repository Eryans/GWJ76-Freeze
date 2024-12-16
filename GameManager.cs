using Godot;
using System;
using System.Linq;

public partial class GameManager : Node
{
	private Godot.Collections.Array<Node> destinationPoints;
	private RandomNumberGenerator rng = new();

	public static event Action<DestinationPoint> DestinationPointChanged;

	public override void _Ready()
	{
		GD.Randomize();
		destinationPoints = GetTree().GetNodesInGroup("destination_points");
		foreach (DestinationPoint point in destinationPoints)
		{
			point.PointReachedByPlayer += OnPlayerReachedPoint;
		}
		SetNewDestination();
	}
	public void OnPlayerReachedPoint(DestinationPoint point)
	{
		SetNewDestination();
		point.isTargetToReach = false;
	}
	private void SetNewDestination()
	{
		int randomIndex = rng.RandiRange(0, destinationPoints.Count - 1);
		DestinationPoint randomPoint = (DestinationPoint)destinationPoints[randomIndex];
		if (randomPoint.isTargetToReach)
		{
			SetNewDestination();
		}
		else
		{
			randomPoint.isTargetToReach = true;
			GD.Print("New destination at point : " + randomPoint.GlobalPosition);
			DestinationPointChanged?.Invoke(randomPoint);
		}
	}
}
