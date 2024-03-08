using Godot;
using System;

public partial class UIControl : CanvasLayer
{
	[Export] Timer oxyTimer;
	[Export] TextureProgressBar oxyBar;
	[Export] ColorRect redTint;
	[Export] Label lvlClear;
	[Export] Label gameOver;
	public int punishment;
	public float playerSpeed;

	private float timer = 0;
	private Color red = new Color(1, 0, 0, 0.34f);
	private Color clear = new Color(1, 1, 1, 0);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		redTint.Color = clear;
		lvlClear.Hide();
		gameOver.Hide();
		oxyBar.MaxValue = oxyTimer.WaitTime;
		oxyBar.Value = oxyBar.MaxValue;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		oxyBar.Value = oxyTimer.TimeLeft;
		redTint.Color = redTint.Color.Lerp(clear, 0.1f);
	}

	public void DepleteOxy()
	{
		GD.Print("lost oxygen");
		if(oxyTimer.TimeLeft - (playerSpeed * punishment) > 0)
		{
			oxyTimer.Start(oxyTimer.TimeLeft - (playerSpeed * punishment));
		}
		else
		{
			oxyTimer.Stop();
			oxyBar.Value = 0;
			ShowGameOver();
			LevelManager parent = GetParent() as LevelManager;
			parent.ForceTimeout();
			GD.Print("time out called UI");
		}
		redTint.Color = red;
	}

	public void ShowLevelCleared()
	{
		lvlClear.Show();
	}

	public void ShowGameOver()
	{
		gameOver.Show();
	}
}
