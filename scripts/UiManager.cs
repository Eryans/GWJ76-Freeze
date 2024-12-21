using Godot;
using System;

public partial class UiManager : Control
{
	private RichTextLabel gameOverText;

	public override void _ExitTree()
	{
		Player.GameOver -= OnGameOver;
	}
	public override void _Ready()
	{
		gameOverText = GetNode<RichTextLabel>("%GameOverText");
		gameOverText.Visible = false;
		Player.GameOver += OnGameOver;
	}
	private void OnGameOver()
	{
		gameOverText.Visible = true;
	}
}
