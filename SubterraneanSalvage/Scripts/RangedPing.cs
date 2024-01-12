using Godot;
using System;

public partial class RangedPing : Area2D
{
	[Export] public float speed;
	public Vector2 direction;
	[Export] public float maxLifetime = 5f;
	[Export] public float padding = 10f;

	private float lifetime = 0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += direction * speed * (float)delta;
		lifetime += (float)delta;

		//might need to run this check seperately elsewhere
		if (lifetime > maxLifetime)
		{
			QueueFree();
		}
	}

	//private void OnBodyEntered(Node2D body)
	//{
	//	direction = Vector2.Zero;
	//}

	private void OnBodyShapeEntered(Rid body_rid, Node2D body, long body_shape_index, long local_shape_index)
	{
		if(body != GetParent())
		{
			Position -= direction * padding;
			direction = Vector2.Zero;
		}
	}
	
	//when colliding with wall, cut al velocites to 0,
	//but let the ping stay there until it expires
	//seperate flight & life time?
	//fly for x time, once contacting obj stay alive for x time?
	//only spawn now ping after old one has expired
	//who handles that?
}
