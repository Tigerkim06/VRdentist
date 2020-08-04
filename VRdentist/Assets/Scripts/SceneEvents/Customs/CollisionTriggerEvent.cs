using com.dgn.SceneEvent;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CollisionTriggerEvent", menuName = "SceneEvent/TeethRemoval/CollisionTriggerEvent")]
public class CollisionTriggerEvent : SceneEvent
{
    public string collisionTriggerName;
    public string targetItemName;
    public SceneEvent nextScene;

    private CollisionTrigger trigger;
    private GrabbableEquipmentBehavior targetItem;

    private bool isCollided;

    public override void InitEvent()
    {
        base.InitEvent();
        bool foundTrigger = SceneAssetManager.GetAssetComponent<CollisionTrigger>(collisionTriggerName, out trigger);
        bool foundItem = SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(targetItemName, out targetItem);

        Debug.Log("Found Asset[" + collisionTriggerName + "]: " + foundTrigger);
        Debug.Log("Found Asset[" + targetItemName + "]: " + foundItem);
        
        if (nextScene) nextScene.InitEvent();
    }

    public override void StartEvent()
    {
        isCollided = false;
        if (trigger)
        {
            trigger.gameObject.SetActive(true);
            Debug.Log("CollisionTriggerEvent assign events");
            trigger.OnCollisionEnterEvent += OnCollisionEnter;
            trigger.OnCollisionExitEvent += OnCollisionExit;
        }
    }

    public override void UpdateEvent()
    {
        if (targetItem && targetItem.IsActivate && isCollided)
        {
            passEventCondition = true;
        }
    }

    public override void StopEvent()
    {
        if (trigger)
        {
            Debug.Log("CollisionTriggerEvent remove events");
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
            trigger.gameObject.SetActive(false);
        }
        Debug.Log("Stop event: " + this.name);
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CollisionTriggerEvent call: " + collision.gameObject.name);
        if (collision.gameObject == targetItem.gameObject) {
            if (isCollided == false) Debug.Log(targetItem.name + "is Collided");
            isCollided = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetItem.gameObject)
        {
            isCollided = false;
        }
    }

    public override void OnDestroy()
    {
        if (trigger)
        {
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
        }
    }
}
