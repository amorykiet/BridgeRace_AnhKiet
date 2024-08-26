using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    private float eulerDirection = 0;
    


    private void Update()
    {
        //Moving
        rb.velocity = new Vector3(joyStick.Direction.x, rb.velocity.y, joyStick.Direction.y) * speed;

        if (joyStick.Direction.magnitude > 0.001f)
        {
            eulerDirection = Vector2.SignedAngle(joyStick.Direction, Vector2.up);
        }
        transform.rotation = Quaternion.Euler(0, eulerDirection , 0);
    }
}
