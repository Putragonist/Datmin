using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting1 : MonoBehaviour
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
        CameraPositionUpdate();
    }

    float timeMax = 10;
    // Update is called once per frame
    void Update()
    {
        //this.transform.LookAt(objList.transform.position);
        //CameraPositionUpdate();
       
        if (Time.time - timeSpawn < timeMax)
        {
          //  Debug.Log("Waiting Physic");
            return;

        }
        Debug.Log("Physic Done");
        timeSpawn = Time.time;

        if (radius >= maxRadius)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            return;
        }

        CameraPositionUpdate();
        
        
        this.transform.LookAt(objList.point.transform);
        GetComponent<Screenshot>().ScreenCaptureFunction();
        objList.NewObjectSpawn();
       // CameraPositionUpdate();
        iteration++;
        if (iteration > 1)
        {
            iteration = 0;
            radius++;
        }
        //}
    }

    public void CameraPositionUpdate()
    {
        point = objList.point;
        Debug.Log("Point is " + point.name);
        float x = Random.Range((float)-radius / 2, (float)radius / 2);
        float y = Random.Range(1, (float)radius / 2);
        float z = Random.Range((float)-radius / 2, (float)radius / 2);
        //foreach (GameObject trans in cameras)
        //{
        if (objList.point != null)
        {
            this.transform.position = new Vector3(x, y, z);
          //  Debug.Log(transform.position);
        }
        else
        {
            Debug.Log("No point");
        }        
    }
}
