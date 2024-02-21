using Godot;
using System;
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
			followPath.Progress += speed * (float)delta;
		}
		else
		{
			timer += (float)delta;
			if(timer < 3f) 
			{
				Vector2 direction = targetPos - Position;
				Position += direction.Normalized() * speed * (float)delta;
			}
			else
			{
				chasing = false;
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
			PlayerSubTest playerScript = new PlayerSubTest();
			if (playerScript.makingNoise)
			{
				ChasePlayer(area.Position);
			}
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
		}
		if(chasing == false)
		{
			chasing = true;
			targetPos = soundOrigin;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		LevelManager lvlMan = GetParent() as LevelManager;
		lvlMan.ForceTimeout();
	}
}
