using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractor))]
public class GrabRotation : MonoBehaviour
{
    XRBaseInteractor xrBaseInteractor;
    HandPresence handPresence;

    Transform grabPoint;
    public float rotSpeed = 30f;

    void Start()
    {
        TryInitialize();
    }

    void Update()
    {
        if (!xrBaseInteractor || !handPresence)
        {
            TryInitialize();
        }
        else
        {
            Vector2 input = handPresence.GetPrimary2DAxis();
            if (grabPoint && input != Vector2.zero) {
                Vector3 rot = new Vector3(-input.y, 0, input.x);
                grabPoint.RotateAround(grabPoint.position, rot, rotSpeed * Time.deltaTime);
            }
        }
    }

    private void TryInitialize()
    {
        if (!xrBaseInteractor) {
            xrBaseInteractor = this.GetComponent<XRBaseInteractor>();
            grabPoint = xrBaseInteractor.attachTransform;
        }
        if (!handPresence) handPresence = this.GetComponentInChildren<HandPresence>();
    }
}
