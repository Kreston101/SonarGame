using Godot;
using System;

public partial class PlayerSub : CharacterBody2D
{
	public float speed = 300.0f;

	private Node2D subBody;

	public override void _Ready()
	{
		subBody = (Node2D)GetNode("SubBody");
	}

	public override void _PhysicsProcess(double delta)
	{
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

		Position += velocity * speed * (float)delta;

		if(velocity == Vector2.Zero)
		{
			RotationDegrees = 0;
		}
	}
}
