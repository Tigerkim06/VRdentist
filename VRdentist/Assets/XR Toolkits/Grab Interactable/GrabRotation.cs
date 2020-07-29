﻿using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabRotation : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private HandPresence handPresence;
    private Transform attachTransform;
    private Quaternion mLocalRot_attachTransform;
    public float rotSpeed = 60f;

    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(OnGrabbed);
        grabInteractable.onSelectExit.AddListener(OnReleased);
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor)
    {
        attachTransform = rBaseInteractor.attachTransform;
        mLocalRot_attachTransform = attachTransform.localRotation;
        handPresence = rBaseInteractor.GetComponentInChildren<HandPresence>();
    }

    void OnReleased(XRBaseInteractor rBaseInteractor)
    {
        if (attachTransform)
        {
            attachTransform.localRotation = mLocalRot_attachTransform;
        }
        attachTransform = null;
        handPresence = null;
    }

    void Update()
    {
        if (handPresence && attachTransform)
        {
            Vector2 input = handPresence.GetPrimary2DAxis();
            if (input != Vector2.zero)
            {
                Vector3 rot = new Vector3(input.y, input.x, 0);
                attachTransform.RotateAround(attachTransform.position, rot, rotSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDestroy()
    {
        if (attachTransform) {
            attachTransform.localRotation = mLocalRot_attachTransform;
        }
        grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        grabInteractable.onSelectExit.RemoveListener(OnReleased);
    }
}