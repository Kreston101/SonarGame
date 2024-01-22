using Godot;
using System;
using System.Net.NetworkInformation;

public partial class Indicator : Sprite2D
{
	[Export] public float pingLifeTime = 1f;
    public bool active = false;

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

        //3-7s timer for ping strength (too long)
        //fade out after time

        if (active)
        {
            timer += (float)delta;
            if (timer > pingLifeTime)
            {
                SelfModulate = SelfModulate.Lerp(clear, 0.1f);
            }
            if(SelfModulate.A8 == 0)
            {
                active = false;
            }
        }
        else
        {
            if(timer != 0)
            {
                timer = 0;
            }
        }
    }
}
