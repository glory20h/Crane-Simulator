using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    public Trailer trailer;
    // Start is called before the first frame update
    void Start()
    {
        trailer.JointTrailerTruckON();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
