using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Json_Helper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson_List<T>(List<T> array)
    {
        Wrapper_List<T> wrapper = new Wrapper_List<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

    [System.Serializable]
    private class Wrapper_List<T>
    {
        public List<T> items;
    }
}
