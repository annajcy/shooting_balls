using System;
using UnityEngine;

namespace Framework.Timer
{
    public class IntervalTimerCallbackHandle
    {
        public int id;
        public int totalTimeElapsed;
        public int intervalTimeElapsed;

        public IntervalTimerCallbackHandle(int id, int totalTimeElapsed, int intervalTimeElapsed)
        {
            this.id = id;
            this.totalTimeElapsed = totalTimeElapsed;
            this.intervalTimeElapsed = intervalTimeElapsed;
        }
    }

    public class TimerEvents
    {
        public Action<int> startCallback;
        public Action<int> finishCallback;
        public Action<IntervalTimerCallbackHandle> intervalCallback;


        public TimerEvents() { }

        public TimerEvents(Action<int> startCallback, Action<int> finishCallback,
            Action<IntervalTimerCallbackHandle> intervalCallback)
        {
            this.startCallback = startCallback;
            this.finishCallback = finishCallback;
            this.intervalCallback = intervalCallback;
        }

        public void Clear()
        {
            startCallback = null;
            intervalCallback = null;
            finishCallback = null;
        }
    }

    public class TimerItem
    {
        public int id;
        public TimerEvents events;

        public int totalTime;
        public int originalTotalTime;

        public int intervalTime;
        public int originalIntervalTime;

        public bool isRunning;
        public bool isTicked;

        public TimerType timerType;

        public int TotalTimeElapsed => this.originalTotalTime - this.totalTime;
        public int IntervalTimeElapsed => this.originalIntervalTime - this.intervalTime;

        public TimerItem() {}

        public TimerItem(int id, int totalTime, int intervalTime, TimerEvents events, TimerType timerType)
        {
            this.id = id;
            this.originalTotalTime = totalTime;
            this.totalTime = totalTime;
            this.originalIntervalTime = intervalTime;
            this.intervalTime = intervalTime;
            this.events = events;
            this.isRunning = false;
            this.isTicked = false;
            this.timerType = timerType;
        }

        public void Reset()
        {
            this.totalTime = this.originalTotalTime;
            this.intervalTime = this.originalIntervalTime;
            this.isRunning = false;
            this.isTicked = false;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Start()
        {
            isRunning = true;
        }

        public void Tick(int deltaTime)
        {
            if (!isRunning) return;
            if (!isTicked)
            {
                events.startCallback?.Invoke(id);
                isTicked = true;
            }

            this.intervalTime -= deltaTime;
            if (this.intervalTime <= 0)
            {
                events.intervalCallback?.Invoke(new IntervalTimerCallbackHandle(id,
                    TotalTimeElapsed, IntervalTimeElapsed));
                intervalTime = originalIntervalTime;
            }

            this.totalTime -= deltaTime;
            if (this.totalTime <= 0) events.finishCallback?.Invoke(id);

        }

    }
}