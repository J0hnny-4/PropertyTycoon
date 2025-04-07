using System.Threading.Tasks;
using UnityEngine;

// Extension method to create a Task-compatible delay using Unity's timing
public static class AsyncDelayHelper
{
    public static async Task DelayAsync(int milliseconds)
    {
        var seconds = milliseconds / 1000f;
        var startTime = Time.time;
        while (Time.time - startTime < seconds) { await Task.Yield(); }
    }
}