using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public GameObject cap;
    public Material bottleClear;
    public Material neckCap;
    public Material label;
    public List<Texture> labels;
    // Start is called before the first frame update
    void Start()
    {

        
        if(labels.Count > 0)
            label.SetTexture("_MainTex", labels[Random.Range(0, labels.Count)]);

        for (int i =0; i < GetComponent<Renderer>().materials.Length; i++)
        {
            Material tmp = null;
            if (GetComponent<Renderer>().materials[i].ToString().ToLower().Contains("bottleclear"))
            {
                tmp = new Material(bottleClear.shader);
                tmp.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.87f);
                Debug.Log(tmp.color);
                GetComponent<Renderer>().materials[i] = tmp;
            }
            if(GetComponent<Renderer>().materials[i].ToString().ToLower().Contains("capblue"))
            {
                tmp = new Material(neckCap.shader);
                tmp.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.87f);
                GetComponent<Renderer>().materials[i] = tmp;
                if(cap!= null)
                 cap.GetComponent<Renderer>().material = tmp;
            }
        }

        //Renderer rend = GetComponent<Renderer>();
        //rend.materials[0].color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        /*
        GameObject[] childs = transform.GetComponentsInChildren
        foreach(GameObject child in childs)
        {
            if (child.name.Contains("Cap"))
            {
                child.GetComponent<Renderer>().material.color = rend.materials[0].color;
            }
        }*/

        // rend.materials[1].color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 0.07f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
