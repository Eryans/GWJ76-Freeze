using Godot;
using System;

public partial class Bullet : Area3D
{
	[Export]
	public float speed = 10f;
	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition += Vector3.Forward * speed * (float)delta;
	}
}
