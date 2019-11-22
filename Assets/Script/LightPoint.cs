using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint : MonoBehaviour
{
    UnityEngine.Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<UnityEngine.Light>();
        GetComponent<UnityEngine.Light>().intensity = Random.Range(0, 1);
        GetComponent<UnityEngine.Light>().areaSize = new Vector2(Random.Range(1, 20), Random.Range(1, 20));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
