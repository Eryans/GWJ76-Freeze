using Godot;
using System;

[GlobalClass]
public partial class DestinationPoint : Area3D
{
	[Export]
	public int PointIndex { get; private set; }
	public event Action<DestinationPoint> PointReachedByPlayer;
	public bool isTargetToReach = false;
	public override void _Ready()
	{
		BodyEntered += OnPointReachedByPlayer;
		if (!Engine.IsEditorHint()) Visible = false;
	}

	private void OnPointReachedByPlayer(Node3D body)
	{
		if (body is Player && isTargetToReach)
			PointReachedByPlayer?.Invoke(this);
	}
}
