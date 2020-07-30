using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CollideToNextEvent", menuName = "SceneEvent/Custom/CollideToNextEvent")]
public class CollideToNextEvent : SceneEvent
{
    public string targetObjectName;
    public string firstObjectName;
    public string secondObjectName;

    private int hitTarget;

    private Collider targetObjectCollider;
    private Collider firstObjectCollider;
    private Collider secondObjectCollider;

    public SceneEvent collideFirstNextScene;
    public SceneEvent collideSecondNextScene;

    public override void InitEvent()
    {
        base.InitEvent();
        bool foundTargetObject = SceneAssetManager.GetAssetComponent<Collider>(targetObjectName, out targetObjectCollider);
        bool foundFirstObject = SceneAssetManager.GetAssetComponent<Collider>(firstObjectName, out firstObjectCollider);
        bool foundSecondObject = SceneAssetManager.GetAssetComponent<Collider>(secondObjectName, out secondObjectCollider);
        Debug.Log("Found Asset[" + targetObjectName + "]: " + foundTargetObject);
        Debug.Log("Found Asset[" + firstObjectName + "]: " + foundFirstObject);
        Debug.Log("Found Asset[" + secondObjectName + "]: " + foundSecondObject);
        if (targetObjectCollider != null && firstObjectCollider != null && secondObjectCollider != null)
        {
            targetObjectCollider.enabled = false;
            firstObjectCollider.enabled = false;
            secondObjectCollider.enabled = false;
        }
        
        hitTarget = 0;

        if (collideFirstNextScene) collideFirstNextScene.InitEvent();
        if (collideSecondNextScene) collideSecondNextScene.InitEvent();
    }
    
    public override void StartEvent()
    {
        if (targetObjectCollider != null && firstObjectCollider != null && secondObjectCollider != null)
        {
            targetObjectCollider.enabled = true;
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
        if (targetObjectCollider.bounds.Intersects(firstObjectCollider.bounds)) {
            hitTarget = 1;
            passEventCondition = true;  
        }
        else if (targetObjectCollider.bounds.Intersects(secondObjectCollider.bounds))
        {
            hitTarget = 2;
            passEventCondition = true;
        }
    }
    
    public override void StopEvent()
    {
        Destroy(targetObjectCollider.gameObject);
        firstObjectCollider.enabled = false;
        secondObjectCollider.enabled = false;
    }

    public override SceneEvent NextEvent()
    {
        if (hitTarget == 1) return collideFirstNextScene;
        return collideSecondNextScene;
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
