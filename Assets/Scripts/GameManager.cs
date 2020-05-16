using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Player
{
    public Player(int index)
    {
        index_player = index;
    }

    public int GetOtherPlayer()
    {
        if (index_player == 0)
        {
            return 1;
        }
        return 0;
    }

    public int current_round_points = 0;
    public int total_points = 0;
    public int index_player = -1;
}

public class GameManager : MonoBehaviour
{
    public GameObject red_Dart;
    public GameObject blue_Dart;

    public float dart_transition_time = 0.5f;

    // Internal variables
    List<GameObject> current_darts = new List<GameObject>();
    float time_at_lerp_start = 0.0f;
    Transform original_dart_transform;

    // Players
    List<Player> players = new List<Player>();
    Player current_player;

    Dart activeDart = null;

    int total_round = 6;
    int current_round = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; ++i)
        {
            players.Add(new Player(i));
        }
        
        NewRound(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && activeDart == null)
        {
            StartCoroutine(TakeDart());
        }
    }

    public void DartThrown()
    {
        Destroy(activeDart.gameObject);
        activeDart = null;

        if (current_darts.Count == 0)
        {
            current_player = players[current_player.GetOtherPlayer()];
            NewRound(current_player.index_player);
        }
    }

    void NewRound(int player_index)
    {
        ++current_round;

        if (current_round > total_round)
        {
            GameObject.Find("ARScene").SetActive(false);
            GameObject.Find("PointsScene").SetActive(true);
        }
        else {
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
    }

    IEnumerator TakeDart()
    {
        if (activeDart != null)
        {
            Destroy(activeDart.gameObject);
            activeDart = null;
        }

        if (current_darts.Count != 0)
        {

            current_darts.Last().transform.SetParent(GameObject.Find("ARCamera").transform);
            original_dart_transform = current_darts.Last().transform;

            time_at_lerp_start = Time.realtimeSinceStartup;

            while (((Time.realtimeSinceStartup - time_at_lerp_start) / dart_transition_time) < 1.0f)
            {
                Vector3.Lerp(original_dart_transform.localPosition, new Vector3(0, -0.45f, 1.25f), (Time.realtimeSinceStartup - time_at_lerp_start) / dart_transition_time);
                Quaternion.Lerp(original_dart_transform.localRotation, Quaternion.Euler(0, 180, 0), (Time.realtimeSinceStartup - time_at_lerp_start) / dart_transition_time);

                yield return new WaitForEndOfFrame();
            }

            current_darts.Last().transform.localPosition = new Vector3(0, -0.45f, 1.25f);
            current_darts.Last().transform.localRotation = Quaternion.Euler(0, 180, 0);
            current_darts.Last().GetComponent<Dart>().isActive = true;
            activeDart = current_darts.Last().GetComponent<Dart>();
            current_darts.Remove(current_darts.Last());
        }
    }
}
