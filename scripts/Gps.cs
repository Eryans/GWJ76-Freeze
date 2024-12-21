using Godot;
using System;

public partial class Gps : Node3D
{
	[Export]
	private Player player;
	private Vector3 target = new();

	public override void _Ready()
	{
		GameManager.DestinationPointChanged += OnNewDestinationPoint;
		Player.PlayerSpawn += (Player playerEntity) => { if (!IsInstanceValid(player)) { player = playerEntity; } };
	}
	public override void _Process(double delta)
	{
		if (IsInstanceValid(player))
		{
			GlobalPosition = player.GlobalPosition;
		}
		if (target != Vector3.Zero)
		{
			Visible = true;
			Vector3 direction = GlobalPosition - target;
			float toLookAt = Mathf.Atan2(direction.X, direction.Z);
			Rotation = Rotation with { Y = toLookAt };
		}
		else
		{
			Visible = false;
		}
	}

	private void OnNewDestinationPoint(DestinationPoint destinationPoint)
	{
		target = destinationPoint.GlobalPosition;
	}
}
