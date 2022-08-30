using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour

{
    public GameObject blocks; //Game Object that holds the blocks(bricks)
    public Text scoreTxt; //UI Text that shows the current record
    public Text winTxt; //UI Text that appears after you lost or won a game. Example: "You Win!"
    public GameObject ball; //Game Object of ball
    int score = 0; 
    int blockCount; // Integer value for # of blocks that is not destroyed. This value is used to check if you break a block or not. For More Information, check line 30.
    void Start()
    {
        scoreTxt.text = "Score: " + score; //Setting score board at start
        blockCount = blocks.transform.childCount; //blockCount is updated at start.
    }
    void Update()
    {
        if (ball.transform.position.y < -20 || ball.transform.position.y > 20) //Checked if ball is outside the game arena, which means you lost.
        {
            winTxt.GetComponent<Text>().text = "You LOSE!"; //Text that shows up when you lost
            winTxt.GetComponent<Text>().enabled = true; //Making UI text visible. Note: We disabled text component of UI text from Inspector. So, it starts disabled.
        }
        if (blocks.transform.childCount - blockCount != 0) // Block count is not updated so that means player breaked a block. Current block count - delayed block count != 0
        {
            score += 10; //How much score you get when you break a brick
            blockCount = blocks.transform.childCount; //delayed block count is equal to current block count until player breaks a block.
            scoreTxt.text = "Score: " + score; //Updating score board
            if (blockCount == 0) //No more blocks? Then, player won.
            {
                winTxt.GetComponent<Text>().text = "You WIN!"; //Message for winning the game
                winTxt.GetComponent<Text>().enabled = true; //Making UI text visible. Note: We disabled text component of UI text from Inspector. So, it starts disabled.
            }
        }
    }
}
