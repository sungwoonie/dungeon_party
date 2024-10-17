using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Scene_Manager
{
    public static IEnumerator Load_Scene(string scene_name)
    {
        AsyncOperation operaion = SceneManager.LoadSceneAsync(scene_name);
        while (!operaion.isDone)
        {
            yield return null;
        }
    }
}
