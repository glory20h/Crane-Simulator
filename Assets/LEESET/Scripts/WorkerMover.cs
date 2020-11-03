using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerMover : MonoBehaviour
{
    public float moveSpeed = 0.2f;

    void Update()
    {
        transform.position += new Vector3(Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.Deg2Rad)) * Time.deltaTime * moveSpeed;
    }
}
