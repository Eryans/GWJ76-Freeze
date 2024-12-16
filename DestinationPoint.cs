using Godot;
using System;

[GlobalClass]
public partial class DestinationPoint : Area3D
{
	public event Action<DestinationPoint> PointReachedByPlayer;
	public bool isTargetToReach = false;
	public override void _Ready()
	{
		BodyEntered += OnPointReachedByPlayer;
	}

	private void OnPointReachedByPlayer(Node3D body)
	{
		if (isTargetToReach)
			PointReachedByPlayer?.Invoke(this);
	}
}
