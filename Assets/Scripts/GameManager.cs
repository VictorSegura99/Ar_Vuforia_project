using System.Collections;
using System.Collections.Generic;
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
    List<GameObject> current_darts;

    public int darts_thrown = 0;

    [HideInInspector]
    List<Player> players;

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
        current_darts.Clear();

        for (int i = 0; i < 3; ++i)
        {
            if (player_index == 0)
            {
                current_darts.Add(Instantiate(red_Dart));
            }
            else
            {
                current_darts.Add(Instantiate(blue_Dart));
            }
        }
    }
}
