using Godot;
using System;

public partial class PCStateMachine : CharacterBody3D
{
	[Export] PCState startingState;
	[Export] public AnimationTree anim;
	[Export] public Node3D camPoint;
	[Export] public AudioStreamPlayer3D aud;
	[Export] bool debugState = false;
	[Export] public Vector3 currentGravityVector = Vector3.Down;
	[Export] RayCast3D floorRay;
	PCState currentState;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ChangeState(startingState);

	}

	public void ChangeState(PCState state) {
		if (currentState != null) {
			currentState.Exit();
			currentState.ExitAnimation();
		}
		currentState = state;
		currentState.Enter();
		currentState.EntryAnimation();
		if(debugState)
		GD.Print(currentState.Name);
	}

	public override void _PhysicsProcess(double delta) {
		if (TouchingFloor())
		{
			CalibrateGravityVector();
		}
		else {
		}
		var newState = currentState.PhysicsProcess(delta);
		if (newState != null) {
			ChangeState(newState);
		}
		
	}

	public void CalibrateGravityVector()
	{
		if (floorRay.IsColliding())
		{
			//currentGravityVector = -floorRay.GetCollisionNormal();
			var rawGravityDir = -floorRay.GetCollisionNormal();
			currentGravityVector = new Vector3(Mathf.Pow(rawGravityDir.X, 2)*Mathf.Sign(rawGravityDir.X), Mathf.Pow(rawGravityDir.Y, 2) * Mathf.Sign(rawGravityDir.Y), Mathf.Pow(rawGravityDir.Z, 2) * Mathf.Sign(rawGravityDir.Z));
			//currentGravityVector = new Vector3(Mathf.Pow(rawGravityDir.X, 3), Mathf.Pow(rawGravityDir.Y, 3), Mathf.Pow(rawGravityDir.Z, 3));
			UpDirection = -currentGravityVector;
		}
	}

	public bool TouchingFloor() // Use this in place of cb.TouchingFloor
	{
		return floorRay.IsColliding();
	}
	
	public override void _Input(InputEvent @event) {
		var newState = currentState.ManageInput(@event);
		if (newState != null) {
			ChangeState(newState);
		}
		}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var newState = currentState.Process(delta);
		if (newState != null) {
			ChangeState(newState);
		}

	}

	public string getAnimationName(){
		return currentState.animationName;
	}

}
