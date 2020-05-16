using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [HideInInspector]
    public TextMeshPro points;

    // Start is called before the first frame update
    void Start()
    {
        points = gameObject.transform.GetChild(1).GetComponent<TextMeshPro>();
        points.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrentPlayerPoints(int new_points)
    {
        StartCoroutine(LerpPoints(int.Parse(this.points.text) + new_points, int.Parse(this.points.text)));
    }
    
    IEnumerator LerpPoints(int current_points, int previous_points)
    {
        float time = Time.realtimeSinceStartup;

        while (current_points != previous_points) 
        {
            points.text = Mathf.Lerp(previous_points, current_points, (time - Time.realtimeSinceStartup) / 0.5f).ToString();

            if (((time - Time.realtimeSinceStartup) / 0.5f) >= 1)
            {
                current_points = previous_points;
                points.text = current_points.ToString();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
