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
            if(timer < 7f)
            {
                return -0.15f;
            }
            else if(timer < 20f)
            {
                return -0.2f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Forklift_Hor")
        {
            if(timer > 3f && timer < 4f)
            {
                return 0.3f;
            }
            else if(timer > 5f && timer < 7f)
            {
                return -0.3f;
            }
            else if(timer > 12f && timer < 14f)
            {
                return 0.15f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Truck_Ver")    //Truck move forward
        {
            if(timer > 9f)
            {
                return -0.1f;
            }
            else
            {
                return 0f;
            }
        }
        else if(axisName == "Truck_Hor")
        {
            return Input.GetAxis(axisName);
            if (timer > 14f)
            {
                return 0.3f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Horizontal")   //Crane move horizontally
        {
            if(timer > 1f && timer < 3.36f)
            {
                return 1f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Vertical")     //Crane move vertically
        {
            if (timer > 0.4f && timer < 2.64f)
            {
                return 1f;
            }
            else
            {
                return Input.GetAxis(axisName);
            }
        }
        else if(axisName == "Vertical_hook")    //Lower/Higher Crane
        {
            if (timer > 0.5f && timer < 5f)
            {
                return 1f;
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
