using Godot;
using System;

[GlobalClass]
public partial class HurtBox : Area3D
{
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area3D area)
	{
		if (area is Bullet)
		{
			GD.Print("outch ! :(");
		}
	}
}
