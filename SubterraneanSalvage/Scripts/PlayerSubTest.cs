using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class PlayerSubTest : CharacterBody2D
{
	[Export] public float speed = 300.0f;
	[Export] public float speedMultiplier = 1f;
	[Export] public const float pingCoolDown = 3f;
	[Export] public const float rangedPingCoolDown = 5f;
	[Export] public PackedScene rangedPingObj;
	[Export] public PackedScene areaPingObj;

	//temp until level manager
	[Export] public Node HealthBar;

	public float minPingTime = 1f;
	public float maxPingTime = 5f;

	private Node2D subBody;
	private Node2D rayCasts;

	private float rangedPingTimer = rangedPingCoolDown;
	private float pingHoldTime;

	public override void _Ready()
	{
		subBody = (Node2D)GetNode("SubBody");
		rayCasts = (Node2D)GetNode("Raycasts");
	}

	public override void _PhysicsProcess(double delta)
	{
		rangedPingTimer += (float)delta;

		//input control
		Vector2 velocity = new Vector2();
		if (Input.IsActionPressed("Left"))
		{
			velocity.X = -1;
			subBody.Scale = new Vector2(-1, 1);
		}
		if (Input.IsActionPressed("Right"))
		{
			velocity.X = 1;
			subBody.Scale = new Vector2(1, 1);
		}
		if (Input.IsActionPressed("Up"))
		{
			velocity.Y = -1;
		}
		if (Input.IsActionPressed("Down"))
		{
			velocity.Y = 1;
		}

		//speed control, faster is louder
		if (Input.IsActionJustPressed("Speed+"))
		{
			if (speedMultiplier < 1.5f)
			{
				speedMultiplier += 0.5f;
			}
			//GD.Print(speedMultiplier);
		}
		if (Input.IsActionJustPressed("Speed-"))
		{
			if (speedMultiplier > 0.5f)
			{
				speedMultiplier -= 0.5f;
			}
			//GD.Print(speedMultiplier);
		}

		Position += velocity.Normalized() * speed * speedMultiplier * (float)delta;

		//pings around the sub, checking for nearby walls/objs/creatures
		//press and hold down for longer ping duration and range
		//but louder sound
		if (Input.IsActionPressed("Ping"))
		{
			pingHoldTime += (float)delta;
			GD.Print(pingHoldTime);
		}

		if (Input.IsActionJustReleased("Ping"))
		{
			SubPing(pingHoldTime);
			pingHoldTime = 0;
		}

		if (Input.IsActionJustPressed("FirePing"))
		{
			RangedPing();
		}

		//handle collision damage
		if (MoveAndSlide())
		{
			UIControl oxyTestScript = HealthBar as UIControl;
			oxyTestScript.DepleteOxy();
			Position -= velocity.Normalized() * 50; //nudge the sub backwards
		}
	}

	public void SubPing(float timeHeld)
	{
		GD.Print("held for " + timeHeld);

		if (timeHeld >= maxPingTime)
		{
			timeHeld = maxPingTime;
		}
		else if (timeHeld <= minPingTime)
		{
			timeHeld = minPingTime;
		}
		else
		{
			timeHeld = (float)Math.Round(timeHeld);
			GD.Print(timeHeld + " abs");
		}

		for (int i = 1; i <= 12; i++)
		{
			Node2D ping = (Node2D)areaPingObj.Instantiate();
			AddSibling(ping);
			ping.Position = GlobalPosition;
			AreaPing pingObjScript = ping as AreaPing;
			pingObjScript.maxLifetime = timeHeld;
			GD.Print(pingObjScript.maxLifetime);
			float angle = i * 0.52f;
			pingObjScript.direction = new Vector2(MathF.Cos(angle),MathF.Sin(angle));
		}
	}

	public void RangedPing()
	{
		if (rangedPingTimer >= rangedPingCoolDown)
		{
			if (GetParent().GetNodeOrNull("RangedPing") == null)
			{
				Node2D ping = (Node2D)rangedPingObj.Instantiate();
				AddSibling(ping);
				ping.Position = GlobalPosition;
				RangedPing pingObjScript = ping as RangedPing;
				pingObjScript.direction = (GetGlobalMousePosition() - pingObjScript.GlobalPosition).Normalized();
				rangedPingTimer = 0;
			}
		}
	}

	private void TestArea(Area2D area)
	{
		//GD.Print("detection");
	}
}
