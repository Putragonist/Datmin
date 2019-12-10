using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Screenshot : MonoBehaviour
{
    public CameraSetting cs;
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

                //int screenX = UnityEngine.Random.Range(480, 1921);
              //  int screenY = UnityEngine.Random.Range(480, 1921);
                //Screen.SetResolution(screenX, screenY, false);
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
        if (GetComponent<CameraSetting>().point == null)
            return;
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
<<<<<<< Updated upstream
                topList.Add((int)cam.WorldToScreenPoint(r.bounds.max).y);
                bottomList.Add((int)cam.WorldToScreenPoint(r.bounds.min).y);
                rightList.Add((int)cam.WorldToScreenPoint(r.bounds.max).x);
                leftList.Add((int)cam.WorldToScreenPoint(r.bounds.min).x);
=======

                Mesh m = r.GetComponent<MeshFilter>().mesh;
               
                int vc = m.vertices.Length;
                //Bounds bounds = new Bounds();
                for(int v = 0; v < vc; v++)
                {
                    //if (v == 0)
                    //{
                    //    bounds = new Bounds(r.gameObject.transform.transformpoint(m.vertices[v]), vector3.zero);
                    //}
                    //else
                    //{
                    //    bounds.Encapsulate(r.transform.TransformPoint(m.vertices[v]));
                    //}

                    Bounds bounds = new Bounds(r.gameObject.transform.TransformPoint(m.vertices[v]), Vector3.zero);
                    Vector3 meshLock = r.gameObject.transform.TransformPoint(m.vertices[v]);
                    topList.Add((int)cam.WorldToScreenPoint(bounds.max).y);
                    bottomList.Add((int)cam.WorldToScreenPoint(bounds.min).y);
                    rightList.Add((int)cam.WorldToScreenPoint(bounds.max).x);
                    leftList.Add((int)cam.WorldToScreenPoint(bounds.min).x);
                }

                //for (int l = 0; l < bounds.; l++)
                //{
                //    topList.Add((int)cam.WorldToScreenPoint(bounds.max).y);
                //    bottomList.Add((int)cam.WorldToScreenPoint(bounds.min).y);
                //    rightList.Add((int)cam.WorldToScreenPoint(bounds.max).x);
                //    leftList.Add((int)cam.WorldToScreenPoint(bounds.min).x);
                //}

                //Vector3 cen = r.bounds.center;
                //Vector3 ext = r.bounds.extents;
                //Vector2[] extentPoints = new Vector2[8]
                //{
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
                //    HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
                //};
                //Vector2 min = extentPoints[0];
                //Vector2 max = extentPoints[0];
                //foreach (Vector2 v in extentPoints)
                //{
                //    min = Vector2.Min(min, v);
                //    max = Vector2.Max(max, v);
                //}

                //topList.Add((int)max.y);
                //bottomList.Add((int)(min).y);
                //rightList.Add((int)(max).x);
                //leftList.Add((int)(min).x);
>>>>>>> Stashed changes
            }

            //update position value
            topPos = Mathf.Max(topList.ToArray());
            bottomPos = Mathf.Min(bottomList.ToArray());
            rightPos = Mathf.Max(rightList.ToArray());
            leftPos = Mathf.Min(leftList.ToArray());
            float temph = (Screen.height - topPos);
            float tempw = (Screen.height - bottomPos);

            //float tempr = (Screen.width - rightPos);
            //float templ = (Screen.width - leftPos);


            float rl = Mathf.Max(rightPos, leftPos);
            float lf = Mathf.Min(leftPos, rightPos);
            float hi = Mathf.Min(temph, tempw);
            float bo = Mathf.Max(tempw, temph);

            //Post it in dataset
            if ((new Rect(0, 0, Screen.width, Screen.height)).Contains(screenPoint) && screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.z > 0){

                if (hi > 0 && lf > 0 && rl > 0 && bo > 0 &&
                    hi >= 0 && bo <= Screen.height && lf >= 0 && rl <= Screen.width &&
                    (rl-lf > 100 || bo - hi > 100)
                    )
                {
                    List<Vector2> bv = new List<Vector2>();
                    bv.Add(new Vector2(lf + (rl - lf) / 2, Screen.height - hi + (bo - hi) / 2));
                    bv.Add(new Vector2(rl - (rl - lf) / 4, Screen.height - bo - (bo - hi) / 4));
                    bv.Add(new Vector2(lf + (rl - lf) / 4, Screen.height - hi + (bo - hi) / 4));

                    foreach (Vector2 v in bv)
                    {
                        RaycastHit hit;
                        Ray ray = GetComponent<Camera>().ScreenPointToRay(v);

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.tag.ToLower() != "bottle" &&
                                hit.transform.gameObject.tag.ToLower() != "maincamera" &&
                                hit.transform.gameObject.tag.ToLower() != "ground")
                            {
                                hit.transform.gameObject.SetActive(false);
                            }
                        }
                    }

                    posText.Add(lf + ";" + hi + ";" + rl + ";" + bo);
                }
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
            //i++;
            i = UnityEngine.Random.Range(0, 9999);
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
                    sw.WriteLine("image_location;left;top;right;bottom");
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