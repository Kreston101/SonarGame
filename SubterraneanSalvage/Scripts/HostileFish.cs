using Godot;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;

public partial class HostileFish : CharacterBody2D
{
	public float speed = 150f;
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
		chasing = false;
		withinNoise = false;
		lvlMan = (LevelManager)GetTree().Root.GetChild(0);
		CreateDirection();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!chasing)
		{
			if (timer < 3f)
			{
				GD.Print("patrol");
				timer += (float)delta;
				KinematicCollision2D collision = MoveAndCollide(direction.Normalized() * speed * (float)delta);
				if (collision != null)
				{
					CreateDirection();
					Node2D checkGroup = (Node2D)collision.GetCollider();
					if (checkGroup.IsInGroup("Player"))
					{
						GD.Print("Nom");
						lvlMan.ForceTimeout();
					}
					timer = 0f;
				}
			}
			else
			{
				CreateDirection();
				timer = 0f;
			}
		}
		else
		{
			if (withinNoise)
			{
				GD.Print("heard sound");
				ChasePlayer(lvlMan.player.GlobalPosition);
				MoveAndCollide(direction.Normalized() * speed * (float)delta);
				KinematicCollision2D collision = MoveAndCollide(direction.Normalized() * speed * (float)delta);
				if (collision != null)
				{
					Node2D checkGroup = (Node2D)collision.GetCollider();
					if (checkGroup.IsInGroup("Player"))
					{
						GD.Print("Nom");
						lvlMan.ForceTimeout();
					}
				}
			}
			else
			{
				//GD.Print("heard/lost sound");
				MoveAndCollide(direction.Normalized() * speed * (float)delta);
				KinematicCollision2D collision = MoveAndCollide(direction.Normalized() * speed * (float)delta);
				if (collision != null)
				{
					Node2D checkGroup = (Node2D)collision.GetCollider();
					if (checkGroup.IsInGroup("Player"))
					{
						GD.Print("Nom");
						lvlMan.ForceTimeout();
					}
				}
				timer += (float)delta;
				Vector2 distToLastHeard = targetPos - Position;
				GD.Print(distToLastHeard.Length());
				if (timer > 3f || distToLastHeard.Length() <= 15f)
				{
					chasing = false;
					timer = 0;
					speed = 150f;
				}
			}
		}
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
		GetNode<AudioStreamPlayer2D>("AggroFish").Play();
		speed = 150f;
		chasing = true;
		targetPos = soundOrigin;
		direction = targetPos - Position;
		//GD.Print("chase player called");
	}
}
