using Godot;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;

public partial class HostileFish : CharacterBody2D
{
	[Export] public float speed;
	[Export] public RayCast2D raycast;
	//[Export] public Line2D debugLine;
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
		CreateDirection();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!chasing)
		{
			if (timer < 3f)
			{
				//GD.Print("patrol");
				timer += (float)delta;
				KinematicCollision2D collision = MoveAndCollide(direction.Normalized() * speed * (float)delta);
				if (collision != null)
				{
					CreateDirection();
					timer = 0f;
				}
			}
			else
			{
				CreateDirection();
				timer = 0f;
			}
		}
		else //chasing
		{
			if (withinNoise)
			{
				ChasePlayer(lvlMan.player.GlobalPosition);
				MoveAndCollide(direction.Normalized() * speed * (float)delta);
				//GD.Print("chasing sound");
			}
			else
			{
				//GD.Print("lost the sounds");
				MoveAndCollide(direction.Normalized() * speed * (float)delta);
				timer += (float)delta;
				Vector2 distToLastHeard = targetPos - Position;
				//GD.Print(distToLastHeard);
				if (timer > 3f || distToLastHeard <= direction.Normalized())
				{
					chasing = false;
					timer = 0;
				}
			}
		}

		//if (chasing)
		//{
		//	Position += direction.Normalized() * speed * (float)delta;
		//	if (withinNoise)
		//	{
		//		ChasePlayer(lvlMan.player.GlobalPosition);
		//	}
		//	else
		//	{
		//		//GD.Print("lag mode");
		//		timer += (float)delta;
		//		if (timer > 3f)
		//		{
		//			chasing = false;
		//			timer = 0;
		//		}
		//	}
		//}
		//else
		//{
		//	if(timer < 3f)
		//	{
		//		Position += direction * speed * (float)delta;
		//		timer += (float)delta;
		//	}
		//	else
		//	{
		//		CreateDirection();
		//		raycast.TargetPosition = direction * 75f;
		//		debugLine.ClearPoints();
		//		debugLine.AddPoint(Vector2.Zero);
		//		debugLine.AddPoint(raycast.TargetPosition);
		//		raycast.ForceRaycastUpdate();
		//		if (raycast.IsColliding())
		//		{
		//			GD.Print("raycast hit");
		//			direction = Vector2.Zero;
		//		}
		//		else
		//		{
		//			timer = 0f;
		//		}
		//	}
		//}
	}

	private void CreateDirection()
	{
		RandomNumberGenerator rng = new();
		float randX = rng.RandfRange(-1f, 1f);
		float randY = rng.RandfRange(-1f, 1f);
		direction = new Vector2(randX, randY).Normalized();
		//GD.Print(direction);
	}

	public void ChasePlayer(Vector2 soundOrigin)
	{
		chasing = true;
		targetPos = soundOrigin;
		direction = targetPos - Position;
		//GD.Print("chase player");
	}
}
