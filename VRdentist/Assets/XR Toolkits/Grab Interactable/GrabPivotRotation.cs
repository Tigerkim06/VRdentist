using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabPivotRotation : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Quaternion oldPivot;
    [SerializeField][ReadOnly]
    private HandPresence handPresence;
    public float rotateAngle = 10f;


    // Start is called before the first frame update
    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(OnGrabbed);
        grabInteractable.onSelectExit.AddListener(OnReleased);
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor) {
        handPresence = rBaseInteractor.GetComponentInChildren<HandPresence>();
        oldPivot = grabInteractable.attachTransform.localRotation;
    }

    void OnReleased(XRBaseInteractor rBaseInteractor) {
        handPresence = null;
        grabInteractable.attachTransform.localRotation = oldPivot;
    }

    // Update is called once per frame
    void Update()
    {
        if (handPresence && handPresence.GetPrimary2DAxis()!=Vector2.zero) {
            Vector2 input = handPresence.GetPrimary2DAxis();
            grabInteractable.attachTransform.RotateAround(grabInteractable.attachTransform.position, input, rotateAngle);
        }
    }
}
