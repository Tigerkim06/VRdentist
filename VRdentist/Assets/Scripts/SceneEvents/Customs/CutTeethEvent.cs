using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CutTeethEvent", menuName = "SceneEvent/TeethRemoval/CutTeethEvent")]
public class CutTeethEvent : SceneEvent
{
    public string wisdomTeethName;
    public string[] fragmentTeethNames;
    public string toolName;
    public float actionTime;
    public SceneEvent nextScene;

    private CollisionTrigger wisdomTeethTrigger;
    private List<GameObject> fragmentTooth;
    private GrabbableEquipmentBehavior tool;

    private bool isCollided;
    private float progressTime;

    public override void InitEvent()
    {
        base.InitEvent();
        bool foundTrigger = SceneAssetManager.GetAssetComponent<CollisionTrigger>(wisdomTeethName, out wisdomTeethTrigger);
        bool foundItem = SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(toolName, out tool);

        fragmentTooth = new List<GameObject>();
        foreach (string targetName in fragmentTeethNames) {
            if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject targetObject)) {
                targetObject.SetActive(false); // hide at begin
                fragmentTooth.Add(targetObject);
            }
        }

        Debug.Log("Found Asset[" + wisdomTeethName + "]: " + foundTrigger);
        Debug.Log("Found Asset[fragment Teeth]: " + (fragmentTooth.Count>0));
        Debug.Log("Found Asset[" + toolName + "]: " + foundItem);

        if (wisdomTeethTrigger) wisdomTeethTrigger.gameObject.SetActive(true);

        if (nextScene) nextScene.InitEvent();
    }

    public override void StartEvent()
    {
        isCollided = false;
        progressTime = 0;
        if (wisdomTeethTrigger)
        {
            Debug.Log("CollisionTriggerEvent assign events");
            wisdomTeethTrigger.OnCollisionEnterEvent += OnCollisionEnter;
            wisdomTeethTrigger.OnCollisionExitEvent += OnCollisionExit;
        }
    }

    public override void UpdateEvent()
    {
        if (tool && tool.IsActivate && isCollided)
        {
            progressTime += Time.deltaTime;
        }
        foreach (GameObject fragment in fragmentTooth) {
            fragment.SetActive(true);

        }
        passEventCondition = progressTime >= actionTime;
    }

    public override void StopEvent()
    {
        if (wisdomTeethTrigger)
        {
            Debug.Log("CollisionTriggerEvent remove events");
            wisdomTeethTrigger.OnCollisionEnterEvent -= OnCollisionEnter;
            wisdomTeethTrigger.OnCollisionExitEvent -= OnCollisionExit;
            wisdomTeethTrigger.gameObject.SetActive(false);
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
        if (collision.gameObject == tool.gameObject)
        {
            if (isCollided == false) Debug.Log(tool.name + "is Collided");
            isCollided = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == tool.gameObject)
        {
            isCollided = false;
        }
    }

    public override void OnDestroy()
    {
        if (wisdomTeethTrigger)
        {
            wisdomTeethTrigger.OnCollisionEnterEvent -= OnCollisionEnter;
            wisdomTeethTrigger.OnCollisionExitEvent -= OnCollisionExit;
        }
    }
}
