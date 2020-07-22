
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
        if (grabInteractable.attachTransform == null) {
            GameObject createdPivot = new GameObject("Pivot");
            createdPivot.transform.SetParent(grabInteractable.transform);
            createdPivot.transform.localPosition = Vector3.zero;
            createdPivot.transform.localEulerAngles = Vector3.zero;
            grabInteractable.attachTransform = createdPivot.transform;
        }
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor) {
        handPresence = rBaseInteractor.GetComponentInChildren<HandPresence>();
        if(grabInteractable.attachTransform)
            oldPivot = grabInteractable.attachTransform.localRotation;
    }

    void OnReleased(XRBaseInteractor rBaseInteractor) {
        handPresence = null;
        if (grabInteractable.attachTransform)
            grabInteractable.attachTransform.localRotation = oldPivot;
    }

    // Update is called once per frame
    void Update()
    {
        if (handPresence && grabInteractable.attachTransform && handPresence.GetPrimary2DAxis()!=Vector2.zero) {
            Vector2 input = handPresence.GetPrimary2DAxis();
            grabInteractable.attachTransform.RotateAround(grabInteractable.attachTransform.position, input, rotateAngle);
        }
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        grabInteractable.onSelectEnter.RemoveListener(OnReleased);
    }
}
