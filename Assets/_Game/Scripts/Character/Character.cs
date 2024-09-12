using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public static event Action<Character> onWin;
    public static event Action<Character ,int , ColorType> onOpenDoor;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected ColorData colorData;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] protected ColorType myColor;
    [SerializeField] protected Transform stackOffset;
    [SerializeField] protected Animator animator;

    protected Stack<GameObject> BrickStack = new();
    protected int currentStageIndex = 0;
    protected string currentAnim;
    protected bool stopMovement = false;
    protected Transform tf;

    public Transform winPos;
    public LayerMask brickOnStairMask;
    public int BrickCollected => BrickStack.Count;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    abstract public void OnInit(ColorType colorType);
    abstract public void Stop();

    public void GoToPos(Transform target)
    {
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(0,180,0);
    }

    protected void AddBrick()
    {
        Brick brick = SimplePool.Spawn<Brick>(PoolType.Brick, Vector3.up * 0.5f * BrickStack.Count, Quaternion.identity, stackOffset);
        brick.OnInit(myColor);
        BrickStack.Push(brick.gameObject);
    }

    protected void RemoveBrick()
    {
        Brick brick = BrickStack.Pop().GetComponent<Brick>();
        brick.OnDespawn();
    }

    public void ClearBrick()
    {
        while (BrickCollected > 0)
        {
            BrickStack.Pop().GetComponent<Brick>().OnDespawn();
        }
    }

    protected void CollideBrick(Collider other)
    {
        Brick target = other.GetComponent<Brick>();
        if (myColor == target.BrickColor)
        {
            target.OnDespawnToSpawn();
            AddBrick();
        }

    }

    protected void OnWin(Character character)
    {
        onWin?.Invoke(character);
    }

    protected void OnOpenDoor(Character character, int currentStageIndex, ColorType myColor)
    {
        onOpenDoor?.Invoke(character, currentStageIndex, myColor);
    }

    abstract protected void CollideDoor(Collider other);

    abstract protected void CollideWinPos();

    protected void OnTriggerEnter(Collider other)
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

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Winpos"))
        {
            collision.collider.enabled = false;
            CollideWinPos();
        }
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

    public void Dance()
    {
        ChangeAnim("dance");
    }
}
