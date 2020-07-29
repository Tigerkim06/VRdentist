using UnityEngine;
using UnityEngine.EventSystems;

public class CollisionTrigger : MonoBehaviour
{
    public delegate void CollisionAction(Collision collision);

    public event CollisionAction OnCollisionEnterEvent;
    public event CollisionAction OnCollisionStayEvent;
    public event CollisionAction OnCollisionExitEvent;
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CollisionTrigger call: " + collision.gameObject.name + " / OnCollisionEnterEvent: "+ (OnCollisionEnterEvent!=null));

        OnCollisionEnterEvent?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollisionStayEvent?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollisionExitEvent?.Invoke(collision);
    }

}
