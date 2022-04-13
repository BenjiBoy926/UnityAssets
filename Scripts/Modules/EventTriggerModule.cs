using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class EventTriggerModule
{
    public static void AddTrigger(this EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger.TriggerEvent eventTrigger = new EventTrigger.TriggerEvent();
        eventTrigger.AddListener(callback);
        trigger.triggers.Add(new EventTrigger.Entry() 
        {
            eventID = type,
            callback = eventTrigger
        });
    }
}
