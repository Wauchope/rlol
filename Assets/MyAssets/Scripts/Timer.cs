using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NORMAL - Uses Time.DeltaTime
//FIXED - Uses Time.FixedDeltaTime
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

    //Increments once the timer based on the type of timer it is.
    //Must be called in the correct update method to work properly.
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

    //Resets the timer to 0
    public void ResetTimer()
    {
        time = 0f;
    }

    //Returns the current value stored within the timer
    public float GetTime()
    {
        return time;
    }
}
