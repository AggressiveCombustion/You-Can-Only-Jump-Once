using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool on = true;

    public bool toggle = true;

    public float onTime = 1.0f;
    public float offTime = 1.0f;

    public float elapsed = 0.0f;

    public float delay = 0;
    public float delayElapsed = 0;

    public LineRenderer lr;
    public Transform startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(delay > 0)
        {
            delayElapsed += Time.deltaTime * GameManager.instance.speed;
        }

        if(delayElapsed >= delay && toggle)
        {
            elapsed += Time.deltaTime * GameManager.instance.speed;
        }

        if(on)
        {
            
            if (elapsed > onTime)
            {
                elapsed = 0;
                on = false;
            }
        }

        else
        {
            if(elapsed > offTime)
            {
                elapsed = 0;
                on = true;
            }
        }

        if(on)
        {
            lr.enabled = true;
            RaycastHit2D hit = Physics2D.Raycast(startPos.position, -transform.up);
            Vector2 endPoint = hit.point;

            if(hit.transform.name == "Player")
            {
                FindObjectOfType<PlayerController>().Hit();
                SFXManager.instance.PlaySFX(FindObjectOfType<PlayerController>().laserHit);
                on = false;
                toggle = false;
            }

            lr.SetPosition(0, startPos.position);
            lr.SetPosition(1, endPoint);
            
        }

        else
        {
            lr.enabled = false;
        }
    }
}
