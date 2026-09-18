using UnityEngine;
using System;



public class KillCount : MonoBehaviour
{
    private static int killedCount = 0;
    public static event Action<int> CountChanged;

    public static void AddKill()
    {
        killedCount++;
        CountChanged?.Invoke(killedCount);
        Debug.Log("Zombies killed: " + killedCount);
    }

    public static int GetKilledCount()
    {
        return killedCount;
    }

    public static void ResetCount()
    {
        killedCount = 0;
        CountChanged?.Invoke(killedCount);
    }
}