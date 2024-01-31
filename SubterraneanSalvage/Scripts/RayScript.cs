using Godot;
using System;

public partial class RayScript : RayCast2D
{
	public Vector2 minRange;
	[Export] public Sprite2D linkedIndicator;
	public float pingHeldTime;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		minRange = TargetPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

    }

	public void SetRange(float _pingHeldTime)
	{
		pingHeldTime = _pingHeldTime;
		TargetPosition *= _pingHeldTime;
		GD.Print("raycasting to " + TargetPosition);
	}

	public void CheckCast()
	{
		ForceRaycastUpdate();
        if (IsColliding())
        {
            linkedIndicator.SelfModulate = Color.Color8(255, 255, 255, 255);
            Indicator indicatorScript = linkedIndicator as Indicator;
            indicatorScript.pingLifeTime = pingHeldTime;
            indicatorScript.active = true;
			TargetPosition = minRange;
        }
		else
		{
			GD.Print("nothing");
			TargetPosition = minRange;
		}
    }
}
