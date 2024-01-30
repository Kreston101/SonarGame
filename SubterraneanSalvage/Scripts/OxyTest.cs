using Godot;
using System;

public partial class OxyTest : CanvasLayer
{
	[Export] Timer OxyTimer;
	[Export] TextureProgressBar OxyBar;
	[Export] ColorRect redTint;

	private float timer = 0;
	private Color red = new Color(1, 0, 0, 0.34f);
	private Color clear = new Color(1, 1, 1, 0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		redTint.Color = clear;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		OxyBar.Value = OxyTimer.TimeLeft;
		redTint.Color = redTint.Color.Lerp(clear, 0.1f);
	}

	public void DepleteOxy()
	{
		GD.Print("lost oxygen");
		OxyTimer.Start(OxyTimer.TimeLeft - 10f);
		redTint.Color = red;
		//hard value for now
		//actual value is, Sub speed * punishmentAmount
		//punishmentAmount, fixed time to loose in seconds, earlier stages, more forgiving
		//think of it as hull damage due to pressure change
		//does that make sense?
		//the screen goes RED, remember
	}
}
