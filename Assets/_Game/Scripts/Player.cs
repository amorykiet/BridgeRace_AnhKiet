using Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private FloatingJoystick joyStick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private ColorData colorData;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private ColorType myColor;
    [SerializeField] private Transform brickStackPos;
    public ColorType ColorType => myColor;
    
    private float eulerDirection = 0;
    private int bricksNum = 0;
    private Stack<GameObject> BrickStack = new Stack<GameObject>();

    //Test
    private void Start()
    {
        OnInit(myColor);
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

    private void AddBrick()
    {

    }

    private void RemoveBrick()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Brick target = other.GetComponent<Brick>();
            if (myColor == target.BrickColor)
            {
                Debug.Log("Collected");
                target.OnDespawn();
            }
        }

    }
}
