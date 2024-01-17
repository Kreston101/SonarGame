using Godot;
using System;
using System.Net.NetworkInformation;

public partial class Indicator : Sprite2D
{
	[Export] public float pingLifeTime = 3f;

	private float number = 255;
	private float timer = 0;

	private Color clear = new Color(1, 1, 1, 0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SelfModulate = clear;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //math to determin fade out tie, keep it slow?
        //3 second min
        //x seconds full ping?
        //seperate lifetime from fade time?

		//3-7s timer for ping strength
		//fade out after time

		if (SelfModulate.A > 0)
		{
			timer += (float)delta;
			if(timer > pingLifeTime)
			{
				SelfModulate = SelfModulate.Lerp(clear, 0.3f);
				timer = 0;
            }
		}
	}
}
