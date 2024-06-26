using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
	private float timer = 0f;
	private float ambienceTimer = 0f;
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
			GD.Print("loading level");
			loadLevelTimer += (float)delta;
			if(loadLevelTimer > 1f)
			{
				UIScript.HideLevelCleared();
			}
			if (loadLevelTimer > 5f)
			{
				GD.Print("called loader");
				LoadLevel();
			}
		}

		if(playerDead == true)
		{
			timer += (float)delta;
			if(timer > 2f)
			{
				UIScript.HideGameOver();
				UIScript.ResetTint();
				ReloadLevel();
			}
		}

		if (levelLoaded)
		{
			if(levelNum >= 5)
			{
				ambienceTimer += (float)delta;
				if (ambienceTimer > 20f)
				{
					GetNode<AudioStreamPlayer>("MonsterAmbience").Play();
					ambienceTimer = 0f;
				}
			}
		}
	}

	public void LoadLevel()
	{
		GetTree().Paused = false;
		
		player.GlobalPosition = Vector2.Zero;
		
		//GD.Print("loading level");
		loadLevelTimer = 0f;
		playerScript.ResetSpeed();

		oxyTimer.WaitTime = stageOxy;
		UIScript.punishment = oxyLossRate;
		
		Node scene = ResourceLoader.Load<PackedScene>($"res://Scenes/layouts/layout_{levelNum}.tscn").Instantiate();

		AddChild(scene);
		scene.Name = $"layout_{levelNum}";
		//GD.Print(scene.Name);

		foreach(Node _child in scene.GetChildren())
		{
			//GD.Print("looping nodes");
			string _nodeName = _child.Name;
			Marker2D fishSpawnPoint = _child as Marker2D;
			if (_nodeName.Contains("FishSpawn"))
			{
				//GD.Print("found fish");
				Node2D hostileFishFab = (Node2D)ResourceLoader.Load<PackedScene>("res://Scenes/hostile_fish.tscn").Instantiate();
				hostileFishFab.GlobalPosition = fishSpawnPoint.GlobalPosition;
				AddChild(hostileFishFab);
			}
		}

		endPointMaker = (Node2D)GetNode($"layout_{levelNum}").GetChild(-1);
		endPoint.Position = endPointMaker.Position;
		endPoint.Show();

		player.Show();

		oxyTimer.Start();

		currentLayout = (Node2D)scene;
		//GD.Print(currentLayout.Name + " loaded");
		levelLoaded = true;
	}

	public void ForceTimeout()
	{
		//GD.Print("Timeoutforced");
		RootOnOxyTimerTimeout();
		player.Modulate = new Color(1, 0, 0);
	}

	private void RootOnOxyTimerTimeout()
	{
		GetNode<AudioStreamPlayer>("PlayerDeathSound").Play();
		GetTree().Paused = true;
		player.Modulate = new Color(1, 0, 0);
		UIScript.ShowGameOver();
		playerDead = true;
		GetChild(-1).QueueFree();
		//GD.Print("dead");
	}

	public void DamagePlayer()
	{
		UIScript.DepleteOxy();
	}

	private void OnEndPointEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			//GD.Print("level cleared");
			GetTree().Paused = true;
			UIScript.ShowLevelCleared();
			levelNum += 1;
			GetChild(-1).QueueFree();
			levelLoaded = false;
			player.Hide();
			endPoint.Hide();
			GetNode<AudioStreamPlayer>("LvlTransition").Play();
		}	
	}	

	private void ReloadLevel()
	{
		GetTree().Paused = false;
		player.GlobalPosition = Vector2.Zero;
		//GD.Print(player.GlobalPosition + "reload");
		playerDead = false;
		timer = 0f;
		levelLoaded = false;
		player.Hide();
		player.Modulate = new Color(1, 1, 1);
	}
}
