using System;

using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public GameObject content;
    public bool hasDelay = false;
    public float delay = 0.5f;

    private CancellationTokenSource cancellationTokenSource;

    private void OnEnable()
    {
        Disable();
    }

    private void OnMouseEnter()
    {
        Enable();
    }

    private void OnMouseDown()
    {
        Disable();
    }

    private void OnMouseExit()
    {
        Disable();
    }

    void Enable()
    {
        if (hasDelay)
        {
            cancellationTokenSource = new CancellationTokenSource();

            Task.Delay(TimeSpan.FromSeconds(delay), cancellationTokenSource.Token)
                .ContinueWith(task =>
                {
                    if (!task.IsCanceled)
                    {
                        content.SetActive(true);
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        else
        {
            content.SetActive(true);
        }
    }

    void Disable()
    {
        if (hasDelay)
            CancelDelayedTask();
        content.SetActive(false);
    }

    public void CancelDelayedTask()
    {
        if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
    }
}