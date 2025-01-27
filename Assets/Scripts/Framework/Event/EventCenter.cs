using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Event
{
    public interface IEventItem { }

    public class EventItem<T> : IEventItem
    {
        public Action<T> events;
        public EventItem() { events = null; }
        public EventItem(Action<T> action) { events = action; }

    }

    public class EventItem : IEventItem
    {
        public Action events;
        public EventItem() { events = null; }
        public EventItem(Action action) { events = action; }

    }

    public class EventCenter : SingletonBase<EventCenter>
    {
        private Dictionary<EventType, IEventItem> eventDict = new Dictionary<EventType, IEventItem>();
        private EventCenter() { }

        public void EventTrigger<T>(EventType eventType, T param)
        {
            if (eventDict.TryGetValue(eventType, out var eventItem))
            {
                Debug.Log("Event triggered: " + eventType);
                ((EventItem<T>)eventItem).events.Invoke(param);
            }

        }

        public void EventTrigger(EventType eventType)
        {
            if (eventDict.TryGetValue(eventType, out var eventItem))
            {
                Debug.Log("Event triggered: " + eventType);
                ((EventItem)eventItem).events?.Invoke();
            }

        }

        public void AddEventListener<T>(EventType eventType, Action<T> action)
        {
            if (eventDict.TryGetValue(eventType, out var eventItem))
            {
                if (!((EventItem<T>)eventItem).events.GetInvocationList().Contains(action))
                    ((EventItem<T>)eventItem).events += action;
            }
            else eventDict.Add(eventType, new EventItem<T>(action));
        }

        public void AddEventListener(EventType eventType, Action action)
        {
            if (eventDict.TryGetValue(eventType, out var eventItem))
            {
                if (!((EventItem)eventItem).events.GetInvocationList().Contains(action))
                    ((EventItem)eventItem).events += action;
            }
            else eventDict.Add(eventType, new EventItem(action));
        }

        public void RemoveEventListener<T>(EventType eventType, Action<T> action)
        {
            if (eventDict.TryGetValue(eventType, out var eventItem))
            {
                ((EventItem<T>)eventItem).events -= action;
                if (((EventItem<T>)eventItem).events == null)
                    eventDict.Remove(eventType);
            }

        }

        public void RemoveEventListener(EventType eventType, Action action)
        {
            if (eventDict.TryGetValue(eventType, out var eventItem))
            {
                ((EventItem)eventItem).events -= action;
                if (((EventItem)eventItem).events == null)
                    eventDict.Remove(eventType);
            }
        }

        public void Remove(EventType eventType)
        {
            if (eventDict.ContainsKey(eventType))
                eventDict.Remove(eventType);
        }

        public void Clear()
        {
            eventDict.Clear();
        }
    }


}