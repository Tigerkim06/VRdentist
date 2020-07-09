using UnityEngine;

public class SceneEventController : Singleton<SceneEventController>
{
    public SceneEvent startEvent;

    private SceneEvent currentEvent;
    public bool toStartEvent;
    private float delayProc;
    public bool skip;
    //public static readonly OVRInput.RawButton SKIP_BUTTON = OVRInput.RawButton.Start;


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        delayProc = 0;
        skip = false;
        toStartEvent = true;
        currentEvent = startEvent;
        if (currentEvent != null) currentEvent.InitEvent();
    }

    void Update()
    {
        if (currentEvent == null) return;

        //if (OVRInput.GetUp(SKIP_BUTTON)) skip = true;

        if (delayProc > 0)
        {
            delayProc -= Time.deltaTime;
            return;
        }

        if (toStartEvent)
        {
            toStartEvent = false;
            skip = false;
            currentEvent.StartEvent();
        }
        else
        {
            currentEvent.UpdateEvent();
            if (currentEvent.CheckPassEventCondition())
            {
                currentEvent.StopEvent();
                delayProc = currentEvent.GetDelayNextEvent();
                currentEvent = currentEvent.NextEvent();
                toStartEvent = true;
            }
        }

        if (skip)
        {
            currentEvent.Skip();
            skip = false;
        }
    }
}