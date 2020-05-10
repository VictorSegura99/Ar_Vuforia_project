using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

class Player
{
    int points = 0;
    int darts_thrown = 0;
}

public class GameManager : MonoBehaviour
{
    public GameObject red_Dart;
    public GameObject blue_Dart;

    // Internal variables
    List<GameObject> current_darts = new List<GameObject>();

    public int darts_thrown = 0;

    [HideInInspector]
    List<Player> players = new List<Player>();
    int current_player = 0;

    // Start is called before the first frame update
    void Start()
    {
        NewRound(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewRound(int player_index)
    {
        current_player = player_index;

        if (current_darts.Count != 0)
        {
            current_darts.Clear();
        }

        for (int i = 0; i < 3; ++i)
        {
            if (player_index == 0)
            {
                current_darts.Add(Instantiate(red_Dart, GameObject.Find("Darts").transform));
                current_darts[i].transform.SetPositionAndRotation(
                    new UnityEngine.Vector3(current_darts[i].transform.position.x + (i - 1) * 0.2f, 0, -0.45f),
                     UnityEngine.Quaternion.Euler(-5, 0, 45));
            }
            else
            {
                current_darts.Add(Instantiate(blue_Dart, GameObject.Find("Darts").transform));
                current_darts[i].transform.SetPositionAndRotation(
                   new UnityEngine.Vector3(current_darts[i].transform.position.x + (i - 1) * 0.2f, 1, -5.33f),
                   UnityEngine.Quaternion.Euler(-5, 0, 45));
            }
        }
    }

    void TakeDart()
    {
        Destroy(current_darts.Last());
        current_darts.Remove(current_darts.Last());
        GameObject dart;

        if (current_player == 0) 
        {
            dart = GameObject.Instantiate(red_Dart, GameObject.Find("ARCamera").transform);
        }
        else
        {

        }
    }
}
