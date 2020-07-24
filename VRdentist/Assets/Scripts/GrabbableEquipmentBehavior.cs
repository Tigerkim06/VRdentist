using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabbableEquipmentBehavior : MonoBehaviour
{
    private static readonly string p_grabbed = "grabbed";
    private static readonly string p_activate= "activate";

    enum ActivateType { Switch, Hold }

    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ActivateType activateType = ActivateType.Switch;
    [SerializeField]
    private Collider[] activateColliders;
    [SerializeField]
    private bool triggerOnGrabbed = true;

    [SerializeField]
    [ReadOnly]
    private bool isGrabbed;
    [SerializeField]
    [ReadOnly]
    private bool isActivate;

    private void Awake()
    {
        isGrabbed = false;
        isActivate = false;
    }

    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        if (!animator) {
            animator = this.GetComponentInChildren<Animator>();
        }
        grabInteractable.onSelectEnter.AddListener(OnGrabbed);
        grabInteractable.onSelectExit.AddListener(OnReleased);
        grabInteractable.onActivate.AddListener(OnActivate);
        grabInteractable.onDeactivate.AddListener(OnDeactivate);
        SetAnimation();
        SetActivateColliders();
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor)
    {
        isGrabbed = true;
        isActivate = false;
        SetAnimation();
        SetActivateColliders();
    }

    void OnReleased(XRBaseInteractor rBaseInteractor)
    {
        isGrabbed = false;
        isActivate = false;
        SetAnimation();
        SetActivateColliders();
    }

    void OnActivate(XRBaseInteractor rBaseInteractor)
    {
        switch (activateType)
        {
            case ActivateType.Switch:
                isActivate = !isActivate;
                break;
            case ActivateType.Hold:
                isActivate = true;
                break;
        }
        SetAnimation();
    }

    void OnDeactivate(XRBaseInteractor rBaseInteractor)
    {
        switch (activateType)
        {
            case ActivateType.Switch:
                break;
            case ActivateType.Hold:
                isActivate = false;
                break;
        }
        SetAnimation();
    }

    private void SetAnimation() {
        if (animator) {
            animator.SetBool(p_grabbed, isGrabbed);
            animator.SetBool(p_activate, isGrabbed && isActivate);
        }
    }

    private void SetActivateColliders()
    {
        foreach (Collider col in activateColliders) {
            if (col)
            {
                col.isTrigger = isGrabbed && triggerOnGrabbed;
            }
        }
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        grabInteractable.onSelectExit.RemoveListener(OnReleased);
        grabInteractable.onActivate.RemoveListener(OnActivate);
        grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
    }
}
