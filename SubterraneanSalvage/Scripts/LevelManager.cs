using Godot;
using System;

public partial class LevelManager : Node2D
{
	[Export] public CharacterBody2D player;
	[Export] public Area2D endPoint;
	[Export] public CanvasLayer UI;
	[Export] public Timer oxyTimer;
	[Export] public int oxyLossRate;
	[Export] public int levelNum;
	[Export] public float stageOxy;
	//stage collectibles

	public float oxyTimeLeft;

	public PlayerSubTest playerScript;
	private UIControl UIScript;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerScript = player as PlayerSubTest;
		UIScript = UI as UIControl;
		oxyTimer.WaitTime = stageOxy;
		UIScript.punishment = oxyLossRate;
		oxyTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		oxyTimeLeft = (float)oxyTimer.TimeLeft;
		UIScript.playerSpeed = playerScript.speedMultiplier;
	}

	private void RootOnOxyTimerTimeout()
	{
		Engine.TimeScale = 0;
		GD.Print("dead");
	}

	public void ForceTimeout()
	{
		RootOnOxyTimerTimeout();
		player.Modulate = new Color(1, 0, 0);
	}
}
