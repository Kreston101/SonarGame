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
		lvlControlNode = (Node2D)GetTree().Root.GetChild(0); //GetOwner<Node2D>();
		//GD.Print(lvlControlNode);
	}

	private void OnPlayerEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			Modulate = new Color(1, 0, 0);
			LevelManager lvlMan = lvlControlNode as LevelManager;
			lvlMan.ForceTimeout();
			GD.Print("time out called sea mine");
		}
	}

	private void OnPassiveSonarEnter(Area2D area)
	{
		GD.Print(area.Name);
		Show();
	}
}
