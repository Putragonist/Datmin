using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Screenshot : MonoBehaviour
{
    public GameObjectList goList;
    public string FolderName = "ScreenShot Folder SOA";
    public string FileName = "Screenshot";
    public int frameRate = 24;
    public bool isAlwaysCapture = false;
    public string objPositionFileName = "ObjectPosition";
    bool takingScreenshot = false;
    // Use this for initialization
    void Start () {
        // Set the playback framerate (real time will not relate to game time after this).
        Time.captureFramerate = frameRate;

    }

    // Update is called once per frame
    void Update() {       


        if(takingScreenshot)            
        {
            //check if taking screenshot is finished and file already writen on disk
            if (!File.Exists(pathFinal)){
                return;
            } else
            {
                takingScreenshot = false;
                GetComponent<CameraSetting>().Rotate();
            }
        }
        switch (isAlwaysCapture) {
            case false:

                if (Input.GetKeyDown(KeyCode.F9))
                    ScreenCaptureFunction();
                break;
            case true:
                ScreenCaptureFunction();
                break;
        }         

	}
    string pathFinal = string.Empty;
    

    public void ScreenCaptureFunction()
    {


        List<string> posText = new List<string>();

        foreach (GameObject go in goList.spawnGameObject)
        {
            if (go.tag.ToLower() != "bottle")
                continue;

            bool isVisible = false;

            //Check if Object is visible on camera
            foreach(Renderer r in go.GetComponentsInChildren<Renderer>())
            {
                if (r.isVisible)
                {
                    isVisible = true;
                    break;
                }
            }
            if (!isVisible)
            {

                GetComponent<CameraSetting>().Rotate();
                return;
            }

            

            Camera cam = this.GetComponent<Camera>();
            Vector3 screenPoint = cam.WorldToScreenPoint(go.transform.position);

            //make sure object is in front of camera
            if(screenPoint.z <= 0)
            {
                GetComponent<CameraSetting>().Rotate();
                return;
            }

            //Set default value (middle point of object
            int topPos = (int) screenPoint.y;
            int bottomPos = (int)screenPoint.y;
            int leftPos = (int)screenPoint.x;
            int rightPos = (int)screenPoint.x;

            //List for getting max and min value
            List<int> topList = new List<int>();
            List<int> bottomList = new List<int>();
            List<int> rightList = new List<int>();
            List<int> leftList = new List<int>();

            //Get every bound of object
            foreach(Renderer r in go.GetComponentsInChildren<Renderer>())
            {
                topList.Add((int)cam.WorldToScreenPoint(r.bounds.max).y);
                bottomList.Add((int)cam.WorldToScreenPoint(r.bounds.min).y);
                rightList.Add((int)cam.WorldToScreenPoint(r.bounds.max).x);
                leftList.Add((int)cam.WorldToScreenPoint(r.bounds.min).x);
            }

            //update position value
            topPos = Mathf.Max(topList.ToArray());
            bottomPos = Mathf.Min(bottomList.ToArray());
            rightPos = Mathf.Max(rightList.ToArray());
            leftPos = Mathf.Min(leftList.ToArray());

            //Post it in dataset
            if ((new Rect(0, 0, Screen.width, Screen.height)).Contains(screenPoint) && screenPoint.z > 0){

                posText.Add(topPos + ";" + bottomPos + ";" + rightPos + ";" + leftPos);;
            }
            
            
        }

        //if there is no bottle in screen, just update screen;
        if (posText.Count == 0)
        {
            GetComponent<CameraSetting>().Rotate();
            return;
        }

        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/" + FolderName + "/";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        int i = 0;
        pathFinal = "";      
        //Change filename
        do
        {
            i++;
            pathFinal = path + FileName+ (i < 10 ? "000" : i < 100 ? "00" : i < 1000 ? "0" : "") + i + ".png";
        }

        while (File.Exists(pathFinal));

        ScreenCapture.CaptureScreenshot(pathFinal);
               
        if (goList == null || goList.spawnGameObject == null)
            return;

        //post every object position
        foreach (string p in posText)
        {            
            if (!File.Exists(path + objPositionFileName))
            {
                using (StreamWriter sw = File.CreateText(path + objPositionFileName))
                {
                    sw.WriteLine("image_location;top_pos;bottom_pos;right_pos;left_pos");
                    sw.WriteLine(pathFinal + ";" + p);
                }
            } 
            else
            {
                using (StreamWriter sw = File.AppendText(path + objPositionFileName))
                {
                    sw.WriteLine(pathFinal + ";" + p);
                }
            }
        }
        ScreenCapture.CaptureScreenshot(pathFinal);
        takingScreenshot = true;       

    }
    
}