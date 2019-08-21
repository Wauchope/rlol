using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerType
{
    NORMAL, FIXED
}

public class Timer
{
    float time;
    TimerType type;

    public Timer(TimerType type)
    {
        this.type = type;
    }

    public void IncrementTimer()
    {
        if (type == TimerType.NORMAL)
        {
            time += Time.deltaTime;
        }
        else if (type == TimerType.FIXED)
        {
            time += Time.fixedDeltaTime;
        }
    }

    public void ResetTimer()
    {
        time = 0f;
    }

    public float GetTime()
    {
        return time;
    }
}
