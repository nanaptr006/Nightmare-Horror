using UnityEngine;



public class KillCount : MonoBehaviour
{
    private static int killedCount = 0;

    public static void AddKill()
    {
        killedCount++;
        Debug.Log("Zombies killed: " + killedCount);
    }

    public static int GetKilledCount()
    {
        return killedCount;
    }

    public static void ResetCount()
    {
        killedCount = 0;
    }
}