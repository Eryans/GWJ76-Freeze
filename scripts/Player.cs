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

	public static event Action Brake;
	public static event Action<Player> PlayerSpawn;
	public static event Action GameOver;

	public float Acceleration { get; private set; } = 0.0f;
	public float CurrentRotationForce { get; private set; } = 0.0f;

	private bool canBounce = true;
	private bool isAlive = true;
	private Timer canAccelerateTimer = new();
	private Health health;

	public override void _Ready()
	{
		AddChild(canAccelerateTimer);
		health = GetNode<Health>("Health");
		canAccelerateTimer.Timeout += () => canBounce = true;
		health.HealthDepleted += OnHealthDepleted;
		PlayerSpawn?.Invoke(this);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (isAlive)
		{
			Vector3 velocity = Velocity;
			Acceleration = Mathf.Clamp(Acceleration, -MaxAcceleration / 2, MaxAcceleration);
			CurrentRotationForce = Mathf.Clamp(CurrentRotationForce, -MaxRotationForce, MaxRotationForce);

			if (Input.IsActionPressed("accelerate") && canBounce)
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
				Brake?.Invoke();
				Acceleration = Mathf.Lerp(Acceleration, 0, BrakeForce * (float)delta); ;
				CurrentRotationForce = Mathf.Lerp(CurrentRotationForce, 0, BrakeForce * (float)delta); ;
			}

			if (canBounce)
				velocity = velocity.Lerp(Transform.Basis * Vector3.Forward * Acceleration, (float)delta);

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
				if (collision.GetCollider() is RigidBody3D rb)
				{
					Vector3 pushDirection = (rb.GlobalPosition - GlobalPosition + Vector3.Up).Normalized();
					rb.ApplyTorqueImpulse(pushDirection);
					rb.ApplyCentralImpulse(pushDirection * Acceleration / 2);
					if (rb is Pinguin pinguin)
					{
						pinguin.hasBeenHit = true;
					}
				}
				if (collision.GetCollider() is Node3D node)
				{
					if (!node.IsInGroup("floor") && canBounce)
					{
						health.ChangeCurrentHealth(Math.Abs(Acceleration)); // Use absolute value here, else player could heal by hitting wall while going in reverse
						Acceleration /= 2;
						canBounce = false;
						canAccelerateTimer.Start(1f);
						Vector3 bounceDirection = velocity.Bounce(collision.GetNormal());
						velocity = bounceDirection.Normalized() * Acceleration;
						GD.Print("OUCH ! :(");
					}
				}

			}
			Velocity = velocity;
			MoveAndSlide();
		}
	}
	private void OnHealthDepleted()
	{
		isAlive = false;
		GameOver?.Invoke();
	}
}
