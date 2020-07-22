using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class HideHandAfterGrabbed : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    [SerializeField]
    [ReadOnly]
    private HandPresence handPresence;
    private bool recordShowController;
    private bool recordShowHand;

    // Start is called before the first frame update
    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(OnGrabbed);
        grabInteractable.onSelectExit.AddListener(OnReleased);
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor)
    {
        handPresence = rBaseInteractor.GetComponentInChildren<HandPresence>();
        recordShowController = handPresence.showController;
        recordShowHand = handPresence.showHand;
        handPresence.showController = false;
        handPresence.showHand = false;
    }

    void OnReleased(XRBaseInteractor rBaseInteractor)
    {
        handPresence.showController = recordShowController;
        handPresence.showHand = recordShowHand;
        handPresence = null;
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        grabInteractable.onSelectEnter.RemoveListener(OnReleased);
    }

}
