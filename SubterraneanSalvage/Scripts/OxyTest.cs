using Godot;
using System;

public partial class OxyTest : CanvasLayer
{
	[Export] Timer OxyTimer;
	[Export] TextureProgressBar OxyBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		OxyBar.Value = OxyTimer.TimeLeft;
	}

	public void DepleteOxy()
	{
		GD.Print("lost oxygen");
		OxyTimer.Start(OxyTimer.TimeLeft - 30f);
		//hard value for now
		//actual value is, Sub speed * punishmentAmount
		//punishmentAmount, fixed time to loose in seconds, earlier stages, more forgiving
		//think of it as hull damage due to pressure change
		//does that make sense?
		//the screen goes RED, remember
	}
}
