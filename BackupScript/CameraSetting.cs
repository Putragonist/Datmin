using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public GameObjectList objList;
    public List<GameObject> cameras;
    public bool isLast;
    public static bool lastFinish = false;
    public GameObject point;
    public float minRadius = 5;
    public float maxRadius = 25;
    int radius = 0;
    int iteration = 0;

    float timeSpawn = 10;
    // Start is called before the first frame update
    void Start()
    {
        float minRadius = this.minRadius;
        float radius = minRadius;
        timeSpawn = Time.time;
        objList.NewObjectSpawn();
        //CameraPositionUpdate();
    }

    float timeMax = 10;
    // Update is called once per frame
    void Update()
    {
        //this.transform.LookAt(objList.transform.position);
        CameraPositionUpdate();
        if (Time.time - timeSpawn < timeMax)
        {
            return;
        }

        timeSpawn = Time.time;

        if (radius >= maxRadius)
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
            return;
            
        }

        Debug.Log("Run this");

        
            iteration++;
            if (iteration > 10)
            {
                iteration = 0;
                radius++;
            }
        //}
        GetComponent<Screenshot>().ScreenCaptureFunction();
        objList.NewObjectSpawn();
    }

    public void CameraPositionUpdate()
    {
        float x = Random.Range((float)-radius / 2, (float)radius / 2);
        float y = Random.Range(1, (float)radius / 2);
        float z = Random.Range((float)-radius / 2, (float)radius / 2);
        //foreach (GameObject trans in cameras)
        //{
        this.transform.position = new Vector3(objList.transform.position.x + x, objList.transform.position.y + y,
            objList.transform.position.z + z);
        this.transform.LookAt(objList.transform.position);
    }
}