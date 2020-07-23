
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class Respawner : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Vector3 respawnPosition;
    private Vector3 respawnEulerAngles;
    private float respawnTimer;

    [SerializeField]
    public float respawnTime = 1.5f;
    [SerializeField]
    private LayerMask respawnOnCollsionLayer = ~0;

    [SerializeField][ReadOnly]
    private HandPresence handPresence;
    [SerializeField]
    [ReadOnly]
    private bool enableRespawn;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEnter.AddListener(OnGrabbed);
        grabInteractable.onSelectExit.AddListener(OnReleased);
        respawnPosition = this.transform.position;
        respawnEulerAngles = this.transform.eulerAngles;
        enableRespawn = true;
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor) {
        handPresence = rBaseInteractor.GetComponentInChildren<HandPresence>();
        enableRespawn = false;
        respawnTimer = 0;
    }

    void OnReleased(XRBaseInteractor rBaseInteractor) {
        handPresence = null;
        enableRespawn = true;
        respawnTimer = 0;
    }

    // Update is called once per frame
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
        this.transform.position = respawnPosition;
        this.transform.eulerAngles = respawnEulerAngles;
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
        grabInteractable.onSelectEnter.RemoveListener(OnGrabbed);
        grabInteractable.onSelectEnter.RemoveListener(OnReleased);
    }
}
