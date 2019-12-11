using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;
    public Transform[] points;

    [SerializeField] private bool isActive = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private int positionInPoints = 0;
    private Vector3 lastPos;
    private Vector3 target;
    private Vector3 heading;
    private float distance;

    public float waitTime = 3.0f;
    public float lerpSpeed = 10.0f;
    public float MaxSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        target = points[0].position;
        lastPos = target;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        movePlatformAround();
    }

    private void movePlatformAround()
    {
        if (isMoving)
        {
            heading = target - platform.transform.position;
            distance = Vector3.Distance(target, platform.transform.position);
            platform.GetComponent<Rigidbody>().MovePosition(platform.transform.position + (MaxSpeed * (heading / Mathf.Clamp(distance, 1, distance) * Time.deltaTime)));
        }
        else if (!isActive)
        {
            platform.GetComponent<Rigidbody>().MovePosition(lastPos);
        }
    }

    public void toggleMove()
    {
        if (isMoving)
        {
            lastPos = platform.transform.position;
            isMoving = false;
            isActive = false;
            StopCoroutine("movePlatform");
        }
        else
        {
            isActive = true;
            StartCoroutine("movePlatform");
        }
    }

    IEnumerator movePlatform()
    {
        if (points.Length >= 2)
        {
            int direction = 1;
            int start = 0;
            int stop = points.Length;
            int i = positionInPoints;

            while (i != stop)
            {
                target = points[i].position;
                isMoving = true;
                yield return new WaitUntil(atTarget);
                isMoving = false;
                if (direction == 1)
                {
                    if (i == stop - 1)
                    {
                        start = points.Length;
                        stop = -1;
                        direction = -1;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        start = 0;
                        stop = points.Length;
                        direction = 1;
                    }
                }
                yield return new WaitForSeconds(waitTime);
                i += direction;
                positionInPoints = i;
            }
        }
    }

    public bool atTarget()
    {
        if (Vector3.Distance(platform.transform.position, target) <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (points.Length >= 2)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }
    }
}
