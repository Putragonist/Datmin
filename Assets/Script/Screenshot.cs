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
            if(screenPoint.z <= 0)
            {
                GetComponent<CameraSetting>().Rotate();
                return;
            }

            int topPos = (int) screenPoint.y;
            int bottomPos = (int)screenPoint.y;
            int leftPos = (int)screenPoint.x;
            int rightPos = (int)screenPoint.x;
            //int layer_mask = LayerMask.GetMask("bottle");
            //int layer_mask = 1 << 8;
            //List<Vector2> set = new List<Vector2>();
            List<int> topList = new List<int>();
            List<int> bottomList = new List<int>();
            List<int> rightList = new List<int>();
            List<int> leftList = new List<int>();

            foreach(Renderer r in go.GetComponentsInChildren<Renderer>())
            {
                topList.Add((int)cam.WorldToScreenPoint(r.bounds.max).y);
                bottomList.Add((int)cam.WorldToScreenPoint(r.bounds.min).y);
                rightList.Add((int)cam.WorldToScreenPoint(r.bounds.max).x);
                leftList.Add((int)cam.WorldToScreenPoint(r.bounds.min).x);
            }

            topPos = Mathf.Max(topList.ToArray());
            bottomPos = Mathf.Min(bottomList.ToArray());
            rightPos = Mathf.Max(rightList.ToArray());
            leftPos = Mathf.Min(leftList.ToArray());


            /*
            topPos = (int)cam.WorldToScreenPoint(go.GetComponent<Renderer>().bounds.max).y;
            bottomPos = (int)cam.WorldToScreenPoint(go.GetComponent<Renderer>().bounds.min).y;
            rightPos = (int)cam.WorldToScreenPoint(go.GetComponent<Renderer>().bounds.max).x;
            leftPos = (int)cam.WorldToScreenPoint(go.GetComponent<Renderer>().bounds.min).x;
            */
            
            /*
            for (int x = (int)screenPoint.x; x <= cam.pixelWidth; x++)
            {
                for (int y = (int)screenPoint.y; y <= cam.pixelHeight; y++)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100000, layer_mask))
                    {
                        Debug.Log("hit" + hit.ToString());
                        set.Add(new Vector2(x, y));
                        break;
                    }
                }
                for (int y = (int)screenPoint.y; y >= 0; y--)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100000, layer_mask))
                    {
                        Debug.Log("hit" + hit.ToString());
                        set.Add(new Vector2(x, y));
                        break;
                    }
                }
            }

            for (int x = (int)screenPoint.x; x >= 0; x--)
            {
                for (int y = (int)screenPoint.y; y <= cam.pixelHeight; y++)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100000, layer_mask))
                    {
                        Debug.Log("hit" + hit.ToString());
                        set.Add(new Vector2(x, y));
                        break;
                    }
                }
                for (int y = (int)screenPoint.y; y >= 0; y--)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100000, layer_mask))
                    {
                        Debug.Log("hit" + hit.ToString());
                        set.Add(new Vector2(x, y));
                        break;
                    }
                }
            }

            if (set.Count > 0) {
                topPos = (int)set[0].y;
                bottomPos = (int)set[0].y;
                rightPos = (int)set[0].x;
                leftPos = (int)set[0].x;
            }
            else if (set.Count > 1) {
                for (int inc = 1; inc < set.Count; inc++)
                {
                    topPos = (int) Mathf.Max(set[inc].y,topPos);
                    bottomPos = (int)Mathf.Min(set[inc].y, bottomPos);
                    rightPos = (int)Mathf.Max(set[inc].x, rightPos);
                    leftPos = (int)Mathf.Min(set[inc].x, leftPos);
                }
            }
            */
            /*
            //check top boundary
            for (var top = (int) screenPoint.y + 1; top <= cam.pixelHeight; top++)
            {     
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPoint.x,top,0));
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, 100000, layer_mask))
                {
                    Debug.Log("hit" + hit.ToString());
                    topPos = top;
                    break;
                }
                if (top == cam.pixelHeight)
                {
                    Debug.Log("hit" + hit.ToString());
                    topPos = cam.pixelHeight;
                }
            }

            //check bottom boundary
            for (var bottom = (int)screenPoint.y - 1; bottom >= 0; bottom--)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPoint.x, bottom, 0));
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, 100000, layer_mask))
                {
                    bottomPos = bottom;
                    break;
                }
                if (bottom == 0)
                    bottomPos = 0;
            }

            //Check Right Boundary
            for (var right = (int)screenPoint.x + 1; right <= cam.pixelWidth; right++)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPoint.x, right, 0));
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, 100000, layer_mask))
                {
                    rightPos = right;
                    break;
                }
                if (right == cam.pixelHeight)
                    rightPos = cam.pixelWidth;
            }

            //Check Left Boundary
            for (var left = (int)screenPoint.x - 1; left>= 0; left--)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPoint.x, left, 0));
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, 100000, layer_mask))
                {
                    leftPos = left;
                    break;
                }
                if (left == cam.pixelHeight)
                    leftPos = cam.pixelWidth;
            }
            */

            if ((new Rect(0, 0, Screen.width, Screen.height)).Contains(screenPoint) && screenPoint.z > 0){

                //posText.Add(screenPoint.x + ";" + screenPoint.y + ";" + screenPoint.z);
                posText.Add(topPos + ";" + bottomPos + ";" + rightPos + ";" + leftPos);;
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