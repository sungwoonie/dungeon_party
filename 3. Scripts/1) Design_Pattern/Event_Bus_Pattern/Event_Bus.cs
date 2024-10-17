using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event_Bus
{
	private static Game_State current_state;
	private static readonly IDictionary<Game_State, UnityEvent> events = new Dictionary<Game_State, UnityEvent>();

	public static void Subscribe_Event(Game_State event_type, UnityAction listener)
	{
		UnityEvent _event;

		if (events.TryGetValue(event_type, out _event))
		{
			_event.AddListener(listener);
		}
		else
		{
			_event = new UnityEvent();
			_event.AddListener(listener);
			events.Add(event_type, _event);
		}
	}

	public static void UnSubscribe_Event(Game_State event_type, UnityAction listener)
	{
		UnityEvent _event;

		if (events.TryGetValue(event_type, out _event))
		{
			_event.RemoveListener(listener);
		}
	}

	public static void Publish(Game_State event_type)
	{
		UnityEvent _event;

        current_state = event_type;

        if (events.TryGetValue(event_type, out _event))
		{
			_event.Invoke();
		}

		Debug_Manager.Debug_In_Game_Message($"{event_type} published");
	}

	public static Game_State Get_Current_State()
	{
		return current_state;
	}
}