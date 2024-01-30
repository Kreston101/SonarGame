using Godot;
using System;
using System.Runtime.InteropServices;

public partial class PassiveCreature : Area2D
{
	[Export] public MeshInstance2D mesh;

	private bool moveDown = true;
	private float timer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += (float)delta;

		if(timer > 3) 
		{
			moveDown = !moveDown;
			timer = 0;
		}

		if (moveDown)
		{
			Position += Vector2.Down;
		}
		else
		{
			Position += Vector2.Up;
		}
	}

	private void OnSonarEntered(Area2D area)
	{
		Show();
	}

	private void OnSonarExit(Area2D area)
	{
		Hide();
	}
}
