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

	private float timer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!chasing)
		{
			//followPath.Progress += speed * (float)delta;
		}
		else
		{
			timer += (float)delta;
			if(timer < 3f) 
			{
				Vector2 direction = targetPos - Position;
				Position += direction.Normalized() * speed * (float)delta;
				GD.Print("chasing");
			}
			else
			{
				chasing = false;
				GD.Print("stopped chasing");
			}
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("PassiveSonar"))
		{
			//Show();
		}

		if (area.IsInGroup("NoiseArea"))
		{
			PlayerSubTest playerScript = area.GetParent() as PlayerSubTest;
			if (playerScript.makingNoise)
			{
				ChasePlayer(area.GlobalPosition);
			}
			GD.Print("chasing sound");
		}
	}


	private void OnAreaExited(Area2D area)
	{
		if (area.IsInGroup("PassiveSonar"))
		{
			//Hide();
		}
	}

	public void ChasePlayer(Vector2 soundOrigin)
	{
		if (chasing)
		{
			timer = 0;
			targetPos = soundOrigin;
			GD.Print("Still chasing");
		}
		if(chasing == false)
		{
			timer = 0;
			chasing = true;
			targetPos = soundOrigin;
			GD.Print(targetPos + "the hunt begins");
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			LevelManager lvlMan = GetOwner<Node2D>() as LevelManager;
			GD.Print(lvlMan);
			lvlMan.ForceTimeout();
		}
	}
}
