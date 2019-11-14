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
    public float timeCapture = 360;
    float lastTime = 0;
    public bool isManualRotation = false;
    // Start is called before the first frame update
    void Start()
    {
        float minRadius = this.minRadius;
        float radius = minRadius;
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastTime > timeCapture)
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        if (!isManualRotation)
            Rotate();

        
    }

    public void Rotate()
    {
        if (radius >= maxRadius)
        {
            radius = 0;

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