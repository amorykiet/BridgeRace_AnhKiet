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
    [SerializeField] private LayerMask brickOnStairMask;


    public ColorType ColorType => myColor;

    private float eulerDirection = 0;
    private Stack<GameObject> BrickStack = new Stack<GameObject>();
    private RaycastHit standingHit;
    private bool grounded;
    private RaycastHit hitBrickOnStair;
    private bool canMoveUp = true;

    private Transform tf;
    public Transform TF
    {
        get
        {
            //tf = tf ?? gameObject.transform;
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    //Test
    private void Start()
    {
        OnInit(myColor);
    }

    private void Update()
    {
        Debug.DrawLine(TF.position, TF.position + Vector3.down * 0.5f, Color.red);
        
        //Change Drag
        grounded = Physics.Raycast(TF.position, Vector3.down, 0.3f, groundMask);
        
        if (grounded)
        {
            rb.drag = 5f;
        }
        else
        {
            rb.drag = 0f;
        }

        //Collide brick on Stair
        if(Physics.Raycast(TF.position, Vector3.forward, out hitBrickOnStair, 0.5f, brickOnStairMask))
        {
            StandOnBrickOnBridge();
        }
    }

    private void FixedUpdate()
    {
        MoveWithJoyStick();

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
        if (Physics.Raycast(TF.position, Vector3.down, out standingHit, 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, standingHit.normal);
            if (angle > 0.0001f && angle < 50.0f)
            {
                return true;
            }

        }
        return false;
    }

    private void StandOnBrickOnBridge()
    {
        BrickOnStair brick = hitBrickOnStair.collider.GetComponent<BrickOnStair>();
        bool isMoveForward = Vector2.Angle(joyStick.Direction, Vector2.up) < 90f;

        if (!isMoveForward || brick.IsSameColor(myColor))
        {
            if (!canMoveUp) canMoveUp = true;
            return;
        }

        if (!brick.IsSameColor(myColor))
        {
            if (BrickStack.Count > 0)
            {
                brick.ChangeColor(myColor);
                RemoveBrick();
            }
            else
            {
                canMoveUp = false;
            }
        }


    }

    private void MoveWithJoyStick()
    {
        //Movement
        Vector3 direction = new Vector3(joyStick.Direction.x, 0, joyStick.Direction.y).normalized; ;
        if (grounded)
        {
            if (IsOnSlope())
            {
                //Move on Slope
                if (!canMoveUp && direction.z > 0)
                {
                    direction.z = 0;
                }
                direction = Vector3.ProjectOnPlane(direction, standingHit.normal).normalized;
            }

            rb.AddForce(direction * speed, ForceMode.Force);
        }

        //Rotation
        if (joyStick.Direction.magnitude > 0.001f)
        {
            eulerDirection = Vector2.SignedAngle(joyStick.Direction, Vector2.up);
        }

        TF.rotation = Quaternion.Euler(0, eulerDirection, 0);

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
