using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CollideFoDuration", menuName = "SceneEvent/Custom/CollideFoDuration")]
public class CollideForDuration : SceneEvent
{


    public string firstObjectName;
    public string secondObjectName;


    private Collider firstObjectCollider;
    private Collider secondObjectCollider;

    public SceneEvent nextScene;
    float timer = 0;


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
        if (firstObjectCollider.bounds.Intersects(secondObjectCollider.bounds))
        {
            passEventCondition = true;
            timer += 1 * Time.deltaTime;
            Debug.Log(timer);
        }
    }



    public override void StopEvent()
    {
        firstObjectCollider.enabled = false;
        secondObjectCollider.enabled = false;
    }


    public override SceneEvent NextEvent()
    {
        return nextScene;
    }






    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    public override void Skip()
    {
        throw new System.NotImplementedException();
    }



    public override void UnPause()
    {
        throw new System.NotImplementedException();
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
