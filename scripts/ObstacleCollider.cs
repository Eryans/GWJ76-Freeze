using Godot;
using System;

[GlobalClass]
public partial class ObstacleCollider : Area3D
{
	public event Action<Node3D> HitObstacle;
	public override void _Ready()
	{
		BodyEntered += OnAreaBodyEntered;
	}

	private void OnAreaBodyEntered(Node3D body)
	{
		if (body.IsInGroup("obstacles"))
		{
			HitObstacle?.Invoke(body);
		}
	}
}
