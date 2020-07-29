using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CollisionTriggerEvent", menuName = "SceneEvent/TeethRemoval/CollisionTriggerEvent")]
public class CollisionTriggerEvent : SceneEvent
{
    public string collisionTriggerName;
    public string targetItemName;
    public SceneEvent nextScene;

    private CollisionTrigger trigger;
    private GameObject targetItem;

    public override void InitEvent()
    {
        base.InitEvent();
        bool foundTargetObject = SceneAssetManager.GetAssetComponent<CollisionTrigger>(collisionTriggerName, out trigger);
        bool foundFirstObject = SceneAssetManager.GetGameObjectAsset(targetItemName, out targetItem);

    }

    public override void StartEvent()
    {

    }

    public override void UpdateEvent()
    {

    }

    public override void StopEvent()
    {

    }

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    public override void Pause()
    {

    }

    public override void UnPause()
    {

    }

    public override void Skip()
    {

    }
}
