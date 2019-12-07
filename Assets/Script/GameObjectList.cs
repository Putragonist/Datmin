using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectList : MonoBehaviour
{
    public CameraSetting cs;
    public List<GameObject> bottleTypes1;

    public List<GameObject> otherTypes;

    public List<GameObject> spawnGameObject;

    public GameObject point;

    private Queue<GameObject> queueSpawn = new Queue<GameObject>();

    public int randomCountMin = 0;
    public int randomCountMax = 5;

    public int randomMinObjectType = 1;
    public int randomMaxObjectType = 20;

    public float radiusMax = 25;

    public float timeSpawn = 10;
    private float lastTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
        NewObjectSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        //if(spawnGameObject.Count > 0)
          //  point = spawnGameObject[Random.Range(0, spawnGameObject.Count)];

        if(Time.time - lastTime > timeSpawn)
        {
            NewObjectSpawn();
            lastTime = Time.time;
        }


    }

    public void NewObjectSpawn()
    {
        foreach(GameObject go in spawnGameObject)
        {            
            Destroy(go);
        }
        spawnGameObject.Clear();

        for (int i = 0; i < Random.Range(randomCountMin, randomCountMax); i++)
        {
            int oval = Random.Range(0, 2);
            GameObject go = go = Instantiate(bottleTypes1[Random.Range(0, bottleTypes1.Count)]);


            float posX = Random.Range(-radiusMax / 2, radiusMax / 2);
            float posY = Random.Range(5 , 10);
            float posZ = Random.Range(-radiusMax / 2, radiusMax / 2);
            go.transform.position = new Vector3(posX, posY, posZ);
            
            spawnGameObject.Add(go);
        }

        if(spawnGameObject.Count > 0)
            cs.point = spawnGameObject[Random.Range(0, spawnGameObject.Count)];

        for(int i = 0; i < Random.Range(randomMinObjectType, randomMaxObjectType); i++)
        {
            GameObject go = Instantiate(otherTypes[Random.Range(0, otherTypes.Count)]);
            float posX = Random.Range(-radiusMax / 2, radiusMax / 2);
            float posY = Random.Range(5, 10);
            float posZ = Random.Range(-radiusMax / 2, radiusMax / 2);
            go.transform.position = new Vector3(posX, posY, posZ);

            spawnGameObject.Add(go);
        }

        point = spawnGameObject[Random.Range(0, spawnGameObject.Count)];
       // Debug.Log(point.name);
    }
}
