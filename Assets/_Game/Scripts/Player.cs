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
    [SerializeField] private ColorType myColor;
    [SerializeField] private Transform stackOffset;
    [SerializeField] private LayerMask groundMask;


    public ColorType ColorType => myColor;

    private float eulerDirection = 0;
    private Stack<GameObject> BrickStack = new Stack<GameObject>();
    private RaycastHit standingHit;
    private bool grounded;

    //Test
    private void Start()
    {
        OnInit(myColor);
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.3f, groundMask);
        
        if (grounded)
        {
            rb.drag = 5f;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    private void FixedUpdate()
    {

        //Movement
        Vector3 direction = new Vector3(joyStick.Direction.x, 0, joyStick.Direction.y).normalized;
        if (grounded)
        {
            if (IsOnSlope())
            {
                direction = Vector3.ProjectOnPlane(direction, standingHit.normal);
            }

            rb.AddForce(direction * speed, ForceMode.Force);
        }        
        
        if (direction.magnitude > 0.001f)
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

    private void AddBrick()
    {
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, Vector3.up * 0.5f * BrickStack.Count, Quaternion.identity, stackOffset);
        brick.OnInit(myColor);
        BrickStack.Push(brick.gameObject);
    }

    private void RemoveBrick()
    {
        Brick brick = BrickStack.Pop().GetComponent<Brick>();
        brick.OnDespawn();
    }

    private bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out standingHit, 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, standingHit.normal);
            if (angle > 0.0001f && angle < 50.0f)
            {
                return true;
            }

        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Brick target = other.GetComponent<Brick>();
            if (myColor == target.BrickColor)
            {
                target.OnDespawn();
                AddBrick();
            }
        }

    }
}
