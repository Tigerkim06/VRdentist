using UnityEngine;

public class MouthController : MonoBehaviour
{
    private static readonly string pull = "pull";
    private static readonly string open = "open";

    public Collider[] verticalCols;
    
    public Collider[] horizontalCols;

    public Animator mouthAnim;

    [SerializeField]
    private LayerMask layermask = ~0;


    void Start()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (!mouthAnim) return;
        if (layermask == (layermask | (1 << other.gameObject.layer)))
        {
            mouthAnim.SetFloat(open, 0);
            mouthAnim.SetFloat(pull, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!mouthAnim) return;
        if (layermask == (layermask | (1 << other.gameObject.layer))) {
            bool verticalHit = CheckIntersect(other, verticalCols);
            bool horizontalHit = CheckIntersect(other, horizontalCols);
            mouthAnim.SetFloat(open, verticalHit ? 1f : 0);
            mouthAnim.SetFloat(pull, horizontalHit ? 1f : 0);
        }
    }
    

    private bool CheckIntersect(Collider target, Collider[] areas)
    {
        bool isIntersect = false;
        foreach (Collider col in areas)
        {
            if (col.bounds.Intersects(target.bounds))
            {
                isIntersect = true;
                break;
            }
        }
        return isIntersect;
    }

}
