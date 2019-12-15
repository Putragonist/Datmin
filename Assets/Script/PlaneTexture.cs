using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTexture : MonoBehaviour
{

    public List<Texture> texture;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //code random posisi texture
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetTexture("_MainTex",texture[Random.Range(0, texture.Count)]);
    }
}
