using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public bool onGround = false;
    public bool onLeftWall = false;
    public bool onRightWall = false;

    public LayerMask groundLayer;

    public float collisionRadius = 1.0f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugColor = Color.red;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        if(onGround)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
            //Debug.Log("AAAA");
            if(hit != false && hit.transform.tag == "movingPlatform")
            {
                //Debug.Log("BBBB");
                transform.parent = hit.transform;
            }
        }

        else
        {
            transform.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
