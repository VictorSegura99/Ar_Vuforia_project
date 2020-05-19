using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player
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

    public int round1 = 0;
    public int round2 = 0;
    public int round3 = 0;
}

enum EndMenuState { 
    START,
    ROUND_1,
    ROUND_2,
    ROUND_3,
    TOTALS,
    FINISHED
}

public class GameManager : MonoBehaviour
{
    // -------------------------
    // END MENU TEXTS
    public GameObject player1_round1;
    public GameObject player1_round2;
    public GameObject player1_round3;
    public GameObject player2_round1;
    public GameObject player2_round2;
    public GameObject player2_round3;
    public GameObject player1_totals;
    public GameObject player2_totals;
    // -------------------------


    public GameObject red_Dart;
    public GameObject blue_Dart;

    public float dart_transition_time = 0.5f;

    // Internal variables
    public List<GameObject> current_darts = new List<GameObject>();
    public List<GameObject> darts_thrown = new List<GameObject>();
    Transform original_dart_transform;

    // Players
    List<Player> players = new List<Player>();

    [HideInInspector]
    public Player current_player;

    Dart activeDart = null;

    int total_round = 6;
    int current_round = 0;

    public GameObject nextScene = null;

    EndMenuState current_menu_state = EndMenuState.START;
    float end_menu_time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; ++i)
        {
            players.Add(new Player(i));
        }

        nextScene.SetActive(true);
        GameObject.Find("MainMenuSceneText").GetComponent<TextMeshProUGUI>().enabled = false;

        nextScene.SetActive(false);

        NewRound(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(activeDart == null && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            TakeDart();
        }

        if (nextScene.activeInHierarchy) 
        {
            switch (current_menu_state)
            {
                case EndMenuState.START:
                    {
                        if (Time.realtimeSinceStartup - end_menu_time > 0.5f)
                        {
                            current_menu_state = EndMenuState.ROUND_1;
                            end_menu_time = Time.realtimeSinceStartup;
                        }
                        break;
                    }
                case EndMenuState.ROUND_1:
                    {
                        float t = (Time.realtimeSinceStartup - end_menu_time) / 0.5f;
                        float lerp_1 = (int)Mathf.Lerp(0, players[0].round1, t);
                        float lerp_2 = (int)Mathf.Lerp(0, players[1].round1, t);

                        player1_round1.GetComponent<TextMeshProUGUI>().text = lerp_1.ToString();
                        player2_round1.GetComponent<TextMeshProUGUI>().text = lerp_2.ToString();

                        if (t >= 1)
                        {
                            player1_round1.GetComponent<TextMeshProUGUI>().text = players[0].round1.ToString();
                            player2_round1.GetComponent<TextMeshProUGUI>().text = players[1].round1.ToString();

                            current_menu_state = EndMenuState.ROUND_2;
                            end_menu_time = Time.realtimeSinceStartup;
                        }

                        break;
                    }
                case EndMenuState.ROUND_2:
                    {
                        float t = (Time.realtimeSinceStartup - end_menu_time) / 0.5f;
                        float lerp_1 = (int)Mathf.Lerp(0, players[0].round2, t);
                        float lerp_2 = (int)Mathf.Lerp(0, players[1].round2, t);

                        player1_round2.GetComponent<TextMeshProUGUI>().text = lerp_1.ToString();
                        player2_round2.GetComponent<TextMeshProUGUI>().text = lerp_2.ToString();

                        if (t >= 1)
                        {
                            player1_round2.GetComponent<TextMeshProUGUI>().text = players[0].round2.ToString();
                            player2_round2.GetComponent<TextMeshProUGUI>().text = players[1].round2.ToString();

                            current_menu_state = EndMenuState.ROUND_3;
                            end_menu_time = Time.realtimeSinceStartup;
                        }

                        break;
                    }
                case EndMenuState.ROUND_3:
                    {
                        float t = (Time.realtimeSinceStartup - end_menu_time) / 0.5f;
                        float lerp_1 = (int)Mathf.Lerp(0, players[0].round3, t);
                        float lerp_2 = (int)Mathf.Lerp(0, players[1].round3, t);

                        player1_round3.GetComponent<TextMeshProUGUI>().text = lerp_1.ToString();
                        player2_round3.GetComponent<TextMeshProUGUI>().text = lerp_2.ToString();

                        if (t >= 1)
                        {
                            player1_round3.GetComponent<TextMeshProUGUI>().text = players[0].round3.ToString();
                            player2_round3.GetComponent<TextMeshProUGUI>().text = players[1].round3.ToString();

                            current_menu_state = EndMenuState.TOTALS;
                            end_menu_time = Time.realtimeSinceStartup;
                        }

                        break;
                    }
                case EndMenuState.TOTALS:
                    {
                        float t = (Time.realtimeSinceStartup - end_menu_time) / 0.5f;
                        float lerp_1 = (int)Mathf.Lerp(0, players[0].total_points, t);
                        float lerp_2 = (int)Mathf.Lerp(0, players[1].total_points, t);

                        player1_totals.GetComponent<TextMeshProUGUI>().text = lerp_1.ToString();
                        player2_totals.GetComponent<TextMeshProUGUI>().text = lerp_2.ToString();

                        if (t >= 1)
                        {
                            player1_totals.GetComponent<TextMeshProUGUI>().text = players[0].total_points.ToString();
                            player2_totals.GetComponent<TextMeshProUGUI>().text = players[1].total_points.ToString();

                            current_menu_state = EndMenuState.FINISHED;
                            end_menu_time = Time.realtimeSinceStartup;
                        }

                        break;
                    }
                case EndMenuState.FINISHED:
                    {
                        if (Time.realtimeSinceStartup - end_menu_time > 0.5f)
                        {
                            if (GameObject.Find("MainMenuSceneText").GetComponent<TextMeshProUGUI>().enabled == false)
                            {
                                GameObject.Find("MainMenuSceneText").GetComponent<TextMeshProUGUI>().enabled = true;
                            }

                            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                            {
                                Debug.Log("Main Menu Going!");
                                SceneManager.LoadScene("MainMenu");
                            }
                        }

                        break;
                    }
            }
        }
    }

    public void ResetActual()
    {
        activeDart = null;
    }

    public void DartThrown()
    {
        for (int i = 0; i < darts_thrown.Count; ++i)
        {
            Destroy(darts_thrown[i]);
            darts_thrown[i] = null;
        }
        darts_thrown.Clear();

        if (current_round >= 5)
        {
            current_player.round3 = current_player.current_round_points;
        }
        else if (current_round >= 3)
        {
            current_player.round2 = current_player.current_round_points;
        }
        else
        {
            current_player.round1 = current_player.current_round_points;
        }

        current_player.current_round_points = 0;

        current_player = players[current_player.GetOtherPlayer()];
        NewRound(current_player.index_player);
    }

    void NewRound(int player_index)
    {
        ++current_round;

        if (current_round > total_round)
        {
            GameObject.Find("ARScene").SetActive(false);
            nextScene.SetActive(true);

            for (int i = 0; i < 2; ++i) 
            {
                players[i].total_points = players[i].round1 + players[i].round2 + players[i].round3;
            }

            end_menu_time = Time.realtimeSinceStartup;
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
                      new Vector3(current_darts[i].transform.position.x + (i - 1) * 20, 0.2f, -90.0f),
                       Quaternion.Euler(-5, 0, 45));
            }
        }
    }

    void TakeDart()
    {
        if (current_darts.Count != 0)
        {
            current_darts.Last().transform.SetParent(GameObject.Find("ARCamera").transform);
            original_dart_transform = current_darts.Last().transform;

            current_darts.Last().transform.localPosition = new Vector3(0, -5, 25);
            current_darts.Last().transform.localRotation = Quaternion.Euler(1.25f, 180, 45);
            current_darts.Last().GetComponent<Dart>().Invoke("SetActiveDart", 0.3F);
            activeDart = current_darts.Last().GetComponent<Dart>();
            current_darts.Remove(current_darts.Last());
        }
    }
}
