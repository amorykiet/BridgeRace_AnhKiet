using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class Bot : MonoBehaviour
{

    public static event Action<int> onBotOpenDoor;

    public NavMeshAgent agent;
    public Transform winPos;
    public LayerMask brickOnStairMask;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private ColorData colorData;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private ColorType myColor;
    [SerializeField] private Transform stackOffset;
    [SerializeField] private Animator animator;
    [SerializeField] private Level currentLevel;

    public Level CurrentLevel { 
        get { return currentLevel; } 
        set { currentLevel = value; }
    }

    public ColorType ColorType => myColor;
    public int BrickCollected => BrickStack.Count;

    private Stack<GameObject> BrickStack = new Stack<GameObject>();
    private int currentStageIndex = 0;
    private string currentAnim;
    private bool stopMovement = false;
    private BaseState<Bot> currentState;
    private float timeToBuild;


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
        if (stopMovement) return;
        if (currentState == null) return;
        currentState.OnExcute(this);
    }

    public void OnInit(ColorType colorType)
    {
        myColor = colorType;
        skinnedMeshRenderer.material = colorData.GetMat(myColor);
        currentStageIndex = 0;
        
        stopMovement = false;
        ChangeState(new IdleState());
    }

    public void ChangeAnim(string anim)
    {
        if (currentAnim != null)
        {
            animator.ResetTrigger(currentAnim);
        }
        currentAnim = anim;
        animator.SetTrigger(currentAnim);

    }
    public void ChangeState(BaseState<Bot> newState)
    {
        if (currentState != null) currentState.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public void StandOnBrickOnBridge(RaycastHit hitBrickOnStair)
    {
        BrickOnStair brick = hitBrickOnStair.collider.GetComponent<BrickOnStair>();

        if (!brick.IsSameColor(myColor))
        {
            if (BrickCollected > 0)
            {
                brick.ChangeColor(myColor);
                RemoveBrick();
            }
            else
            {
                ChangeState(new PatrolState());
            }
        }


    }

    public Stage GetStage()
    {
        return currentLevel.GetStage(currentStageIndex);
    }

    private void AddBrick()
    {
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, Vector3.up * 0.5f * BrickCollected, Quaternion.identity, stackOffset);
        brick.OnInit(myColor);
        BrickStack.Push(brick.gameObject);
    }

    private void RemoveBrick()
    {
        Brick brick = BrickStack.Pop().GetComponent<Brick>();
        brick.OnDespawn();
    }

    private void ClearBrick()
    {
        while (BrickCollected > 0)
        {
            BrickStack.Pop().GetComponent<Brick>().OnDespawn();
        }
    }

    private void CollideBrick(Collider other)
    {
        Brick target = other.GetComponent<Brick>();
        if (myColor == target.BrickColor)
        {
            target.OnDespawn();
            AddBrick();
        }

    }

    private void CollideDoor(Collider other)
    {
        Door door = other.GetComponent<Door>();
        if(currentStageIndex < door.stageToOpenIndex)
        {
            currentStageIndex = door.stageToOpenIndex;
            onBotOpenDoor?.Invoke(currentStageIndex);
            ChangeState(new PatrolState());
        }
    }

    private void CollideWinPos()
    {

        ClearBrick();
        ChangeAnim("dance");
        stopMovement = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            CollideBrick(other);
        }
        else if (other.CompareTag("Door"))
        {
            CollideDoor(other);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Winpos"))
        {
            CollideWinPos();
        }
    }


}
