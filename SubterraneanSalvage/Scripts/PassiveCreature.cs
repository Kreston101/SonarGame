using Godot;
using System;
using System.Runtime.InteropServices;

public partial class PassiveCreature : Area2D
{
	[Export] public MeshInstance2D mesh;
	[Export] public PathFollow2D followPath;
	[Export] public float speed;

	private float timer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		followPath.Progress += speed * (float)delta;
	}

	private void OnSonarEntered(Area2D area)
	{
		if (area.IsInGroup("PassiveSonar"))
		{
			Show();
		}
	}

	private void OnSonarExit(Area2D area)
	{
		if (area.IsInGroup("PassiveSonar"))
		{
			Hide();
		}
	}
}
