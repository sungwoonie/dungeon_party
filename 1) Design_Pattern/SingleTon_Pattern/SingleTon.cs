using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour
{
	public bool dont_destroy = true;

	private static T _instance;

	public static T instance
	{
		get
		{
			return _instance;
		}
	}

	protected virtual void OnEnable()
	{
		if (dont_destroy)
		{
			DontDestroyOnLoad(this);
		}
	}

	protected virtual void Awake()
	{
		_instance = this.GetComponent<T>();
	}
}