using Godot;
using System;

public partial class PCIdleState : PCState
{

	[Export] PCState jumpState;
	[Export] PCState walkState;
	[Export] PCState fallState;
	[Export] PCState suckState;

	public override PCState Enter() {
		pc.Velocity = Vector3.Zero;
		return null;
	}

	public override PCState ManageInput(InputEvent @event) {
		if (@event.IsActionPressed("Jump")) {
			if (pc.IsOnFloor())
			{
				return jumpState;

			}
		}
		if(@event.GetActionStrength("MoveLeft")>.1f ||  Input.GetActionStrength("MoveRight") > .1f || Input.GetActionStrength("MoveUp") > .1f || Input.GetActionStrength("MoveDown") > .2f) {
			return walkState;
		}

		
		return null;
	}

	public override PCState PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		_ApplyGravity(delta);
		pc.MoveAndSlide();
		if (pc.IsOnFloor()) {
			OrientAgainstGravity();
			return null;
		}
		else {
			return fallState;

		}

	}
	public override void EntryAnimation() {
		pc.anim.Set("parameters/Ground/GroundBlend/transition_request", animationName);
	}

	public override void ExitAnimation() {
		//base.ExitAnimation();
	}
}
