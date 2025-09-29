using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class DelayMaker 
{
    public static async void InvokeAfterDelay(Action action, float delayInSeconds)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
            action?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in InvokeAfterDelay: {ex.Message}");
        }
    }

    public static async Task WaitForSeconds(float seconds)
    {
        int milliseconds = (int)(seconds * 1000);
        await Task.Delay(milliseconds);
    }
    
    //EXECUTION EXAMPLE
    // private async void CreateEarthquake()
    // {
    //     for (int i = 0; i < frequence; i++)
    //     {
    //         CommitEarthquakePhase();
    //         await DelayMaker.WaitForSeconds(waitTime); // Wait asynchronously
    //         //Debug.Log($"Phase {i} completed at {Time.time}");
    //     }
    // }
}
