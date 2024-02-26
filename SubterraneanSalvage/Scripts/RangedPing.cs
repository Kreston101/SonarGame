using Godot;
using Godot.NativeInterop;
using System;

public partial class RangedPing : Area2D
{
	//[Export] public float speed;
	//public Vector2 direction;
	//[Export] public float maxLifetime = 5f;
	//[Export] public float padding = 10f;

	//private float lifeTime = 0f;

	//// Called when the node enters the scene tree for the first time.
	//public override void _Ready()
	//{
	//}

	//// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//	Position += direction * speed * (float)delta;
	//	lifeTime += (float)delta;

	//	//might need to run this check seperately elsewhere
	//	if (lifeTime > maxLifetime)
	//	{
	//		QueueFree();
	//	}
	//}

	//private void OnBodyShapeEntered(Rid body_rid, Node2D body, long body_shape_index, long local_shape_index)
	//{
	//	if(body.IsInGroup("Terrain"))
	//	{
	//		Position -= direction * padding;
	//		direction = Vector2.Zero;
	//	}
	//}
	
	//when colliding with wall, cut al velocites to 0,
	//but let the ping stay there until it expires
	//seperate flight & life time?
	//fly for x time, once contacting obj stay alive for x time?
	//only spawn now ping after old one has expired
	//who handles that?
}
