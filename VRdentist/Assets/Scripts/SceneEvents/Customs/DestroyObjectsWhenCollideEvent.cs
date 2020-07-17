using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DestroyObjectsWhenCollideEvent", menuName = "SceneEvent/Custom/DestroyObjectsWhenCollideEvent")]
public class DestroyObjectsWhenCollideEvent : SceneEvent
{
    public string firstObjectName;
    public string secondObjectName;

    private Collider firstObjectCollider;
    private Collider secondObjectCollider;

    public SceneEvent nextScene;
    
    public override void InitEvent()
    {
        base.InitEvent();
        bool foundFirstObject = SceneAssetManager.GetAssetComponent<Collider>(firstObjectName, out firstObjectCollider);
        bool foundSecondObject = SceneAssetManager.GetAssetComponent<Collider>(secondObjectName, out secondObjectCollider);
        Debug.Log("Found Asset[" + firstObjectName + "]: " + foundFirstObject);
        Debug.Log("Found Asset[" + secondObjectName + "]: " + foundSecondObject);
        if (firstObjectCollider != null && secondObjectCollider != null)
        {
            firstObjectCollider.enabled = false;
            secondObjectCollider.enabled = false;
        }
        if (nextScene) nextScene.InitEvent();
    }
    
    public override void StartEvent()
    {
        if (firstObjectCollider != null && secondObjectCollider != null)
        {
            firstObjectCollider.enabled = true;
            secondObjectCollider.enabled = true;
        }
        else
        {
            passEventCondition = true;
        }
    }
    
    public override void UpdateEvent()
    {
        if (firstObjectCollider.bounds.Intersects(secondObjectCollider.bounds)) {
            passEventCondition = true;  
        }
    }
    
    public override void StopEvent()
    {
        Debug.Log("Destroy Objects");
      //  Destroy(firstObjectCollider.gameObject);
        Destroy(secondObjectCollider.gameObject);
    }

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    public override void Pause()
    {

    }

    public override void Skip()
    {

    }

    public override void UnPause()
    {

    }
}
