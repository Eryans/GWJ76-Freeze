using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float MaxAcceleration { get; private set; } = 25f;
	[Export]
	public float AccelerationForce { get; private set; } = .25f;
	[Export]
	public float MaxRotationForce { get; private set; } = .15f;
	[Export]
	public float AccelerationFriction { get; private set; } = .75f;
	[Export]
	public float RotationFriction { get; private set; } = .75f;
	[Export]
	public float RotationForce { get; private set; } = .1f;
	[Export]
	public float BrakeForce { get; private set; } = .2f;
	public float Acceleration { get; private set; } = 0.0f;
	public float CurrentRotationForce { get; private set; } = 0.0f;
	private bool canAccelerate = true;
	private Timer canAccelerateTimer = new();
	public override void _Ready()
	{
		AddChild(canAccelerateTimer);
		canAccelerateTimer.Timeout += () => canAccelerate = true;
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		Acceleration = Mathf.Clamp(Acceleration, -MaxAcceleration / 2, MaxAcceleration);
		CurrentRotationForce = Mathf.Clamp(CurrentRotationForce, -MaxRotationForce, MaxRotationForce);

		if (Input.IsActionPressed("accelerate") && canAccelerate)
		{
			Acceleration += AccelerationForce;
		}
		else
		{
			Acceleration = Mathf.Lerp(Acceleration, 0, AccelerationFriction * (float)delta);
		}
		if (Input.IsActionPressed("brake_or_reverse"))
		{
			Acceleration -= AccelerationForce / 2;
		}
		if (Input.IsActionPressed("shift"))
		{
			Acceleration = Mathf.Lerp(Acceleration, 0, BrakeForce * (float)delta); ;
			CurrentRotationForce = Mathf.Lerp(CurrentRotationForce, 0, BrakeForce * (float)delta); ;
		}

		velocity = Transform.Basis * Vector3.Forward * Acceleration;

		if (!IsOnFloor())
		{
			velocity += GetGravity();
		}

		float inputDir = Input.GetAxis("turn_right", "turn_left");
		if (Mathf.Floor(Acceleration) != 0)
		{
			CurrentRotationForce += inputDir * RotationForce * (float)delta;
		}
		if (inputDir == 0)
		{
			CurrentRotationForce = Mathf.Lerp(CurrentRotationForce, 0, (float)delta * RotationFriction);
		}
		Vector3 rotation = Rotation;
		rotation.Y += CurrentRotationForce;
		Rotation = rotation;

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (collision.GetCollider() is Node3D node)
			{
				if (node.IsInGroup("obstacles"))
				{
					Acceleration = -Acceleration / 2;
					velocity = velocity.Bounce(collision.GetNormal());
				}
			}

		}
		Velocity = velocity;
		MoveAndSlide();
	}

}
