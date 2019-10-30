using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAreaSpawner : MonoBehaviour
{
    public GameObject itemToSpread;

    int numItemToSpawn = 30;

    public float itemXSpread = 10;
    public float itemYSpread = 0;
    public float itemZSpread = 10;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numItemToSpawn; i++)
        {
            SpreadItems();
        }
    }

    void SpreadItems()
    {
        Vector3 randomPostion = new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread)) + transform.position;
        GameObject clone = Instantiate(itemToSpread, randomPostion, Quaternion.identity);
    }

   
}
