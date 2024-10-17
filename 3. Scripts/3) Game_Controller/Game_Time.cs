using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game_Time
{
    public static float game_time = 1.0f;
    private static float previous_game_time = 1.0f;

    private static readonly ArrayList observers = new ArrayList();

    #region "Observer"

    public static void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    public static void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    public static void Notify_To_Observers()
    {
        foreach (Observer observer in observers)
        {
            observer.Notify();
        }
    }

    #endregion

    public static void Set_Game_Time(float amount)
    {
        previous_game_time = game_time;
        game_time = amount;
        Notify_To_Observers();
    }

    public static float Get_Game_Time()
    { 
        return game_time;
    }

    public static void Reset_Game_Time()
    {
        game_time = previous_game_time;
        Notify_To_Observers();
    }
}