using Godot;
using System;
using System.Diagnostics;
using System.Reflection;

public partial class HostileFish : Area2D
{
	[Export] public MeshInstance2D mesh;
	[Export] public PathFollow2D followPath;
	[Export] public float speed;
	public bool chasing = false;
	public Vector2 targetPos;
	public Vector2 direction;
	public bool withinNoise = false;

	private float timer = 0;
	private LevelManager lvlMan;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lvlMan = GetOwner<Node2D>() as LevelManager;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (chasing)
		{
			Position += direction.Normalized() * speed * (float)delta;
			if (withinNoise)
			{
				ChasePlayer(lvlMan.player.GlobalPosition);
			}
			else
			{
				//GD.Print("lag mode");
				timer += (float)delta;
				if (timer > 3f)
				{
					chasing = false;
					timer = 0;
				}
			}
		}
		else
		{
			//follow path, path find, something
		}
	}

	//trigger
	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("NoiseArea"))
		{
			//GD.Print("in area");
			if (lvlMan.playerScript.makingNoise == true) //only trigger if ther player is moving
			{
				withinNoise = true; //only enter exit trigger, assume stay if no exit
				ChasePlayer(area.GlobalPosition); //get the player coords
			}
			//GD.Print("chasing sound");
			//GD.Print(area.GlobalPosition);
		}
	}


	private void OnAreaExited(Area2D area)
	{
		if (area.IsInGroup("NoiseArea"))
		{
			withinNoise = false;
			//GD.Print("leaft area");
		}
	}

	public void ChasePlayer(Vector2 soundOrigin)
	{
		chasing = true;
		targetPos = soundOrigin;
		direction = targetPos - Position;
		//GD.Print("Still chasing");
	}

	private void OnBodyEntered(Node2D body)
	{
		//if (body.IsInGroup("Player"))
		//{
		//	lvlMan.ForceTimeout();
		//}
	}
}
