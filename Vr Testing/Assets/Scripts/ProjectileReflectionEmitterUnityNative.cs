using UnityEditor;
using UnityEngine;

public class ProjectileReflectionEmitterUnityNative : MonoBehaviour
{
    public GameObject startPos;
    public GameObject lastHitObject;

    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;

    [SerializeField] private int numBounces = 0;


    public LineRenderer laser;

    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.startPos.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.startPos.transform.position, 0.25f);

        DrawPredictedReflectionPattern(this.startPos.transform.position + this.transform.forward * 0.75f, this.transform.forward, maxReflectionCount);
    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0)
        {
            laser.positionCount = maxReflectionCount;
            return;
        }

        if (reflectionsRemaining == maxReflectionCount)
        {
            numBounces = 0;
        }

        Vector3 startingPosition = position;
        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxStepDistance))
        {
            if (hit.collider.tag == "Mirror")
            {
                direction = Vector3.Reflect(direction, hit.normal);
                position = hit.point;
                numBounces++;
                laser.positionCount = maxReflectionCount;
            }
            else if (hit.collider.tag == "LaserActivator")
            {
                hit.collider.gameObject.GetComponent<LaserActivator>().SendMessage("onHitEnter");
                position = hit.point;
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startingPosition, position);
                laser.positionCount = numBounces + 2;
                laser.SetPosition(0, startPos.transform.position);
                laser.SetPosition(numBounces + 1, position);

                if (lastHitObject != hit.collider.gameObject)
                {
                    lastHitObject.GetComponent<LaserActivator>().SendMessage("onHitExit");
                    lastHitObject = hit.collider.gameObject;
                }
                return;
            }
            else
            {
                position = hit.point;
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startingPosition, position);
                laser.positionCount = numBounces + 2;
                laser.SetPosition(0, startPos.transform.position);
                laser.SetPosition(numBounces + 1, position);
                return;
            }
        }
        else
        {
            position += direction * maxStepDistance;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startingPosition, position);
        laser.positionCount = maxReflectionCount + 1;
        laser.SetPosition(0, startPos.transform.position);
        laser.SetPosition((maxReflectionCount + 1) - reflectionsRemaining, position);

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }
}
