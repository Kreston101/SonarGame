using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class PlayerSub : CharacterBody2D
{
	[Export] public float speed = 300.0f;
	[Export] public float speedMultiplier = 1f;
	[Export] public const float pingCoolDown = 3f;
	[Export] public const float rangedPingCoolDown = 5f;
	[Export] public PackedScene rangedPingObj;

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
			subBody.Scale = new Vector2(-1,1);
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
			if(speedMultiplier < 1.5f)
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
		//press and hol down for longer ping duration and range
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
			OxyTest oxyTestScript = HealthBar as OxyTest;
			oxyTestScript.DepleteOxy();
			Position -= velocity.Normalized() * 50;
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

		foreach (RayCast2D child in rayCasts.GetChildren())
		{
			RayScript childScript = child as RayScript;
			childScript.SetRange(timeHeld);
			childScript.CheckCast();
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
		GD.Print("detection");
	}
}
