using Godot;
using System;

public partial class PlayerSub : CharacterBody2D
{
	public float speed = 300.0f;
	[Export] public float speedMultiplier = 1f;

	private Node2D subBody;
	private Node2D rayCasts;
	private Node2D indicators;

	private float timer = 0;

	public override void _Ready()
	{
		subBody = (Node2D)GetNode("SubBody");
		rayCasts = (Node2D)GetNode("Raycasts");
		indicators = (Node2D)GetNode("Indicators");
	}

	public override void _PhysicsProcess(double delta)
	{
		timer += (float)delta;

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
			GD.Print(speedMultiplier);
		}
		if (Input.IsActionJustPressed("Speed-"))
		{
			if (speedMultiplier > 0.5f)
			{
				speedMultiplier -= 0.5f;
			}
			GD.Print(speedMultiplier);
		}

		Position += velocity.Normalized() * speed * speedMultiplier * (float)delta;

		//pings around the sub, checking for nearby walls/objs/creatures
		if (Input.IsActionJustPressed("Ping"))
		{
			SubPing();
		}

		MoveAndSlide();

		float limit = 2f;
		if (timer >= limit)
		{
			GD.Print("ran");
			ExpirePing();
			timer = 0;
		}
	}

	public void SubPing()
	{
		RayCast2D ray;
		Node2D indicatorPing;
		for(int _i = 0;  _i <= rayCasts.GetChildCount() - 1; _i++)
		{
			ray = (RayCast2D)rayCasts.GetChild(_i);
			indicatorPing = (Node2D)indicators.GetChild(_i);
			if (ray.IsColliding())
			{
				indicatorPing.Show();
			}
			else
			{
				indicatorPing.Hide();
			}
		}
	}

	public void ExpirePing()
	{
		foreach(Node2D child in indicators.GetChildren())
		{
			child.Hide();
		}
	}
}
