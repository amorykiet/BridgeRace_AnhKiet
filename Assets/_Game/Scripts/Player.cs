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
    public ColorType ColorType => myColor;
    
    private float eulerDirection = 0;
    private Stack<GameObject> BrickStack = new Stack<GameObject>();

    //Test
    private void Start()
    {
        OnInit(myColor);
    }

    private void FixedUpdate()
    {

        //Movement

        //Vector3 direction = Vector3.forward * joyStick.Vertical + Vector3.right * joyStick.Horizontal;
        //rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        if (joyStick.Direction.magnitude > 0.001f)
        {
            rb.velocity = new Vector3(joyStick.Direction.x, 0, joyStick.Direction.y) * speed;
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
