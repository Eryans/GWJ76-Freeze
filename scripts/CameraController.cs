using Godot;
using System;

public partial class CameraController : Node3D
{
	[Export]
	public CharacterBody3D Target { get; private set; }
	[Export]
	public float LerpWeight { get; private set; } = 5f;
	[Export]
	public float RotationLerpWeight { get; private set; } = 5f;
	private Camera3D camera;

	public override void _Ready()
	{
		camera = GetNode<Camera3D>("%Camera3D");
		Player.brake += () =>
		{
			Rotation = Rotation with { Y = Mathf.LerpAngle(Rotation.Y, Target.Rotation.Y, (float)GetPhysicsProcessDeltaTime()) };
		};
	}
	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = GlobalPosition.Lerp(Target.GlobalPosition, LerpWeight * (float)delta);
		float rotationDirection = -Input.GetAxis("camera_left", "camera_right");
		Rotation = Rotation with { Y = Mathf.LerpAngle(Rotation.Y, Rotation.Y + rotationDirection, RotationLerpWeight * (float)delta) };
		Vector3 directionToTarget = new Vector3(camera.GlobalPosition.X, Target.GlobalPosition.Y, camera.GlobalPosition.Z).DirectionTo(Target.GlobalPosition);
		float dotProduct = directionToTarget.Normalized().Dot(-Target.Basis.Z);
		if (Mathf.Acos(dotProduct) > Mathf.Pi / 6 && Target.Velocity.Length() > 1)
		{
			Rotation = Rotation with { Y = Mathf.LerpAngle(Rotation.Y, Target.Rotation.Y, (float)delta) };
		}
	}
}
