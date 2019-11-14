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

        // Create the folder
        //System.IO.Directory.CreateDirectory(folder);
    }

    // Update is called once per frame
    void Update() {       


        if(takingScreenshot)            
        {
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

       // bool isEmpty = true;
        List<string> posText = new List<string>();
        foreach (GameObject go in goList.spawnGameObject)
        {
            if (go.tag.ToLower() != "bottle")
                continue;

            bool isVisible = false;
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
                //Debug.Log("IsNotVisible");
                GetComponent<CameraSetting>().Rotate();
                return;
            }
           // Debug.Log("IsVisible");
            
            Camera cam = this.GetComponent<Camera>();
            Vector3 screenPoint = cam.WorldToScreenPoint(go.transform.position);
            if ((new Rect(0, 0, Screen.width, Screen.height)).Contains(screenPoint) && screenPoint.z > 0){
                
                posText.Add(screenPoint.x + ";" + screenPoint.y + ";" + screenPoint.z);
            }
            
            
        }

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

        do
        {
            i++;
            pathFinal = path + FileName+ (i < 10 ? "000" : i < 100 ? "00" : i < 1000 ? "0" : "") + i + ".png";
        }

        while (File.Exists(pathFinal));

        ScreenCapture.CaptureScreenshot(pathFinal);



        //string path2 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/" + FolderName + "Point/";

        if (goList == null || goList.spawnGameObject == null)
            return;
        foreach (string p in posText)
        {
            
            //else { // (screenPoint.y > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1){
            if (!File.Exists(path + objPositionFileName))
            {
                using (StreamWriter sw = File.CreateText(path + objPositionFileName))
                {
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
            //}
        }
        ScreenCapture.CaptureScreenshot(pathFinal);
        takingScreenshot = true;

        

    }
    /*
    // The folder to contain our screenshots.
    // If the folder exists we will append numbers to create an empty folder.
    public string folder = "ScreenshotFolder";
    public int frameRate = 25;
    void Start()
    {
        // Set the playback framerate (real time will not relate to game time after this).
        Time.captureFramerate = frameRate;

        // Create the folder
        System.IO.Directory.CreateDirectory(folder);
    }

    void Update()
    {
        // Append filename to folder name (format is '0005 shot.png"')
        string name = string.Format("{0}/{1:D04} shot.png", folder, Time.frameCount);

        // Capture the screenshot to the specified file.
        ScreenCapture.CaptureScreenshot(name);
    }
    */
}