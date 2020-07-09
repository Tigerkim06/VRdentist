using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SequenceEvents", menuName = "SceneEvent/SequenceEvents", order = 1)]
public class SequenceEvents : SceneEvent
{
    public SceneSubEvent[] subEvents;

    private int eventIter;
    private bool callNextProc;
    private float delayProc;
    private bool skip;

    public SceneEvent nextEvent;

    public override void InitEvent()
    {
        base.InitEvent();
        foreach (SceneSubEvent sceneEvent in subEvents)
        {
            sceneEvent.InitEvent();
        }
        eventIter = 0;
        callNextProc = false;
        skip = false;
    }

    public override void StartEvent()
    {
        InitEvent();
        callNextProc = true;
    }
    
    public override void UpdateEvent()
    {
        if (delayProc > 0)
        {
            delayProc -= Time.deltaTime;
            return;
        }
        if (eventIter >= subEvents.Length)
        {
            passEventCondition = true;
            return;
        }
        if (callNextProc)
        {
            callNextProc = false;
            subEvents[eventIter].StartEvent();
        }
        else
        {
            subEvents[eventIter].UpdateEvent();
            if (subEvents[eventIter].CheckPassEventCondition())
            {
                subEvents[eventIter].StopEvent();
                delayProc = subEvents[eventIter].GetDelayNextEvent();
                eventIter++;
                callNextProc = true;
            }
        }
        if (skip && !callNextProc)
        {
            subEvents[eventIter].Skip();
        }
    }


    public override void StopEvent() {  }

    public override void Skip()
    {
        skip = true;
    }

    public override void Pause() { }
    public override void UnPause() { }

    public override SceneEvent NextEvent()
    {
        return nextEvent;
    }
}
