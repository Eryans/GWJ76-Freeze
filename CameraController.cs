using Godot;
using System;

public partial class CameraController : Node3D
{
	[Export]
	public Node3D Target { get; private set; }
	[Export]
	public float LerpWeight { get; private set; } = 5f;

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = GlobalPosition.Lerp(Target.GlobalPosition, LerpWeight * (float)delta);
	}
}
