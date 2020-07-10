using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SetActiveEvent", menuName = "SceneEvent/SetActiveEvent", order = 1)]
public class SetActiveEvent : SceneEvent
{
    public enum ActiveType { ToActive, ToDeactive }
    public string assetName;
    public ActiveType type;
    private SceneAsset target;
    public SceneEvent nextScene;

    public override void InitEvent()
    {
        base.InitEvent();
        bool found = SceneAssetManager.GetAsset(assetName, out target);
        Debug.Log("Found Asset[" + assetName + "]: " + found);
       if(nextScene) nextScene.InitEvent();
    }

    public override void StartEvent()
    {
        InitEvent();
        if (target != null) {
            switch (type)
            {
                case ActiveType.ToActive:
                    target.gameObject.SetActive(true);
                    break;
                case ActiveType.ToDeactive:
                    target.gameObject.SetActive(false);
                    break;
            }
            passEventCondition = true;
        }
    }

    public override void UpdateEvent() { }
    public override void StopEvent() { }
    public override void Pause() { }
    public override void UnPause() { }
    public override void Skip() { }

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

}
