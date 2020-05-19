using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI points;

    // Start is called before the first frame update
    void Start()
    {
        Transform points_GO = gameObject.transform.GetChild(1);
        points = points_GO.GetComponent <TextMeshProUGUI>();
        points.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrentPlayerPoints(int new_points)
    {
        StartCoroutine(LerpPoints(int.Parse(points.text) + new_points, int.Parse(points.text)));
    }
    
    IEnumerator LerpPoints(int current_points, int previous_points)
    {
        float time = Time.realtimeSinceStartup;

        while (current_points != previous_points) 
        {
            points.text = ((int)Mathf.Lerp(previous_points, current_points, (Time.realtimeSinceStartup - time) / 0.5f)).ToString();

            if (((Time.realtimeSinceStartup - time) / 0.5f) >= 1) 
            {
                previous_points = current_points;
                points.text = ((int)current_points).ToString();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
