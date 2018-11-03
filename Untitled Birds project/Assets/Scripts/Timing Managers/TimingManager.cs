using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains the logic for scheduling multiple events to happen on a timer. 
public abstract class TimingManager : MonoBehaviour
{
    public delegate void EventFunction();

    List<TimedEvent> eventQueue;
    float timeElapsed;

    // Use this for initialization
    void Start ()
    {
        Initialize();
    }

    // Update the queue so that the runtime represents how soon after t=0 the event activates, not how soon after the last event.
    // This is a requisite for the queue to work correctly in Update().
    public void BuildQueue()
    {
        float totalTime = 0;
        for (int i = 0; i < eventQueue.Count; i++)
        {
            totalTime += eventQueue[i].runTime;
            eventQueue[i].runTime = totalTime;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeElapsed += Time.deltaTime;

        while (eventQueue.Count > 0 && timeElapsed > eventQueue[0].runTime)
        {
            Debug.Log("Fired " + eventQueue[0].eventFunction.Method.Name + " at " + timeElapsed);
            eventQueue[0].eventFunction();
            eventQueue.RemoveAt(0);
            
        }
	}

    public void Initialize()
    {
        eventQueue = new List<TimedEvent>();
        timeElapsed = 0;
    }

    public void PrintElapsedTime()
    {
        Debug.Log("Time: " + timeElapsed);
    }

    // Binary search addition of new event to queue
    public void AddEvent(EventFunction eventFunction, float runTime)
    {
        TimedEvent newEvent = new TimedEvent(eventFunction, runTime);

        //if (eventQueue.Count == 0)
        //    eventQueue.Add(newEvent);
        //else if (runTime < eventQueue[0].runTime)
        //    eventQueue.Insert(0, newEvent);
        //else if (runTime > eventQueue[eventQueue.Count - 1].runTime)
        //    eventQueue.Add(newEvent);
        //else
        //{
        //    int low = 0, high = eventQueue.Count - 1;
        //    while (low <= high)
        //    {
        //        int mid = (low + high) / 2;
        //        if (eventQueue[low].runTime > runTime)
        //            high = mid - 1;
        //        else
        //            low = mid + 1;
        //    }

        //    eventQueue.Insert(low, newEvent);
        //}

        eventQueue.Add(newEvent);
    }

    public class TimedEvent
    {
        public EventFunction eventFunction;
        public float runTime;   // Time between previous event and this one

        public TimedEvent(EventFunction eventFunction, float runTime)
        {
            this.eventFunction = eventFunction;
            this.runTime = runTime;
        }
    }
}
