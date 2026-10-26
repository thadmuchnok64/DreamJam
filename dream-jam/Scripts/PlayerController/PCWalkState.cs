using Godot;
using System;

public partial class PCWalkState : PCState
{

	[Export] PCState idleState;
	[Export] PCState jumpState;
	[Export] PCState fallState;
	[Export] PCState sprintState;
	[Export] PCState suckState;



	[Export] float minSpeed = .05f;
	[Export] float maxSpeed = 5f;

	[Export] float dragForce;

	public override PCState ManageInput(InputEvent @event) {
		if (@event.IsActionPressed("Jump")) {
			if (pc.IsOnFloor())
				return jumpState;
		}
		return null;
	}
	public override PCState PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (pc.IsOnFloor()){
			OrientAgainstGravity();


		}
		if (pc.Velocity.Y < 0.2f && !pc.IsOnFloor()) {
			return fallState;
		}
		//GD.Print(pc.Velocity);

		//movement
		Vector2 movement = new Vector2(Input.GetAxis("MoveLeft","MoveRight"), Input.GetAxis("MoveUp","MoveDown"));
		if (movement.Length() > .1f) {
			_Move(movement,maxSpeed, delta);
		}
		else {
			return idleState;
			//_SlowGroundMovement(delta);
		}

		//Gravity
		_ApplyGravity(delta);

		pc.MoveAndSlide();


		var hVel = new Vector3(pc.Velocity.X, 0, pc.Velocity.Z);
		if (hVel.Length() > .2f)
		{
			//pc.LookAt(pc.Position - hVel.Normalized() * 5, Vector3.Up);
			//pc.Rotation = new Vector3(0, pc.Rotation.Y, 0);
		}
		if (pc.IsOnFloor()) {
			//if(Input.GetActionStrength("Sprint") > .1) {
			//	return sprintState;
			//}
			if (pc.Velocity.Length() > 0.05f) {
				return null;
			}
			else {
				return idleState;
			}
		}

		return null;

	}

	public override PCState Process(double delta) {

		var timeScale = Mathf.Clamp((pc.Velocity.Length() - minSpeed)/(maxSpeed - minSpeed),0,1);
		pc.anim.Set("parameters/Ground/RunTimeScale/scale", timeScale);

		return null;
	}

	private void _SlowGroundMovement(double delta) {
		var newLen = (pc.Velocity.Length() - dragForce * (float)delta);
		if (newLen <= 0) {
			pc.Velocity = new Vector3(0, pc.Velocity.Y,0);
			return;
		}
		pc.Velocity = pc.Velocity.Normalized() * newLen;
	}

	public override void EntryAnimation() {
		pc.anim.Set("parameters/Ground/GroundBlend/transition_request",animationName);
	}

	public override void ExitAnimation() {
		//base.ExitAnimation();
	}

}
