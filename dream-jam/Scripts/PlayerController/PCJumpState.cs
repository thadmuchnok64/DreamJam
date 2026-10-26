using Godot;
using System;

public partial class PCJumpState : PCState
{

	[Export] PCState idleState;
	[Export] PCState walkState;
	[Export] PCState fallState;
	[Export] PCState ledgeState;
	[Export] PCState ledgeDetector;
	[Export] AudioStream jumpSfx;
	[Export] AudioStream landSfx;

	[Export] float jumpCancelGrav;


	[Export] float jumpForce;

	bool cancelling = false;
	// Called when the node enters the scene tree for the first time.
	public override PCState Enter() {
		pc.Velocity = pc.Velocity += pc.Basis.Y * jumpForce;
		pc.MoveAndSlide();
		pc.aud.Stream = jumpSfx;
		pc.aud.Play();
		cancelling = false;
		return null;
	}

	public override PCState ManageInput(InputEvent @event) {
		if (@event.IsActionReleased("Jump")) {
			cancelling = true;
		}
		return null;
	}

	public override void _ApplyGravity(double delta) {
		if (cancelling) {
			var grav = pc.currentGravityVector* jumpCancelGrav;
			pc.Velocity += grav;
		}
		else {
			base._ApplyGravity(delta);
		}
	}


	public override PCState PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		_ApplyGravity(delta);

		if(pc.Velocity.Y < 0) {
			return fallState;
		}
		//movement
		Vector2 movement = new Vector2(Input.GetAxis("MoveLeft", "MoveRight"), Input.GetAxis("MoveUp", "MoveDown"));
		if (movement.Length() > .1f) {
			//_Move(movement,delta);
			var force = movement.Normalized();
			var dir = new Vector3(force.X, 0, force.Y);
			dir = dir.Rotated(new Vector3(0, 1, 0), pc.camPoint.Rotation.Y);

			//pc.LookAt(pc.Position - new Vector3(dir.X, 0, dir.Z).Normalized() * 5, Vector3.Up);
			//pc.Rotation = new Vector3(0, pc.Rotation.Y, 0);
		}

		pc.MoveAndSlide();

		if (pc.IsOnFloor()) {

			pc.aud.Stream = landSfx;
			pc.aud.Play();
			if (movement.Length() > 0) {
				return walkState;
			}
			else {
				return idleState;
			}
		}

		//PCState ledge = LedgeFound(ledgeDetector);

		//return ledge; // will be null if not found;
		return null;
	}

	/*

	public PCState LedgeFound(LedgeDetector _detector) {

		var ledgePos = _detector.LedgeFound();
		if (ledgePos != Vector3.Zero) {
			cb.Position = new Vector3(cb.Position.X, ledgePos.Y, cb.Position.Z);
			var norm = _detector.FindLedgeNormal();
			cb.LookAt(cb.GlobalPosition + norm * 5, Vector3.Up);
			cb.Rotation = new Vector3(0, cb.Rotation.Y, 0);
			return ledgeState;
		}
		return null;
	}
	*/
}
