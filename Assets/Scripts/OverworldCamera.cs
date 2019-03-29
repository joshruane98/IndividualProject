using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        offset = new Vector3(0, 3, -6);
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset;

        transform.LookAt(target);
    }
}
