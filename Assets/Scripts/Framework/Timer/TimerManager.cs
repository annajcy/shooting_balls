using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Framework.Mono;
using Framework.Singleton;
using UnityEngine;

public enum TimerType
{
    Regular,
    Realtime
}

namespace Framework.Timer
{
    public class TimerManager : SingletonBase<TimerManager>
    {
        private int currentTimerID = 0;

        private ConcurrentDictionary<int, TimerItem> regularTimerDict = new ConcurrentDictionary<int, TimerItem>();
        private ConcurrentDictionary<int, TimerItem> realtimeTimerDict = new ConcurrentDictionary<int, TimerItem>();

        private List<int> realtimeRemoveList = new List<int>();
        private List<int> regularRemoveList = new List<int>();

        private const float globalIntervalTime = 0.1f;
        private WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(globalIntervalTime);
        private WaitForSeconds waitForSecondsRegular = new WaitForSeconds(globalIntervalTime);

        private TimerManager() { Start(); }

        private void Start()
        {
            MonoManager.Instance.StartCoroutine(StartRegularTiming());
            MonoManager.Instance.StartCoroutine(StartRealtimeTiming());
        }

        IEnumerator StartRegularTiming()
        {
            while (true)
            {
                yield return waitForSecondsRegular;
                foreach (var timerItem in regularTimerDict.Values)
                    timerItem.Tick((int)(globalIntervalTime * 1000));
                RemoveTimersFromList(regularTimerDict, regularRemoveList);
            }
        }

        IEnumerator StartRealtimeTiming()
        {
            while (true)
            {
                yield return waitForSecondsRealtime;
                foreach (var timerItem in realtimeTimerDict.Values)
                    timerItem.Tick((int)(globalIntervalTime * 1000));
                RemoveTimersFromList(realtimeTimerDict, realtimeRemoveList);
            }
        }

        private void RemoveTimersFromList(ConcurrentDictionary<int, TimerItem> timerDict, List<int> removeList)
        {
            foreach (var timerID in removeList)
            {
                if (timerDict.TryRemove(timerID, out var timerItem))
                    timerItem.events.Clear();
            }
            removeList.Clear();
        }

        public int CreateTimer(TimerType timerType, int totalTime, int intervalTime, TimerEvents timerEvents)
        {
            int id = ++currentTimerID;

            TimerItem timerItem = new TimerItem(id, totalTime, intervalTime, timerEvents, timerType);

            if (timerType == TimerType.Regular)
            {
                timerItem.events.finishCallback += AddToRegularRemoveList;
                regularTimerDict.TryAdd(id, timerItem);
            }
            else if (timerType == TimerType.Realtime)
            {
                timerItem.events.finishCallback += AddToRealtimeRemoveList;
                realtimeTimerDict.TryAdd(id, timerItem);
            }

            return id;
        }

        public int CreateBulletTimeTimer(float timeScale, int totalTime, int intervalTime, TimerEvents timerEvents)
        {
            int id = ++currentTimerID;

            TimerItem timerItem = new TimerItem(id, totalTime, intervalTime, timerEvents, TimerType.Realtime);

            timerItem.events.finishCallback += AddToRealtimeRemoveList;
            timerItem.events.startCallback += (_) => { SetTimeScale(timeScale); };
            timerItem.events.finishCallback += (_) => { RecoverTimeScale(); };

            realtimeTimerDict.TryAdd(id, timerItem);
            return id;
        }

        private void AddToRealtimeRemoveList(int timerID)
        {
            if (realtimeTimerDict.ContainsKey(timerID))
                realtimeRemoveList.Add(timerID);
            else Debug.LogError("Invalid timer ID");
        }

        private void AddToRegularRemoveList(int timerID)
        {
            if (regularTimerDict.ContainsKey(timerID))
                regularRemoveList.Add(timerID);
            else Debug.LogError("Invalid timer ID");
        }

        public TimerItem GetTimer(int timerID)
        {
            if (regularTimerDict.TryGetValue(timerID, out var regularTimerItem)) return regularTimerItem;
            if (realtimeTimerDict.TryGetValue(timerID, out var realtimeTimerItem)) return realtimeTimerItem;
            Debug.LogError("Invalid timer ID");
            return null;
        }

        public void AddToRemoveList(int timerID)
        {
            if (regularTimerDict.ContainsKey(timerID)) AddToRegularRemoveList(timerID);
            else if (realtimeTimerDict.ContainsKey(timerID)) AddToRealtimeRemoveList(timerID);
            else Debug.LogError("Invalid timer ID");
        }

        public void StartTimer(int timerID)
        {
            if (regularTimerDict.TryGetValue(timerID, out var regularTimerItem)) regularTimerItem.Start();
            else if (realtimeTimerDict.TryGetValue(timerID, out var realtimeTimerItem)) realtimeTimerItem.Start();
            else Debug.LogError("Invalid timer ID");
        }

        public void StopTimer(int timerID)
        {
            if (regularTimerDict.TryGetValue(timerID, out var regularTimerItem)) regularTimerItem.Stop();
            else if (realtimeTimerDict.TryGetValue(timerID, out var realtimeTimerItem)) realtimeTimerItem.Stop();
            else Debug.LogError("Invalid timer ID");
        }

        public void ResetTimer(int timerID)
        {
            if (regularTimerDict.TryGetValue(timerID, out var regularTimerItem)) regularTimerItem.Reset();
            else if (realtimeTimerDict.TryGetValue(timerID, out var realtimeTimerItem)) realtimeTimerItem.Reset();
            else Debug.LogError("Invalid timer ID");
        }

        private void SetTimeScale(float scale, bool isSetFixedTimeScale = true)
        {
            Time.timeScale *= scale;
            if (isSetFixedTimeScale) Time.fixedDeltaTime = 0.02f * scale;
        }

        private void RecoverTimeScale()
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}