using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    public List<Timer> timers;

    public List<Timer> timersToAdd;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        timers = new List<Timer>();
        timersToAdd = new List<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
    }

    void UpdateTimers()
    {
        // update each timer
        foreach(Timer t in timers)
        {
            t.Update();
        }

        // remove timers that already fired
        for(int i = 0; i < timers.Count; i++)
        {
            if (timers[i].complete)
                timers.RemoveAt(i);
        }

        // add new timers
        foreach(Timer t in timersToAdd)
        {
            timers.Add(t);
        }

        timersToAdd.Clear();
    }

    public void AddTimer(TimerEvent e, float d)
    {
        timersToAdd.Add(new Timer(d, e));
    }

    public void AddTimer(TimerEvent e, float d, string n)
    {
        timersToAdd.Add(new Timer(d, e, n));
    }

    public void AddTimer(TimerEvent e, float d, string n, bool useSpeed)
    {
        timersToAdd.Add(new Timer(d, e, n, useSpeed));
    }
}

public delegate void TimerEvent();

public class Timer
{
    public float duration = 0;
    public float elapsed = 0;
    public TimerEvent onTimer;
    public bool complete = false;
    public string timerName = "";
    public bool affectedBySpeed = true;

    public Timer(float d, TimerEvent e)
    {
        duration = d;
        onTimer = e;
    }

    public Timer(float d, TimerEvent e, string n)
    {
        duration = d;
        onTimer = e;
        timerName = n;
    }

    public Timer(float d, TimerEvent e, string n, bool useSpeed)
    {
        duration = d;
        onTimer = e;
        timerName = n;
        affectedBySpeed = useSpeed;
    }

    public void Update()
    {
        if (complete)
            return;

        elapsed += Time.deltaTime * (affectedBySpeed ? GameManager.instance.speed : 1);

        if(elapsed > duration)
        {
            onTimer();
            complete = true;
        }
    }

    public void PrintProgress()
    {
        Debug.Log("Timer <" + timerName + ">: " + elapsed + " / " + duration);
    }
}