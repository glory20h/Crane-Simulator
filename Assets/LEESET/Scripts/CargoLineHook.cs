using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for attaching cargo and hook
/// Press 'E' to connect/unconnect hook to cargo
/// </summary>
public class CargoLineHook : MonoBehaviour
{
    public Transform pointHookRef;
    public Rigidbody hookRigid;
    public Transform Hook;
    GameObject pointHook;
    public Transform pointHookCargo;
    public Transform pointCargo1;
    public Transform pointCargo2;
    public Transform pointCargo3;
    public Transform pointCargo4;
    public Material matLineRen;

    //bool onKey = false;
    bool cargoCapture_Bool = true;
    bool lineHook_Bool = false;

    void Start()
    {
        cargoCapture_Bool = true;
        pointHook = Hook.GetChild(0).gameObject;
    }

    void Update()
    {
        Hook.position = pointHookRef.position + new Vector3(0, -0.1f, -0.02f);
        pointHook.transform.position = pointHookRef.position + new Vector3(0, -0.1f, -0.02f);

        if (lineHook_Bool == true)
        {
            LineHook();
        }

        /*
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("E Pressed!");
            Debug.Log("cargoCapture_Bool : " + cargoCapture_Bool);
            //Debug.Log("onKey : " + onKey);
        }
        */

        if (Input.GetKeyDown("e") && cargoCapture_Bool == true/*&& onKey == true*/)
        {
            FixedJoint cargo = gameObject.AddComponent<FixedJoint>();
            cargo.GetComponent<FixedJoint>().connectedBody = hookRigid;
            lineHook_Bool = true;
            pointHook.AddComponent<LineRenderer>();
            pointHook.GetComponent<LineRenderer>().startWidth = 0.02f;
            pointHook.GetComponent<LineRenderer>().endWidth = 0.02f;
            cargoCapture_Bool = false;
        }
        else if (Input.GetKeyDown("e") && cargoCapture_Bool == false/*&& onKey == true*/)
        {
            FixedJoint cargo = gameObject.GetComponent<FixedJoint>();
            Destroy(cargo.GetComponent<FixedJoint>());
            lineHook_Bool = false;
            cargoCapture_Bool = true;
            LineHookOff();
        }
    }

    /*
    void OnTriggerEnter(Collider hook)
    {
        //Can Press the 'Hook' Image to hook cargo to crane
        if (hook.tag == "Hook")
        {
            //imageHook.enabled = true;
            onKey = true;
        }
    }

    void OnTriggerExit(Collider hook)
    {
        if (hook.tag == "Hook")
        {
            //imageHook.enabled = false;
            onKey = false;
        }
    }
    */

    public void LineHook()
    {
        Vector3[] lineHookCargo = new Vector3[8];
        lineHookCargo[0] = new Vector3(pointHookCargo.position.x, pointHookCargo.position.y, pointHookCargo.position.z);
        lineHookCargo[1] = new Vector3(pointCargo1.position.x, pointCargo1.position.y, pointCargo1.position.z);
        lineHookCargo[2] = new Vector3(pointHookCargo.position.x, pointHookCargo.position.y, pointHookCargo.position.z);
        lineHookCargo[3] = new Vector3(pointCargo2.position.x, pointCargo2.position.y, pointCargo2.position.z);
        lineHookCargo[4] = new Vector3(pointHookCargo.position.x, pointHookCargo.position.y, pointHookCargo.position.z);
        lineHookCargo[5] = new Vector3(pointCargo3.position.x, pointCargo3.position.y, pointCargo3.position.z);
        lineHookCargo[6] = new Vector3(pointHookCargo.position.x, pointHookCargo.position.y, pointHookCargo.position.z);
        lineHookCargo[7] = new Vector3(pointCargo4.position.x, pointCargo4.position.y, pointCargo4.position.z);
        pointHook.GetComponent<LineRenderer>().positionCount = lineHookCargo.Length;
        pointHook.GetComponent<LineRenderer>().SetPositions(lineHookCargo);
        pointHook.GetComponent<LineRenderer>().material = matLineRen;
    }

    public void LineHookOff()
    {
        Destroy(pointHook.GetComponent<LineRenderer>());
    }
}
