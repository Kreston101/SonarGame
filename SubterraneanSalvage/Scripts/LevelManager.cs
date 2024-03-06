using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class LevelManager : Node2D
{
	[Export] public CharacterBody2D player;
	[Export] public Area2D endPoint;
	[Export] public CanvasLayer UI;
	[Export] public Timer oxyTimer;
	[Export] public int oxyLossRate;
	[Export] public int levelNum;
	[Export] public float stageOxy;
	public Node2D endPointMaker;
	//stage collectibles

	public float oxyTimeLeft;
	public List<Node> layouts;

	public PlayerSubTest playerScript;
	private UIControl UIScript;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node scene = ResourceLoader.Load<PackedScene>($"res://Scenes/layouts/layout_{levelNum}.tscn").Instantiate();

		AddChild(scene);
		scene.Name = $"layout_{levelNum}";
		GD.Print("loaded layout");

		endPointMaker = (Node2D)GetNode($"layout_{levelNum}").GetChild(-1);
		endPoint.Position = endPointMaker.Position;
		GD.Print(endPointMaker);

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
		player.Modulate = new Color(1, 0, 0);
		GD.Print("dead");
	}

	public void ForceTimeout()
	{
		GD.Print("Timeoutforced");
		RootOnOxyTimerTimeout();
		player.Modulate = new Color(1, 0, 0);
	}

	public void DamagePlayer()
	{
		UIScript.DepleteOxy();
	}
}
