using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractorBehavior : MonoBehaviour
{
    [Serializable]
    class XRInteractorConfig {
        [SerializeField]
        public XRBaseInteractor interactor;
        private UnityAction<XRBaseInteractable> onSelectAction;
        private UnityAction<XRBaseInteractable> onUnselectAction;
        private bool init = false;

        public XRInteractorConfig(XRBaseInteractor baseInteractor,
            UnityAction<XRBaseInteractable> selectAction,
            UnityAction<XRBaseInteractable> unselectAction) {
            interactor = baseInteractor;
            onSelectAction = selectAction;
            onUnselectAction = unselectAction;
        }

        public void StartActions() {
            if (interactor && !init)
            {
                if(onSelectAction != null) interactor.onSelectEnter.AddListener(onSelectAction);
                if(onUnselectAction != null) interactor.onSelectExit.AddListener(onUnselectAction);
                init = true;
            }
        }

        public void TerminateActions() {
            if (interactor && init)
            {
                if (onSelectAction != null) interactor.onSelectEnter.RemoveListener(onSelectAction);
                if (onUnselectAction != null) interactor.onSelectExit.RemoveListener(onUnselectAction);
                init = false;
            }
        }
    }

    [SerializeField]
    private XRBaseInteractor[] leftInteractors;
    private XRInteractorConfig[] leftInteractorConfigs;

    [SerializeField]
    private XRBaseInteractor[] rightInteractors;
    private XRInteractorConfig[] rightInteractorConfigs;

    private void Awake()
    {
        leftInteractorConfigs = InitializeConfigs(leftInteractors);
        rightInteractorConfigs = InitializeConfigs(rightInteractors);
    }

    void DisableInteractors(XRBaseInteractor[] otherInteractors, XRBaseInteractor ignoreTarget)
    {
        foreach (XRBaseInteractor config in otherInteractors) {
            if (config) {
                // enabled only exception
                config.enableInteractions = (config == ignoreTarget);
            }
        }
    }

    void EnableInteractors(XRBaseInteractor[] targetInteractors)
    {
        foreach (XRBaseInteractor config in targetInteractors)
        {
            if (config)
                config.enableInteractions = true;
        }
    }

    XRInteractorConfig[] InitializeConfigs(XRBaseInteractor[] interactors) {
        XRInteractorConfig[] targetConfigs = new XRInteractorConfig[interactors.Length];
        for (int i = 0; i < interactors.Length; i++)
        {
            if (interactors[i])
            {
                XRBaseInteractor target = interactors[i];
                targetConfigs[i] = new XRInteractorConfig(
                    target,
                    delegate { DisableInteractors(interactors, target); },
                    delegate { EnableInteractors(interactors); }
                    );
            }
        }
        return targetConfigs;
    }

    void StartConfigs(XRInteractorConfig[] targetConfigs)
    {
        foreach (XRInteractorConfig config in targetConfigs)
        {
            if (config != null)
            {
                config.StartActions();
            }
        }
    }

    void TerminateConfigs(XRInteractorConfig[] targetConfigs) {
        foreach (XRInteractorConfig config in targetConfigs)
        {
            if (config != null)
            {
                config.TerminateActions();
            }
        }
    }

    private void OnEnable()
    {
        StartConfigs(leftInteractorConfigs);
        StartConfigs(rightInteractorConfigs);
    }

    private void OnDisable()
    {
        TerminateConfigs(leftInteractorConfigs);
        TerminateConfigs(rightInteractorConfigs);
    }

    private void OnDestroy()
    {
        TerminateConfigs(leftInteractorConfigs);
        TerminateConfigs(rightInteractorConfigs);
    }
}
