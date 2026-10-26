using Godot;
using System;

public partial class PCState : Node3D
{

	[Export] public string animationName;
	[Export] protected PCStateMachine pc;
	float defaultGravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	public virtual PCState Enter()
	{
		return null;
		//do animation here
	}

	public virtual PCState Exit()
	{
		return null;
	}

	public virtual void ExitAnimation() {
		pc.anim.Set($"parameters/conditions/{animationName}", false);
	}

	public virtual void EntryAnimation() {
		pc.anim.Set($"parameters/conditions/{animationName}", true);

	}

	public virtual PCState ManageInput(InputEvent @event) {
		return null;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pc = GetParent().GetParent<PCStateMachine>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public virtual PCState Process(double delta)
	{

		return null;
	}

	public virtual PCState PhysicsProcess(double delta) {
		return null;
	}

	// Expected to be called every frame
	public virtual void _ApplyGravity(double delta) { 
		var grav = -pc.GlobalBasis.Y*defaultGravity;
		pc.Velocity += grav;
	}

	public virtual void _Move(Vector2 forw,float moveSpeed, double delta) {
		var force = forw.Normalized() * moveSpeed;
		var addedForce = pc.GlobalBasis.X * force.X + pc.GlobalBasis.Z * force.Y;// new Vector3(force.X, 0, force.Y);
																				 //addedForce = addedForce.Rotated(new Vector3(0, 1, 0), pc.camPoint.GlobalRotation.Y);
	//	var dot = addedForce.Normalized().Dot((pc.Velocity).Normalized());
		//if (dot > 0) {
		//	var lerpVel = (pc.Velocity).Lerp(addedForce, (float)delta*40);
		//	pc.Velocity = (new Vector3(lerpVel.X, lerpVel.Y, lerpVel.Z));

		//}
		//if (true) { }
		//else
		//{
			pc.Velocity = (new Vector3(addedForce.X, addedForce.Y, addedForce.Z));

		//}

		//anim.Run();
		//storedHForc 
	}

	public string GetAnimationName() { return animationName; }

	public void SetAnimationTree(AnimationTree tree) {
		pc.anim = tree;
	}

	public void SetAudioSource(AudioStreamPlayer3D _aud) {
		pc.aud = _aud;
	}


	public void OrientAgainstGravity()
	{
		var cros = -(pc.currentGravityVector).Cross(Basis.Y);
		var upward = -Mathf.Sign(pc.currentGravityVector.Y);
		if (upward >= 0)
		{
			pc.Rotation = new Vector3(-cros.X * MathF.PI / 2f, 0, -cros.Z * MathF.PI / 2f);
		}
		else
		{
			pc.Rotation = new Vector3(-cros.X * MathF.PI / 2f, 0, Mathf.Pi +cros.Z * MathF.PI / 2f);

		}
		GD.Print(cros);
	}

}
