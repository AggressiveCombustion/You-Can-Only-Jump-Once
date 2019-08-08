using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string text = "";

        if(player.canJump)
        {
            text += "You can only jump once.";
        }

        if (player.canDash)
        {
            text += "\n\nYou can only dash once.";
        }

        if (player.canShoot)
        {
            text += "\n\nYou can only shoot once.";
        }

        if (player.canWalljump)
        {
            text += "\n\nYou can only walljump once.";
        }

        if (player.canGetHit)
        {
            text += "\n\nYou can only get hit once.";
        }

        if (player.canStopTime)
        {
            text += "\n\nYou can only stop time once.";
        }

        if (player.canDie)
        {
            text += "\n\nYou can only die once.";
        }

        GetComponent<Text>().text = text;
    }
}
