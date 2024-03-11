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
	private Node2D currentLayout;
	private bool levelLoaded = false;
	private float loadLevelTimer = 0f;
	private bool playerDead = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerScript = player as PlayerSubTest;
		UIScript = UI as UIControl;

		LoadLevel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		oxyTimeLeft = (float)oxyTimer.TimeLeft;
		UIScript.playerSpeed = playerScript.speedMultiplier;

		if (levelLoaded == false)
		{
			loadLevelTimer += (float)delta;
			if (loadLevelTimer > 5f)
			{
				LoadLevel();
			}
		}
	}

	public void LoadLevel()
	{
		UIScript.HideLevelCleared();
		GD.Print("loading level");
		loadLevelTimer = 0f;

		oxyTimer.WaitTime = stageOxy;
		UIScript.punishment = oxyLossRate;
		
		Node scene = ResourceLoader.Load<PackedScene>($"res://Scenes/layouts/layout_{levelNum}.tscn").Instantiate();

		AddChild(scene);
		scene.Name = $"layout_{levelNum}";
		GD.Print(scene.Name);

		endPointMaker = (Node2D)GetNode($"layout_{levelNum}").GetChild(-1);
		endPoint.Position = endPointMaker.Position;
		endPoint.Show();

		player.GlobalPosition = Vector2.Zero;
		player.Show();
		playerDead = false;

		oxyTimer.Start();

		currentLayout = (Node2D)scene;
		GD.Print(currentLayout.Name + " loaded");
		levelLoaded = true;
	}

	private void RootOnOxyTimerTimeout()
	{
		GetTree().Paused = true;
		player.Modulate = new Color(1, 0, 0);
		UIScript.ShowGameOver();
		playerDead = true;
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

	private void OnEndPointEntered(Node2D body)
	{
		UIScript.ShowLevelCleared();
		levelNum += 1;
		GetChild(-1).QueueFree();
		player.Hide();
		endPoint.Hide();
		levelLoaded = false;
	}
}
