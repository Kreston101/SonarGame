using Godot;
using System;
using System.Runtime.InteropServices;

public partial class PassiveCreature : Area2D
{
	public PathFollow2D followPath;
	[Export] public float speed;

	private float timer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		followPath = (PathFollow2D)GetParent();
		Hide();
		//GD.Print($"{followPath} {Name}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		followPath.Progress += speed * (float)delta;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("PassiveSonar"))
		{
			Show();
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if (area.IsInGroup("PassiveSonar"))
		{
			Hide();
		}
	}
}
