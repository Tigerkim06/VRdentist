using UnityEngine;

[System.Serializable]
public abstract class SceneEvent : ScriptableObject, ISceneEvent,ICustomSceneEventNext
{
    protected bool passEventCondition;
    [SerializeField]
    protected float delayNextEvent;
    
    public virtual void InitEvent() {
        passEventCondition = false;
    }

    public abstract void StartEvent();
    public abstract void UpdateEvent();
    public abstract void StopEvent();

    public abstract void Skip();
    public abstract void Pause();
    public abstract void UnPause();

    public abstract SceneEvent NextEvent();

    public virtual bool CheckPassEventCondition()
    {
        return passEventCondition;
    }
    public virtual float GetDelayNextEvent()
    {
        return delayNextEvent;
    }

    public virtual void InitScene(SceneEvent sceneEvent) {
        if (sceneEvent == null) return;
        if (this.GetInstanceID() == sceneEvent.GetInstanceID()) return;
        sceneEvent.InitEvent();
    }

    public virtual void OnDestroy() { }
}
