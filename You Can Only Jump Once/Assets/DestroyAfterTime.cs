using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    float elapsed = 0;
    public float duration = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime * GameManager.instance.speed;

        if (elapsed > duration)
            Destroy(gameObject);
    }
}
