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
        return Input.GetAxis(axisName);
    }

    public bool GetKeyDown(string name)
    {
        return Input.GetKeyDown(name);
    }
}
