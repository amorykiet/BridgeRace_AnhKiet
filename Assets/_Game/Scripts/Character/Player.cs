using Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Player : Character
{
    //In Player
    public FixedJoystick joyStick;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed;


    private float eulerDirection = 0;
    private RaycastHit standingHit;
    private bool grounded;
    private RaycastHit hitBrickOnStair;
    private bool canMoveUp = true;


    private void Update()
    {
        Debug.DrawLine(TF.position, TF.position + Vector3.down * 0.5f, Color.red);
        
        //Stop move after win
        if (stopMovement)
        {
            return;
        }
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
        if (stopMovement)
        {
            return;
        }
        MoveWithJoyStick();
    }

    override protected void CollideWinPos()
    {
        //Go to win pos
        tf.position = winPos.position;
        tf.rotation = winPos.rotation;
        ClearBrick();
        ChangeAnim("dance");
        Stop();
        base.OnWin(this);
    }

    override protected void CollideDoor(Collider other)
    {

        Door door = other.GetComponent<Door>();
        if (currentStageIndex < door.stageToOpenIndex)
        {
            currentStageIndex = door.stageToOpenIndex;
            base.OnOpenDoor(this, currentStageIndex, myColor);
        }
    }

    override public void Stop()
    {
        stopMovement = true;
    }

    override public void OnInit(ColorType colorType)
    {
        myColor = colorType;
        skinnedMeshRenderer.material = colorData.GetMat(myColor);
        currentStageIndex = 0;
        stopMovement = false;
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
        Vector3 direction = new Vector3(joyStick.Direction.x, 0, joyStick.Direction.y).normalized;
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
        if (direction.magnitude > 0.001f)
        {
            //anim set continous
            ChangeAnim("run");
            eulerDirection = Vector2.SignedAngle(joyStick.Direction, Vector2.up);
        }
        else
        {
            ChangeAnim("idle");
        }

        TF.rotation = Quaternion.Euler(0, eulerDirection, 0);

    }



}
