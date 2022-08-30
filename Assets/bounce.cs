using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{
    public Rigidbody brb; //Rigid Body of Ball, Ball Rigid Body(BRB)
    public float speed = 10; //Speed of ball
    public float bds= 120; // Bounce Degree Sensitivity of Pong

    bool started = true; //Boolean to make an action at the first Update() loop.
    void Update()
    {
        if (started) //First Update() loop
        {
            brb.velocity = speed * new Vector3((float)0, (float)-1, 0); // Ball is set to go down at start
            started = false; //Set started to false so,  it doesn't enter again.
        }
    }
    private void OnTriggerEnter(Collider other) //Trigger is used because every physics action is controlled by scripts(mostly algorithms that I figured out).
    {
        if (other.tag.Equals("Pong")) //Ball hitting Pong:
        {
            brb.velocity = speed * DirCal(brb.position.x, other.transform.position.x, other.transform.localScale.x); //Setting velocity. Speed is default speed we set from Inpsector of Ball. Direction is the direction we set with DirCal() algorithm. For more information, check line 64.
        }
        else if (other.tag.Equals("Wall")) //Ball hitting Walls
        {
            List<float> list = findFace(brb.velocity, brb.position - other.transform.position, other.transform.lossyScale, brb.transform.lossyScale);
            float face = list[0];
            //brb.position = new Vector3(list[3], list[4], 0) + other.transform.position; //Teleports to correct position, the position where it needs to be if ontrigger wasn't late.  It is not used because teleporting does not look good. However, this is more accurate physics-wise.
            if (face == 1 || face == 0) //Up and Down
            {
                brb.velocity = Vector3.Scale(brb.velocity, new Vector3(1, -1, 0));
            }
            else if (face == 3 || face == 2) //Right and Left
            {
                brb.velocity = Vector3.Scale(brb.velocity, new Vector3(-1, 1, 0));
            }
            else
            {
                Debug.Log(face);
            }
        }
        else if (other.tag.Equals("Block"))
        {
            List<float> list = findFace(brb.velocity, brb.position - other.transform.position, other.transform.lossyScale, brb.transform.lossyScale); //For more information, check line 72.
            float face = list[0]; //Setting face variable to calculated face from the findFace() algorithm
            //brb.position = new Vector3(list[3], list[4], 0) + other.transform.position; //Teleports to correct position, the position where it needs to be if OnTrigger wasn't late.  It looks bad because it is not smooth. Note: OnTrigger() is delayed so speed of the ball is faster than OnTrigger(). Make sure to check it out. Someday they might fix this.
            if (face == 1 || face == 0) //Up and Down Note: Physics of bouncing from upside vs downside is the same. So, we only need to know up/down or left/right.
            {
                brb.velocity = Vector3.Scale(brb.velocity, new Vector3(1, -1, 0)); //Flipping velocity vector across x-axis. It might feel weird to flip it across x-axis but don't forget to include directions.
            }
            else if (face== 3 || face == 2) //Right and Left Note: Pyhsics of bouncing from leftside vs rightside is the same. So, we only need to know up/down or left/right.
            {
                brb.velocity = Vector3.Scale(brb.velocity, new Vector3(-1, 1, 0)); //Flipping velocity vector across y-axis. It might feel weird to flip it across y-axis but don't forget to include directions.
            }
            else
            {
                Debug.Log(face); //Error code. Check findFace() to debug the problem.
            }
        }
    }
    //An algorithm I used to calculate the direction of ball after hitting the Pong. I can't explain it without visuals. You can make your own algorithm, use this algorithm without knowing how does it work or ask me to explain it.
    Vector3 DirCal(float ballx, float blockx, float blockLen) //Calculates the direction of the ball after hitting Pong.
    {
        float x = ballx - blockx;
        float y = -bds/blockLen * x + 90;
        float yrad = y * Mathf.Deg2Rad;
        return Vector3.Normalize(new Vector3(1/Mathf.Tan(yrad), 1, 0));
    }
    //An algorithm I made to detect which face of the block/wall the ball hits. It is too complicated to just explain it with comments. Visual explanation is mandatory. So, if enough people wants an explanation, I will make a visual(video or images) guide.
    List<float> findFace(Vector3 vel, Vector3 pos, Vector3 scale,Vector3 sc) //vel: velocity of ball, pos: position of ball relative to object hit, scale: scale of the object hit, sc: scale of the ball, (face: 0:down,1:up,2:left,3:right) , output: (face,m,n,x,y)
    {
        float face,m=0, n=0, x=0, y=0;
        List<float> output = new List<float>();
        if (vel.x == 0)
        {
            if (vel.y > 0)
            {
                face = 0; //down
            }
            else if (vel.y < 0)
            {
                face = 1; //up
            }
            else
            {
                face =  (float)0.1;
            }
        }
        else if(vel.y == 0)
        {
            if (vel.x > 0)
            {
                face = 2; //left
            }
            else if (vel.x < 0)
            {
                face = 3; //right
            }
            else
            {
                face = (float)0.2;
            }
        }
        else
        {
            m = vel.y / vel.x;
            n = pos.y - vel.y / vel.x * pos.x;
            if(vel.x>0 && vel.y > 0)
            {
                y = -m * (sc.x+scale.x) / 2 + n; //Left
                x = (-(sc.y+scale.y) / 2 - n) / m; //Down
                if(y< (sc.y + scale.y) / 2 && y > -(sc.y+scale.y) / 2)
                {
                    x = -(sc.x + scale.x) / 2;
                    face = 2; //left
                }
                else if(x < (sc.x + scale.x) / 2 && x > -(sc.x + scale.x) / 2)
                {
                    y = -(sc.y + scale.y) / 2;
                    face = 0; //down
                }
                else
                {
                    face = (float)0.3;
                }
            }
            else if(vel.x>0 && vel.y < 0)
            {
                y = -m * (sc.x + scale.x) / 2 + n; //Left
                x = ((sc.y + scale.y) / 2 - n) / m; //Up
                if (y < (sc.y + scale.y) / 2 && y > -(sc.y + scale.y) / 2)
                {
                    x = -(sc.x + scale.x) / 2;
                    face = 2; //left
                }
                else if (x < (sc.x + scale.x) / 2 && x > -(sc.x+ scale.x) / 2)
                {
                    y = (sc.y + scale.y) / 2;
                    face = 1; //up
                }
                else
                {
                    face = (float)0.4;
                }
            }
            else if(vel.x<0 && vel.y > 0)
            {
                y = m * (sc.x + scale.x) / 2 + n; //Right
                x = (-(sc.y + scale.y) / 2 - n) / m; //Down
                if (y < (sc.y + scale.y) / 2 && y > -(sc.y + scale.y) / 2)
                {
                    x = (sc.x + scale.x) / 2;
                    face = 3; //right
                }
                else if (x < (sc.x + scale.x) / 2 && x > -(sc.x + scale.x) / 2)
                {
                    y = -(sc.y + scale.y) / 2;
                    face = 0; //down
                }
                else
                {
                    face = (float)0.5;
                }
            }
            else if(vel.x<0 && vel.y < 0)
            {
                y = m * (sc.x + scale.x) / 2 + n; //Right
                x = ((sc.y + scale.y) / 2 - n) / m; //Up
                if (y < (sc.y + scale.y) / 2 && y > -(sc.y + scale.y) / 2)
                {
                    x = (sc.x + scale.x) / 2;
                    face = 3; //right
                }
                else if (x < (sc.x + scale.x) / 2 && x > -(sc.x + scale.x) / 2)
                {
                    y = (sc.y + scale.y) / 2;
                    face = 1; //up
                }
                else
                {
                    face = (float)0.6;
                }
            }
            else
            {
                face = (float)0.7;
            }
        }
        output.Add(face);
        output.Add(m);
        output.Add(n);
        output.Add(x);
        output.Add(y);
        return output;
    }

}
