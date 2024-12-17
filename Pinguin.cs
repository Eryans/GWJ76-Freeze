using Godot;
using System;

public partial class Pinguin : RigidBody3D
{
	public bool hasBeenHit = false;
	private Player player;
	private PackedScene bulletScene;

	public override void _Ready()
	{
		player = (Player)GetTree().GetFirstNodeInGroup("player");
		bulletScene = GD.Load<PackedScene>("");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsInstanceValid(player) && !hasBeenHit)
		{
			Vector3 directionToPlayer = (player.GlobalPosition - GlobalPosition).Normalized();
			Rotation = Rotation with { Y = Mathf.Atan2(directionToPlayer.X, directionToPlayer.Z) };
		}
	}
}
