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
            int max_y = (int) screenPoint.y;
            int min_y = (int)screenPoint.y;
            int min_x = (int)screenPoint.x;
            int max_x = (int)screenPoint.x;

            //List for getting max and min value
            List<int> ys_max = new List<int>();
            List<int> ys_min = new List<int>();
            List<int> xs_max = new List<int>();
            List<int> xs_min = new List<int>();

            //Get every bound of object
            foreach(Renderer r in go.GetComponentsInChildren<Renderer>())
            {
                Mesh m = r.GetComponent<MeshFilter>().mesh;  //Dapatkan mesh objek             
                int vc = m.vertices.Length; //jumlah vertex pada objek
                for(int v = 0; v < vc; v++)
                {
                    Bounds bounds = new Bounds(r.gameObject.transform.TransformPoint(m.vertices[v]), Vector3.zero);
                    Vector3 meshLock = r.gameObject.transform.TransformPoint(m.vertices[v]);
                    ys_max.Add((int)cam.WorldToScreenPoint(bounds.max).y);
                    ys_min.Add((int)cam.WorldToScreenPoint(bounds.min).y);
                    xs_max.Add((int)cam.WorldToScreenPoint(bounds.max).x);
                    xs_min.Add((int)cam.WorldToScreenPoint(bounds.min).x);
                }
            }

            //get max y, min y, max x and min x in list
            max_y = Mathf.Max(ys_max.ToArray());
            min_y = Mathf.Min(ys_min.ToArray());
            max_x = Mathf.Max(xs_max.ToArray());
            min_x = Mathf.Min(xs_min.ToArray());

            //sin 0 y position in unity is at the bottom, convert it to y in pixel
            float temph = (Screen.height - max_y);
            float tempw = (Screen.height - min_y);

            //get coordinate for each bounding box edge
            float right = Mathf.Max(max_x, min_x);
            float left = Mathf.Min(min_x, max_x);
            float top = Mathf.Min(temph, tempw);
            float bottom = Mathf.Max(tempw, temph);

            //Post it in dataset
            if ((new Rect(0, 0, Screen.width, Screen.height)).Contains(screenPoint) && screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.z > 0){

                if (top > 0 && left > 0 && right > 0 && bottom > 0 &&
                    top >= 0 && bottom <= Screen.height && left >= 0 && right <= Screen.width &&
                    (right-left > 100 || bottom - top > 100)
                    )
                {
                    List<Vector2> bv = new List<Vector2>();
                    bv.Add(new Vector2(left + (right - left) / 2, Screen.height - top + (bottom - top) / 2));
                    bv.Add(new Vector2(right - (right - left) / 4, Screen.height - bottom - (bottom - top) / 4));
                    bv.Add(new Vector2(left + (right - left) / 4, Screen.height - top + (bottom - top) / 4));

                    foreach (Vector2 v in bv)
                    {
                        //hide objek yang menghalangi kamera dari objek botol
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

                    posText.Add(left + ";" + top + ";" + right + ";" + bottom);
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