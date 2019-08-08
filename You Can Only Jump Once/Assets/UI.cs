using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject jump;
    public GameObject dash;
    public GameObject shoot;
    public GameObject wallJump;
    public GameObject getHit;
    public GameObject stopTime;
    public GameObject die;
    public GameObject toldYa;

    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        jump.SetActive(pc.canJump);
        dash.SetActive(pc.canDash);
        shoot.SetActive(pc.canShoot);
        wallJump.SetActive(pc.canWalljump);
        getHit.SetActive(pc.canGetHit);
        stopTime.SetActive(pc.canStopTime);
        die.SetActive(pc.canDie);

        //toldYa.SetActive(pc.dead);
    }
}
