using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Threading;
using System;

//modified script from https://stackoverflow.com/questions/54148708/programming-an-fps-limiter-for-unity
public class FPSLimiter : MonoBehaviour
{
    private FPSLimiter m_Instance;
    public FPSLimiter Instance { get { return m_Instance; } }

    private double FPSLimit = 60.0;

    private long lastTime = HighResolutionDateTime.UtcNow.Ticks;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
    }
    void Awake()
    {
        m_Instance = this;
    }

    void OnDestroy()
    {
        m_Instance = null;
    }

    void Update()
    {
        if (FPSLimit == 0.0) return;

        lastTime += TimeSpan.FromTicks((long) (TimeSpan.TicksPerSecond * (1.0 / FPSLimit))).Ticks; //should be 16.6667
        Debug.Log(TimeSpan.FromTicks((long) (TimeSpan.TicksPerSecond * (1.0 / FPSLimit))).Ticks);

        var now = HighResolutionDateTime.UtcNow.Ticks;

        if (now >= lastTime)
        {
            lastTime = now;
            return;
        }
        else
        {
            SpinWait.SpinUntil(() => { return (HighResolutionDateTime.UtcNow.Ticks >= lastTime); });
        }
    }
}

public static class HighResolutionDateTime
{
    public static bool IsAvailable { get; private set; }

    [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

    public static DateTime UtcNow
    {
        get
        {
            if (!IsAvailable)
            {
                throw new InvalidOperationException(
                    "High resolution clock isn't available.");
            }

            long filetime;
            GetSystemTimePreciseAsFileTime(out filetime);

            return DateTime.FromFileTimeUtc(filetime);
        }
    }

    static HighResolutionDateTime()
    {
        try
        {
            long filetime;
            GetSystemTimePreciseAsFileTime(out filetime);
            IsAvailable = true;
        }
        catch (EntryPointNotFoundException)
        {
            // Not running Windows 8 or higher.
            IsAvailable = false;
        }
    }
}