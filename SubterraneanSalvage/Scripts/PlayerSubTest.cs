using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class PlayerSubTest : CharacterBody2D
{
	[Export] public float speed = 300.0f;
	[Export] public float speedMultiplier = 1f;
	[Export] public const float pingCoolDown = 3f;
	//[Export] public const float rangedPingCoolDown = 5f;
	[Export] public PackedScene rangedPingObj;
	[Export] public PackedScene areaPingObj;
	[Export] public Area2D noiseArea;

	//temp until level manager
	[Export] public Node levelManager;

	//public float minPingTime = 0.5f;
	public float maxPingTime = 5f;
	public bool hasHitWall = false;
	public bool makingNoise = false;

	private Node2D subBody;
	//private Node2D rayCasts;

	//private float rangedPingTimer = rangedPingCoolDown;
	private float pingHoldTime;

	public override void _Ready()
	{
		subBody = (Node2D)GetNode("SubBody");
		//rayCasts = (Node2D)GetNode("Raycasts");
	}

	public override void _PhysicsProcess(double delta)
	{
		//rangedPingTimer += (float)delta;

		if (Input.IsActionJustPressed("Speed+"))
		{
			if (speedMultiplier < 1.5f)
			{
				speedMultiplier += 0.5f;
				noiseArea.Scale += new Vector2(0.33f, 0.33f);
			}
			//GD.Print(speedMultiplier);
			//GD.Print(noiseArea.Scale);
		}
		if (Input.IsActionJustPressed("Speed-"))
		{
			if (speedMultiplier > 0.5f)
			{
				speedMultiplier -= 0.5f;
				noiseArea.Scale -= new Vector2(0.33f, 0.33f);
			}
			//GD.Print(speedMultiplier);
			//GD.Print(noiseArea.Scale);
		}

		//pings around the sub, checking for nearby walls/objs/creatures
		//press and hold down for longer ping duration and range
		//but louder sound
		if (Input.IsActionPressed("Ping"))
		{
			pingHoldTime += (float)delta;
			//GD.Print(pingHoldTime);
		}

		if (Input.IsActionJustReleased("Ping"))
		{
			//Node2D mark = (Node2D)rangedPingObj.Instantiate();
			//AddSibling(mark);
			//mark.Position = GlobalPosition;
			SubPing(pingHoldTime);
			pingHoldTime = 0;
		}

		//if (Input.IsActionJustPressed("FirePing"))
		//{
		//	RangedPing();
		//}

		//input control
		GetInput();

		if(Velocity == Vector2.Zero)
		{
			makingNoise = false;
		}
		else
		{
			GD.Print("current =" + Velocity + " " + makingNoise);
			makingNoise = true;
		}

		//handle collision damage
		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			GD.Print("collision");
			Node2D checkGroup = (Node2D)collision.GetCollider();
			if (checkGroup.IsInGroup("Terrain"))
			{
				if (hasHitWall == false)
				{
					GD.Print("hit wall");
					hasHitWall = true;
					LevelManager lvlManScript = levelManager as LevelManager;
					lvlManScript.DamagePlayer();
				}
			}
			else if (checkGroup.IsInGroup("Hostile"))
			{
				GD.Print("hit fish");
				LevelManager lvlManScript = levelManager as LevelManager;
				lvlManScript.ForceTimeout();
				GD.Print("time out called plyer");
			}
		}
		else
		{
			hasHitWall = false;
		}
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		Velocity = inputDirection.Normalized() * speed * speedMultiplier;
		if(Velocity.X < 0)
		{
			subBody.Scale = new Vector2(-1f, 1f);
		}
		if (Velocity.X > 0)
		{
			subBody.Scale = new Vector2(1f, 1f);
		}
	}

	public void SubPing(float timeHeld)
	{
		//GD.Print("held for " + timeHeld);

		if (timeHeld >= maxPingTime)
		{
			timeHeld = maxPingTime;
		}
		else
		{
			timeHeld = (float)Math.Round(timeHeld);
			//GD.Print(timeHeld + " abs");
		}

		for (int i = 1; i <= 30; i++)
		{
			Node2D ping = (Node2D)areaPingObj.Instantiate();
			AddSibling(ping);
			AreaPing pingObjScript = ping as AreaPing;
			ping.Position = GlobalPosition;
			pingObjScript.origin = GlobalPosition;
			pingObjScript.maxLifetime = timeHeld;
			//GD.Print(pingObjScript.maxLifetime);
			float angle = i * ((360/30) * (Mathf.Pi/180));
			//GD.Print(angle);
			pingObjScript.direction = new Vector2(MathF.Cos(angle),MathF.Sin(angle));
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Hostile"))
		{
			if(makingNoise == true)
			{
				GD.Print("alerted");
				HostileFish madFish = body as HostileFish;
				madFish.ChasePlayer(GlobalPosition);
				madFish.withinNoise = true;
			}
		}
	}

	private void OnBodyExit(Node2D body)
	{
		if (body.IsInGroup("Hostile"))
		{
			GD.Print("outran");
			HostileFish madFish = body as HostileFish;
			madFish.withinNoise = false;
		}
	}

	//public void RangedPing()
	//{
	//	if (rangedPingTimer >= rangedPingCoolDown)
	//	{
	//		if (GetParent().GetNodeOrNull("RangedPing") == null)
	//		{
	//			Node2D ping = (Node2D)rangedPingObj.Instantiate();
	//			AddSibling(ping);
	//			ping.Position = GlobalPosition;
	//			RangedPing pingObjScript = ping as RangedPing;
	//			pingObjScript.direction = (GetGlobalMousePosition() - pingObjScript.GlobalPosition).Normalized();
	//			rangedPingTimer = 0;
	//		}
	//	}
	//}

	//private void SonarArea(Area2D area)
	//{
	//	GD.Print("detection" + area.Name);
	//}
}
