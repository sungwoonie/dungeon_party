using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Storage;


public class Anti_Cheat_Manager : SingleTon<Anti_Cheat_Manager>
{
    protected override void Awake()
    {
        base.Awake();

        Initialize_Anti_Cheat();
    }

    public void Initialize_Anti_Cheat()
    {
        ObscuredCheatingDetector.StartDetection(Detect_Cheat);

        ObscuredFilePrefs.Init(true);
        ObscuredFilePrefs.NotGenuineDataDetected += Detect_Cheat;
        ObscuredFilePrefs.DataFromAnotherDeviceDetected += Detect_Cheat;
    }

    private void Detect_Cheat()
    {
        Remove_All();
        Application.Quit();
    }

    public T Get<T>(string key, T defaultValue = default)
    {
        return ObscuredFilePrefs.Get(key, defaultValue);
    }

    public void Set<T>(string key, T value)
    {
        ObscuredFilePrefs.Set(key, value);
    }

    public void Remove_All()
    {
        ObscuredFilePrefs.DeleteAll();
    }
}