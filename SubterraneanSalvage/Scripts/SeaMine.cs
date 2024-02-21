using Godot;
using System;
using System.Reflection;

public partial class SeaMine : Area2D
{
	[Export] private Area2D areaPings;

	private Node2D lvlControlNode;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		lvlControlNode = GetParent<Node2D>();
	}

	private void OnPlayerEntered(Node2D body)
	{
		Modulate = new Color(1, 0, 0);
		LevelManager lvlMan = lvlControlNode as LevelManager;
		lvlMan.ForceTimeout();
	}

	private void OnPassiveSonarEnter(Area2D area)
	{
		Show();
	}

	private void OnPassiveSonarExit(Area2D area)
	{
		Hide();
	}
}
