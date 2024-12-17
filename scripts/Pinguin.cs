using Godot;
using System;

public partial class Pinguin : RigidBody3D
{
	[Export]
	public float DistanceForAggro { get; private set; } = 45f;
	[Export]
	public float ShootCountdown { get; private set; } = 3f;
	public bool hasBeenHit = false;
	private Player player;
	private PackedScene bulletScene;
	private Timer shootTimer = new();
	private Marker3D riflePoint;
	private Vector3 directionToPlayer = new();
	public override void _Ready()
	{
		player = (Player)GetTree().GetFirstNodeInGroup("player");
		riflePoint = GetNode<Marker3D>("model/rifle/Marker3D");
		bulletScene = GD.Load<PackedScene>("res://scene/bullet.tscn");
		AddChild(shootTimer);
		shootTimer.Timeout += OnShootTimerTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsInstanceValid(player) && !hasBeenHit)
		{
			directionToPlayer = player.GlobalPosition - GlobalPosition;
			Vector3 unitDirectionToPlayerVector = directionToPlayer.Normalized();
			Rotation = Rotation with { Y = Mathf.Atan2(unitDirectionToPlayerVector.X, unitDirectionToPlayerVector.Z) };
			CheckCanAggroPlayer();
		}
	}

	private void CheckCanAggroPlayer()
	{
		if (directionToPlayer.Length() < DistanceForAggro && shootTimer.TimeLeft <= 0)
		{
			shootTimer.Start(ShootCountdown);
		}
	}

	private void OnShootTimerTimeout()
	{
		Bullet bullet = (Bullet)bulletScene.Instantiate();
		GetTree().CurrentScene.AddChild(bullet);
		bullet.GlobalPosition = riflePoint.GlobalPosition;
		bullet.Direction = directionToPlayer.Normalized();
		shootTimer.Stop();
	}
}
