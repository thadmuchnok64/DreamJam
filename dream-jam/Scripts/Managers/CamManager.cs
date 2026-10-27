using Godot;
using System;

public partial class CamManager : Node3D
{
	[Export] Node3D trackingObj;
	[Export] Curve lerpCurve;
	[Export] float minDis, maxDis;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GlobalPosition = trackingObj.GlobalPosition;
		//float lerpVal = ((GlobalPosition.DistanceTo(trackingObj.GlobalPosition) - minDis) / maxDis).Clamp01();
		//GD.Print(lerpVal);
		//GlobalPosition = GlobalPosition.Lerp(trackingObj.GlobalPosition, lerpCurve.Sample(lerpVal*(float)delta));
	}

    public override void _PhysicsProcess(double delta)
    {
        float lerpVal = ((GlobalPosition.DistanceTo(trackingObj.GlobalPosition) - minDis) / maxDis).Clamp01();
        GD.Print(lerpVal);
        GlobalPosition = GlobalPosition.Lerp(trackingObj.GlobalPosition, lerpCurve.Sample(lerpVal * (float)delta));
    }
}
