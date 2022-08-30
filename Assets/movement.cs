using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody rb; //rigidbody of this object.
    public Actions acts; //Control actions. For more information, research "Unity Input System".
    public float accTimeLimit = 5; //After this much time(in seconds), Pong will stop accelerating.
    public float speed; //Default speed. Set in Inspector.
    public float acceleration; //Default acceleration. Set in Inspector.
    public float dirX; //X-axis direction of Pong 
    float cTime;
    bool stopWatchOn = false;
    private void Awake() // A lot of New Input System functions which I am trying to figure out. If you are not curious enough, just ignore them and use my code. For more information, check https://www.youtube.com/watch?v=Pzd8NhcRzVo. I believe there are other ways to use Input System so research more.
    {
        acts = new Actions();
        acts.Movement.HorizontalMovement.performed += ctx => Move(ctx.ReadValue<float>());
        acts.Movement.HorizontalMovement.canceled += ctx => Move(ctx.ReadValue<float>());
    }
    private void Update()
    {
        StopWatch(); //this function works when it is in Update()
        rb.velocity = (speed + acceleration * cTime) * new Vector3(dirX, 0, 0); //DirX is set with controls. Speed calculation is as follows: Vcurrent = Vinitial + a * t (V is velocity(in this context it is only speed), a is acceleration, t is time)
    }
    void Move(float dir) //Function to set DirX and Start stopwatch.
    {
        if(dir == 0) //If no input or both inputs(left and right):
        {
            stopWatchOn = false; //Movement didn't start so no need to calculate acceleration
            cTime = 0; //Should be zero if there is no movement
        }
        else// There is an input. There is a movement.
        {
            stopWatchOn = true; //Start the stop watch to calculate acceleration. We started moving.
        }
        dirX = dir; //Setting dirX
    }
    private void OnEnable()//New Input System functions
    {
        acts.Enable();
    }
    private void OnDisable()//New Input System functions
    {
        acts.Disable();
    }
    void StopWatch() //StopWatch function
    {
        if (stopWatchOn)//If this boolean is true. Stop watch starts.
        {
            if (cTime < accTimeLimit) //If it is smaller than the limit, stop watch continues.
            {
                cTime += Time.deltaTime; //This function works in Update() so, Update() is called in every delta time. 
            }
            else // We are over or on the limit. So, current time should stop on the limit. This causes acceleration to be at max.
            {
                cTime = accTimeLimit; //Setting time to limit.
            }
        }
    }
}
