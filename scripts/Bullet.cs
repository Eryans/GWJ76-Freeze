using Godot;
using System;

public partial class Bullet : Area3D
{
	[Export]
	public float speed = 25f;
	public Vector3 Direction = new();
	private Timer lifespanTimer = new();
	private float lifespan = 10f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		AddChild(lifespanTimer);
		lifespanTimer.Start(lifespan);
		lifespanTimer.Timeout += () => QueueFree();
	}
	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition += Direction * speed * (float)delta;
	}

	private void OnBodyEntered(Node3D body)
	{
		CallDeferred("queue_free");
	}
}
