using Godot;
using Godot.NativeInterop;
using System;
using static Godot.TextServer;

public partial class AreaPing : Area2D
{
	[Export] public float speed;
	public Vector2 direction;
	public float maxLifetime = 1f;
	[Export] public float padding = 10f;
	public Vector2 origin;

	private float lifeTime = 0f;
	private Color clear = new Color(1, 1, 1, 0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		origin = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += direction * speed * (float)delta;
		lifeTime += (float)delta;

		//might need to run this check seperately elsewhere
		if (lifeTime > maxLifetime)
		{
			Modulate = Modulate.Lerp(clear, 0.1f);
			if (Modulate.A8 == 0)
			{
				QueueFree();
			}
		}
	}

	private void OnBodyShapeEntered(Rid body_rid, Node2D body, long body_shape_index, long local_shape_index)
	{
		if (body.IsInGroup("Terrain"))
		{
			Position -= direction * padding;
			direction = Vector2.Zero;
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("Hostile"))
		{
			//send position
			//set enemy in track mode
		}
	}
}
