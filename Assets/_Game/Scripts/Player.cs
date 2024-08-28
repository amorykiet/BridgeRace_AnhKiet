using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private ColorData colorData;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private ColorType myColor;
    public ColorType ColorType => myColor;

    private float eulerDirection = 0;


    //Test
    private void Start()
    {
        OnInit(ColorType.Blue);
    }

    private void Update()
    {
        //Movement
        rb.velocity = new Vector3(joyStick.Direction.x, rb.velocity.y, joyStick.Direction.y) * speed;

        if (joyStick.Direction.magnitude > 0.001f)
        {
            eulerDirection = Vector2.SignedAngle(joyStick.Direction, Vector2.up);
        }
        transform.rotation = Quaternion.Euler(0, eulerDirection , 0);

        
    }

    public void OnInit(ColorType colorType)
    {
        myColor = colorType;
        skinnedMeshRenderer.material = colorData.GetMat(myColor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Brick"))
        {
            Brick target = collision.collider.GetComponent<Brick>();
            if (myColor == target.BrickColor)
            {
                Debug.Log("Collected");
                target.OnDespawn();
            }
        }
    }
}
