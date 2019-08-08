using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    public bool pressed = false;

    public LayerMask bulletLayer;

    public float collisionRadius = 1.0f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    private Color debugColor = Color.red;

    public GameObject anim;

    public Interactable target;

    public AudioClip activateSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position, collisionRadius, bulletLayer))
        {
            Debug.Log("Hit");
            if(!pressed)
            {
                target.Activate();
                pressed = true;
                SFXManager.instance.PlaySFX(activateSound, true);
            }
        }

        anim.GetComponent<Animator>().SetBool("pressed", pressed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        //var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius);
    }
}
