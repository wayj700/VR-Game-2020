using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoor : MonoBehaviour
{
    public GameObject door;
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
        moveDoorAround();
    }

    private void moveDoorAround()
    {
        if (isMoving)
        {
            heading = target - door.transform.position;
            distance = Vector3.Distance(target, door.transform.position);
            door.GetComponent<Rigidbody>().MovePosition(door.transform.position + (MaxSpeed * (heading / Mathf.Clamp(distance, 1, distance) * Time.deltaTime)));
        }
        else if (!isActive)
        {
            door.GetComponent<Rigidbody>().MovePosition(lastPos);
        }
    }

    public void toggleMove()
    {
        if(isMoving == false)
        {
            isActive = true;
            StartCoroutine("moveDoor");
        }
    }

    IEnumerator moveDoor()
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
                i += direction;
                positionInPoints = i;
                isActive = false;
                break;
            }
        }
    }

    public bool atTarget()
    {
        if (Vector3.Distance(door.transform.position, target) == 0.01f)
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
