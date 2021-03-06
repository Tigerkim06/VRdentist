﻿
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// When this object touches other object with curtain layer [respawnOnCollsionLayer]
/// for respawn time, it will be respawned to spawn position.
/// Default spawn position is at start position of object.
/// In case it has XRGrabInteractable component, it won't respawn as long as the object is grabbed.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Respawner : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private float respawnTimer;

    private Vector3 defaultPosition;
    private Vector3 defaultEulerAngles;

    [SerializeField]
    public Transform respawnAt;
    [SerializeField]
    public float respawnTime = 1.5f;
    [SerializeField]
    private LayerMask respawnOnCollsionLayer = ~0;
    
    [SerializeField]
    [ReadOnly]
    [ModifiableProperty]
    private bool enableRespawn;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        if (grabInteractable) {
            grabInteractable.onSelectEnter.AddListener(OnGrabbed);
            grabInteractable.onSelectExit.AddListener(OnReleased);
        }
        defaultPosition = this.transform.position;
        defaultEulerAngles = this.transform.eulerAngles;
        enableRespawn = true;
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor) {
        enableRespawn = false;
        respawnTimer = 0;
    }

    void OnReleased(XRBaseInteractor rBaseInteractor) {
        enableRespawn = true;
        respawnTimer = 0;
    }
    
    void Update()
    {
        if (enableRespawn && respawnTimer > 0) {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0) {
                Respawn();
            }
        }
    }

    private void Respawn() {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        if (respawnAt) {
            this.transform.position = respawnAt.position;
            this.transform.eulerAngles = respawnAt.eulerAngles;
        }
        else {
            this.transform.position = defaultPosition;
            this.transform.eulerAngles = defaultEulerAngles;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (enableRespawn)
            if ((respawnOnCollsionLayer.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
                if(respawnTimer <= 0) respawnTimer = respawnTime;
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (enableRespawn)
            if ((respawnOnCollsionLayer.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
                if (respawnTimer > 0) respawnTimer = 0;
    }

    private void OnDestroy()
    {
        if (grabInteractable)
        {
            grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
            grabInteractable.onSelectEnter.RemoveListener(OnReleased);
        }
    }
}
