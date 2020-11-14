using Godot;
using System;

public class Map : Node2D
{
	public override void _Ready()
	{
		SetCameraLimits();
		Input.SetCustomMouseCursor(GD.Load("res://Assets/UI/crossair_black.png"), Input.CursorShape.Arrow, new Vector2(16,16));
	}

	private void SetCameraLimits()
	{
		var ground = (TileMap)GetNode("Ground");
		var bounds = ground.GetUsedRect();
		var cellsize = ground.CellSize;
		var player = (KinematicBody2D)GetNode("Player");
		var cam = (Camera2D)player.GetNode("Camera2D");
		cam.LimitLeft = (int)(bounds.Position.x * cellsize.x);
		cam.LimitRight = (int)(bounds.End.x * cellsize.x);
		cam.LimitTop = (int)(bounds.Position.y * cellsize.y);
		cam.LimitBottom = (int)(bounds.End.y * cellsize.y);
	}

	public void OnTankShoot(PackedScene bullet, Vector2 position, Vector2 direction, Node2D target=null)
	{
		var b = bullet.Instance() as Bullet;
		AddChild(b);
		b.Start(position, direction, target);
	}

 	public void OnPlayerDead()
	{
		GetTree().ReloadCurrentScene();
	}
}
