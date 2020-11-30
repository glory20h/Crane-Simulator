using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    float timer;

    void Start()
    {
        timer = -1f;
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public float GetAxis(string axisName)
    {
        if(axisName == "Forklift_Ver")  //Forklift move forward
        {
            if(timer < 12f)
            {
                return -0.3f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Truck_Ver")    //Truck move forward
        {
            if(timer > 12f)
            {
                return -1f;
            }
            else
            {
                return 0f;
            }
        }
        else if(axisName == "Horizontal")   //Crane move horizontally
        {
            if(timer > 1.5f && timer < 2f)
            {
                return -0.25f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Vertical")     //Crane move vertically
        {
            if (timer > 1f && timer < 4.2f)
            {
                return 0.75f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Vertical_hook")    //Lower/Higher Crane
        {
            if (timer > 0.75f && timer < 6f)
            {
                return 0.85f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else
        {
            return Input.GetAxis(axisName);
        }
    }

    public bool GetKeyDown(string name)
    {
        return Input.GetKeyDown(name);
    }
}
