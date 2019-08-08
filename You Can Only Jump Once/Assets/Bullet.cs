using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask groundLayer;

    public float collisionRadius = 1.0f;

    float elapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        //TimerManager.instance.AddTimer(DestroySelf, 10);
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime * GameManager.instance.speed;
        if (elapsed > 10)
            DestroySelf();

        if (Physics2D.OverlapCircle((Vector2)transform.position, collisionRadius, groundLayer))
            DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
