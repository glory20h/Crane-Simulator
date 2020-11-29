using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    //public GameObject Truck;
    //BoxCollider box;

    float timer;
    float timeCount;
    bool measure;

    void Start()
    {
        //box = GetComponent<BoxCollider>();
        //Debug.Log("detectionBox.center = " + box.center);
        //Debug.Log("detectionBox.size = " + box.size);
        //Truck.transform.position = transform.position + new Vector3(box.center.x + (box.size.x / 2), box.center.y + (box.size.y / 2), box.center.z + (box.size.z / 2));
        timer = 1f;
        timeCount = 0f;
        measure = true;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Input.GetAxis('Horizontal')" + Input.GetAxis("Horizontal"));
            timer = 1f;
        }

        /*
        if (Input.GetKey("w") && measure)
        {
            timeCount += Time.deltaTime;
            
        }
        */
    }
}
