using Godot;
using System;
using System.Net.NetworkInformation;

public partial class Indicator : Sprite2D
{
	[Export] public float pingLifeTime = 3f;

	private float number = 255;
	private float timer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SelfModulate = Color.Color8(255, 255, 255, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

    }
}
