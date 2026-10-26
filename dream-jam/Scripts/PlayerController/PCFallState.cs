using Godot;
using System;

public partial class PCFallState : PCState {
	[Export] PCState idleState;
	[Export] PCState moveState;
	[Export] AudioStream landSfx;
	//[Export] LedgeDetector ledgeDetector;

	public override PCState PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		_ApplyGravity(delta);
		//movement
		Vector2 movement = new Vector2(Input.GetAxis("MoveLeft", "MoveRight"), Input.GetAxis("MoveUp", "MoveDown"));
		if (movement.Length() > .1f) {
			//_Move(movement, delta);

			var force = movement.Normalized();
			var dir = new Vector3(force.X, 0, force.Y);
			dir = dir.Rotated(new Vector3(0, 1, 0), pc.camPoint.GlobalRotation.Y);

			//pc.LookAt(pc.Position - new Vector3(dir.X, 0, dir.Z).Normalized() * 5, Vector3.Up);
			//pc.Rotation = new Vector3(0, pc.Rotation.Y, 0);
		}
		pc.MoveAndSlide();

		if (pc.IsOnFloor()) {
			pc.aud.Stream = landSfx;
			pc.aud.Play();
			if (movement.Length() > 0) {
				return moveState;
			}
			else {
				return idleState;
			}
		}
		return null;
	}

	/*
	public KoboldState LedgeFound(LedgeDetector _detector) {

		var ledgePos = _detector.LedgeFound();
		if (ledgePos != Vector3.Zero) {
			cb.Position = new Vector3(cb.Position.X,ledgePos.Y +.05f,cb.Position.Z);
			var norm = _detector.FindLedgeNormal();
			cb.LookAt(cb.GlobalPosition + norm * 5, Vector3.Up);
			cb.Rotation = new Vector3(0, cb.Rotation.Y, 0);
			return ledgeState;
		}
		return null;
	}
	*/

}
