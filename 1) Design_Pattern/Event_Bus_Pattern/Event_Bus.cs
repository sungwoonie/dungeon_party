using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Event_Type
{
    Ready, Combat, Boss_Spawn
}

public class Event_Bus
{
	private static readonly IDictionary<Event_Type, UnityEvent> events = new Dictionary<Event_Type, UnityEvent>();

	public static void Subscribe_Event(Event_Type event_type, UnityAction listener)
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

	public static void UnSubscribe_Event(Event_Type event_type, UnityAction listener)
	{
		UnityEvent _event;

		if (events.TryGetValue(event_type, out _event))
		{
			_event.RemoveListener(listener);
		}
	}

	public static void Publish(Event_Type event_type)
	{
		UnityEvent _event;

		if (events.TryGetValue(event_type, out _event))
		{
			_event.Invoke();
		}
	}
}