using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Player
{
    int points = 0;
}

public class GameManager : MonoBehaviour
{
    public GameObject red_Dart;
    public GameObject blue_Dart;

    public float dart_transition_time = 0.5f;

    // Internal variables
    List<GameObject> current_darts = new List<GameObject>();
    bool dart_taken = false;
    bool dart_in_position = false;
    float time_at_lerp_start = 0.0f;
    bool lerping_dart = false;
    Transform original_dart_transform;

    // Players
    List<Player> players = new List<Player>();
    Player current_player;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; ++i)
        {
            players.Add(new Player());
        }
        
        NewRound(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!dart_taken)
            {
                TakeDart();
            }
            else if (dart_taken && dart_in_position) 
            {
                ThrowDart();
            }
        }

        if (lerping_dart)
        {
            float t = (Time.realtimeSinceStartup - time_at_lerp_start) / dart_transition_time;

            Vector3.Lerp(original_dart_transform.localPosition, new Vector3(0, -0.45f, 1.25f), t);
            Quaternion.Lerp(original_dart_transform.localRotation, Quaternion.Euler(0, 180, 0), t);

            if (t >= 1)
            {
                current_darts.Last().transform.localPosition = new Vector3(0, -0.45f, 1.25f);
                current_darts.Last().transform.localRotation = Quaternion.Euler(0, 180, 0);
                dart_in_position = true;
                lerping_dart = false;
            }
        }
    }

    void NewRound(int player_index)
    {
        current_player = players[player_index];

        if (current_darts.Count != 0)
        {
            current_darts.Clear();
        }

        for (int i = 0; i < 3; ++i)
        {
            if (player_index == 0)
            {
                current_darts.Add(Instantiate(red_Dart, GameObject.Find("Darts").transform));
              
            }
            else
            {
                current_darts.Add(Instantiate(blue_Dart, GameObject.Find("Darts").transform));
            }

            current_darts[i].transform.SetPositionAndRotation(
                  new Vector3(current_darts[i].transform.position.x + (i - 1) * 0.2f, 0, -0.45f),
                   Quaternion.Euler(-5, 0, 45));
        }
    }

    void TakeDart()
    {
        dart_taken = true;

        current_darts.Last().transform.SetParent(GameObject.Find("ARCamera").transform);
        original_dart_transform = current_darts.Last().transform;

        time_at_lerp_start = Time.realtimeSinceStartup;

        // Lerp to camera
        //StartCoroutine("LerpToCamera");
        lerping_dart = true;
    }

    void ThrowDart()
    {
        dart_in_position = false;
        current_darts.Last().GetComponent<Rigidbody>().AddForce(current_darts.Last().transform.forward * 50);
        current_darts.Last().GetComponent<Rigidbody>().useGravity = true;
    }

    //IEnumerator LerpToCamera()
    //{
    //    float t = (Time.realtimeSinceStartup - time_at_lerp_start) / dart_transition_time;

    //    Vector3.Lerp(original_dart_transform.localPosition, new Vector3(0, -0.45f, 1.25f), t);
    //    Quaternion.Lerp(original_dart_transform.localRotation, Quaternion.Euler(0, 180, 0), t);

    //    if (t >= 1)
    //    {
    //        current_darts.Last().transform.localPosition = new Vector3(0, -0.45f, 1.25f);
    //        current_darts.Last().transform.localRotation = Quaternion.Euler(0, 180, 0);
    //        dart_in_position = true;
    //        StopCoroutine("LerpToCamera");
    //    }

    //    Debug.Log("1 cicle");
    //    yield return new WaitForFixedUpdate();
    //}
}
