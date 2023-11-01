using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Context {

	public IState Current_State
	{ 
		get; set;
	}

	private readonly Transform controller;

	public State_Context(Transform _controller)
	{
		controller = _controller;
	}

	public void Transition()
	{
		Current_State.Handle (controller);
	}

	public void Transition(IState state)
	{
		Current_State = state;
		Current_State.Handle (controller);
	}
}
