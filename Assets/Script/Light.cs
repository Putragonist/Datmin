using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1)), Random.Range(0, 360));
        transform.localEulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }
}
