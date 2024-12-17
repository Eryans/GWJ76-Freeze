using Godot;
using System;

public partial class Bullet : Area3D
{
	[Export]
	public float speed = 15f;
	public Vector3 Direction = new();
	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition += Direction * speed * (float)delta;
	}
}
