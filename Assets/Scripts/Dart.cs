using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dart : MonoBehaviour
{
    Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

    public float throwForceInXandY = 0f;
    public float throwForceInZ = 1500f;

    Rigidbody rb;

    public bool isActive = false;
    bool throww = false;
    List<TriggerData> triggers_passed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        triggers_passed = new List<TriggerData>();
    }

    public void SetActiveDart()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || throww)
        {
            return;
        }

        // if you touch the screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // getting touch position and marking time when you touch the screen
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        // if you release your finger
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // marking time when you release it
            touchTimeFinish = Time.time;

            // calculate swipe time interval 
            timeInterval = touchTimeFinish - touchTimeStart;

            // getting release finger position
            endPos = Input.GetTouch(0).position;

            // calculating swipe direction in 2D space
            direction = startPos - endPos;

            // add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
            rb.useGravity = true;
            rb.isKinematic = false;
            Physics.gravity = new Vector3(0,-300, 0);
            rb.AddForce(-direction.x * 100, -direction.y * 25, (throwForceInZ / timeInterval) * 10);

            Destroy(gameObject, 3.0F);
            GameObject.Find("GameManager").GetComponent<GameManager>().Invoke("ActiveDart", 1.3F);
            throww = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TriggerData>().absolute_trigger)
        {
            rb.isKinematic = true;
            GetComponent<AudioSource>().Play();

            if (triggers_passed.Count != 0)
            {
                TriggerData lowest_priority = triggers_passed.First();

                for (int i = 0; i < triggers_passed.Count; ++i)
                {
                    if (triggers_passed[i].priority < lowest_priority.priority)
                    {
                        lowest_priority = triggers_passed[i];
                    }
                }

                GameObject.Find("GameManager").GetComponent<GameManager>().current_player.current_round_points += lowest_priority.points;
                GameObject.Find("Points").GetComponent<UI>().UpdateCurrentPlayerPoints(lowest_priority.points);
            }
        }
        else
        {
            triggers_passed.Add(other.gameObject.GetComponent<TriggerData>());
        }
    }
}
