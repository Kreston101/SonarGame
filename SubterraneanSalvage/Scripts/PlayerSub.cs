using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class PlayerSub : CharacterBody2D
{
	public float speed = 300.0f;
	[Export] public float speedMultiplier = 1f;
	[Export] public const float pingCoolDown = 3f;
	[Export] public const float rangedPingCoolDown = 5f;
	[Export] public PackedScene rangedPingObj;

	private Node2D subBody;
	private Node2D rayCasts;
	private Node2D indicators;

	private float rangedPingTimer = rangedPingCoolDown;
	private float pingHoldTime;

	public override void _Ready()
	{
		subBody = (Node2D)GetNode("SubBody");
		rayCasts = (Node2D)GetNode("Raycasts");
		indicators = (Node2D)GetNode("Indicators");
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

		MoveAndSlide();
	}

	public void SubPing(float timeHeld)
	{
		GD.Print(timeHeld);
		RayCast2D ray;
		Node2D indicatorPing;

		if (timeHeld >= 5)
		{
			timeHeld = 5;
		}
		else if (timeHeld <= 1)
		{
			timeHeld = 1;
		}

		foreach (RayCast2D child in rayCasts.GetChildren())
		{
			child.TargetPosition *= timeHeld;
			GD.Print(child.TargetPosition);
		}

		for (int _i = 0;  _i <= rayCasts.GetChildCount() - 1; _i++)
		{
			ray = (RayCast2D)rayCasts.GetChild(_i);
			indicatorPing = (Node2D)indicators.GetChild(_i);
			if (ray.IsColliding())
			{
				indicatorPing.SelfModulate = Color.Color8(255, 255, 255, 255);
			}
			ray.TargetPosition /= timeHeld;
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
}
