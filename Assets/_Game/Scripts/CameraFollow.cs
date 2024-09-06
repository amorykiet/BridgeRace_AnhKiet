using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float speed;

    private void Start()
    {
        target = transform;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (target == null) return;
        if (Vector3.Distance(target.position, transform.position) > 0.001f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.fixedDeltaTime * speed);
        }
    }

    public void FollowToTarget(Transform target)
    {
        this.target = target;
    }

}