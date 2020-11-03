using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Axel
{
    Front,
    Rear
}

[Serializable]
public class Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

public class ForkliftController : MonoBehaviour
{
    /* Variables for Car Controlling */
    [SerializeField]
    private float maxAcceleration = 20.0f;
    [SerializeField]
    private float turnSensitivity = 1.0f;
    [SerializeField]
    private float maxSteerAngle = 45.0f;
    [SerializeField]
    private List<Wheel> wheels;

    public float speed;

    private float inputX, inputY;
    private Rigidbody _rb;
    /* Variables for Car Controlling */

    Transform fork;
    Transform chainRollers;
    Transform forkMechanism;

    Material chainMat;

    const float forkMaxUp = 2f;
    float forkMaxDown;

    public CustomInput customInput;

    void Start()
    {
        //Search children based on MeshFilter components (they all have it)
        foreach (var mf in GetComponentsInChildren<MeshFilter>())
        {
            //Find fork
            if (mf.name.Equals("Fork"))
            {
                fork = mf.transform;
                forkMaxDown = fork.transform.localPosition.z;
            }
            //Find fork mechanism, when found, store Chain material
            if (mf.name.Equals("Fork_Mechanism"))
            {
                forkMechanism = mf.transform;
                Renderer r = mf.GetComponent<Renderer>();
                foreach (var m in r.materials)
                {
                    if (m.name.Contains("Chain"))
                    {
                        chainMat = m;
                    }
                }
            }
            //Rollers
            if (mf.name.Equals("Chain_Rollers"))
            {
                chainRollers = mf.transform;
            }
        }

    }

    void FixedUpdate()
    {
        //Move fork up and down on local axis, set offset for chain material, rotate the rollers
        if (Input.GetKey(KeyCode.I))
        {
            if (fork.transform.localPosition.z <= forkMaxUp)
            {
                fork.transform.localPosition += Vector3.forward * Time.deltaTime;
                chainMat.mainTextureOffset = new Vector2(chainMat.mainTextureOffset.x - Time.deltaTime, chainMat.mainTextureOffset.y);
                chainRollers.Rotate(Vector3.right * 6);
            }
        }
        if (Input.GetKey(KeyCode.K))
        {
            if (fork.transform.localPosition.z >= forkMaxDown)
            {
                fork.transform.localPosition -= Vector3.forward * Time.deltaTime;
                chainMat.mainTextureOffset = new Vector2(chainMat.mainTextureOffset.x + Time.deltaTime, chainMat.mainTextureOffset.y);
                chainRollers.Rotate(-Vector3.right * 6);
            }
        }

        //Tilt the mechanism
        /* Don't need this for nows
        if (Input.GetKey(bendMechanismIn))
        {
            if (forkMechanism.localEulerAngles.x < 275f)
            {
                forkMechanism.Rotate(Vector3.right * Time.deltaTime * 2);
            }

        }
        if (Input.GetKey(bendMechanismOut))
        {
            if (forkMechanism.localEulerAngles.x > 270f)
            {
                forkMechanism.Rotate(-Vector3.right * Time.deltaTime * 2);
            }
        }
        */
    }

    void Update()
    {
        AnimateWheels();
        GetInputs();
    }

    void LateUpdate()
    {
        Move();
        Turn();
    }

    void GetInputs()
    {
        //inputX = Input.GetAxis("Forklift_Hor");
        //inputY = Input.GetAxis("Forklift_Ver");
        inputX = customInput.GetAxis("Forklift_Hor");
        inputY = customInput.GetAxis("Forklift_Ver");
    }

    void Move()
    {
        foreach(var wheel in wheels)
        {
            wheel.collider.motorTorque = inputY * maxAcceleration * speed * Time.deltaTime;
        }
    }

    void Turn()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                var _steerAngle = inputX * turnSensitivity * maxSteerAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, _steerAngle, 0.5f);
            }
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheel.collider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;
        }
    }
}