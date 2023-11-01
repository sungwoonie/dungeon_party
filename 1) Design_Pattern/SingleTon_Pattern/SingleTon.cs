using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour
{
	private static T _instance;

	public static T instance
	{
		get
		{
			return _instance;
		}
	}
	protected virtual void Awake()
	{
		_instance = this.GetComponent<T>();
	}
}