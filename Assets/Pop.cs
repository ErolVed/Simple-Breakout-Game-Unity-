using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop : MonoBehaviour
{
    public Material mat01; //the numbers in the names represent block's hp. For example when hp = 3, the block will use material mat03.
    public Material mat02;
    public Material mat03;
    public Material mat04;
    public Material mat05;
    public Material mat06; 

    public int maxHP; //Maximum hp of the block which is set in Block's inspector.
    int HP; //Current HP

    void Start()
    {
        HP = maxHP; //Setting hp at start
        SetMat(HP); //Setting material of current HP
    }
    private void OnTriggerEnter(Collider other) //Triggers are used because every physics action is handled from scripts.
    {
        if (other.tag.Equals("Ball")) //Ball hitting Block
        {

            HP -= 1; //Ball hits Block. Block is damaged by 1.
            if (HP == 0)
            {
                Death(); //Block dies if hp is 0.
            }
            else
            {
                SetMat(HP); //Changes material if it is still alive.
            }
        }
    }
    void Death() //Death of a block is destruction of its Game Object. If you want to change this. Make sure to change score script. That script uses counting Game Objects. If # of game objects doesn't change, then the script won't know if you breaked a block or not.
    {
        Destroy(this.gameObject);
    }
    void SetMat(int hp) //Sets materials according to block's hp. This can be handled better by using names of the materials but it is not mandatory.
    {
        if (hp == 6)
        {
            this.GetComponent<Renderer>().material = mat06;
        }
        else if (hp == 5)
        {
            this.GetComponent<Renderer>().material = mat05;
        }
        else if (hp == 4)
        {
            this.GetComponent<Renderer>().material = mat04;
        }
        else if (hp == 3)
        {
            this.GetComponent<Renderer>().material = mat03;
        }
        else if(hp == 2)
        {
            this.GetComponent<Renderer>().material = mat02;
        }
        else if (hp == 1)
        {
            this.GetComponent<Renderer>().material = mat01;
        }
    }
}
