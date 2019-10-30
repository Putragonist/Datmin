using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{

    public GameObject point;
    public float minRadius = 5;
    public float maxRadius = 25;
    int radius = 0;
    int iteration = 0;
    // Start is called before the first frame update
    void Start()
    {
        float minRadius = this.minRadius;
        float radius = minRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (radius >= maxRadius)
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
            return;
            
        }

        float x = Random.Range((float)-radius / 2, (float)radius / 2);
        float y = Random.Range(1, (float)radius / 2);
        float z = Random.Range((float)-radius / 2, (float)radius / 2);
        this.transform.position = new Vector3(point.transform.position.x + x, point.transform.position.y + y, 
            point.transform.position.z + z);
        this.transform.LookAt(point.transform.position);
            
        iteration++;
        if (iteration > 10)
        {
            iteration = 0;
            radius++;
        }
    }
}