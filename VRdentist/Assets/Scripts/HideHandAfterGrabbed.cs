using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class HideHandAfterGrabbed : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private HandPresence handPresence;
    private bool recordShowController;
    private bool recordShowHand;
    
    private XRInteractorLineVisual lineVisual;
    private bool recordEnableLineVisual;

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
        lineVisual = rBaseInteractor.GetComponentInChildren<XRInteractorLineVisual>();
        if (handPresence) {
            recordShowController = handPresence.showController;
            recordShowHand = handPresence.showHand;
            handPresence.showController = false;
            handPresence.showHand = false;
        }
        if (lineVisual) {
            recordEnableLineVisual = lineVisual.enabled;
            lineVisual.enabled = false;
        }
    }

    void OnReleased(XRBaseInteractor rBaseInteractor)
    {
        if (handPresence)
        {
            handPresence.showController = recordShowController;
            handPresence.showHand = recordShowHand;
        }
        if (lineVisual)
        {
            lineVisual.enabled = recordEnableLineVisual;
        }
        handPresence = null;
        lineVisual = null;
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        grabInteractable.onSelectExit.RemoveListener(OnReleased);
    }

}
