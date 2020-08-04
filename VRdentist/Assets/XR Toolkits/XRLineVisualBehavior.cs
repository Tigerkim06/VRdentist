using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRInteractorLineVisual))]
public class XRLineVisualBehavior : MonoBehaviour
{
    [SerializeField]
    private XRBaseInteractor[] xrInteractors;
    private XRInteractorLineVisual xrLineVisual;
    
    void Awake()
    {
        xrLineVisual = this.GetComponent<XRInteractorLineVisual>();
        Debug.Log("XRLineVisualBehavior Awake");
    }

    void OnGrab(XRBaseInteractable baseInteractable)
    {
       if(xrLineVisual) xrLineVisual.enabled = false;
    }

    void OnRelease(XRBaseInteractable baseInteractable)
    {
        if(xrLineVisual) xrLineVisual.enabled = true;
    }

    private void OnEnable()
    {
        foreach (XRBaseInteractor interactor in xrInteractors)
        {
            if (interactor)
            {
                interactor.onSelectEnter.AddListener(OnGrab);
                interactor.onSelectExit.AddListener(OnRelease);
            }
        }
        Debug.Log("XRLineVisualBehavior OnEnable");
    }

    private void OnDisable()
    {
        foreach (XRBaseInteractor interactor in xrInteractors)
        {
            if (interactor)
            {
                interactor.onSelectEnter.RemoveListener(OnGrab);
                interactor.onSelectExit.RemoveListener(OnRelease);
            }
        }
        Debug.Log("XRLineVisualBehavior OnDisable");
    }

    private void OnDestroy()
    {
        foreach (XRBaseInteractor interactor in xrInteractors)
        {
            if (interactor)
            {
                interactor.onSelectEnter.RemoveListener(OnGrab);
                interactor.onSelectExit.RemoveListener(OnRelease);
            }
        }
        Debug.Log("XRLineVisualBehavior OnDestroy");
    }
}
