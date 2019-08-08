using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    public bool bgmOn = true;

    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(source.isPlaying && !bgmOn)
        {
            source.Pause();
        }

        if(source.isPlaying & bgmOn)
        {
            source.volume = 0.15f;
        }

        if(!source.isPlaying && bgmOn)
        {
            source.Play();
            source.volume = 0.15f;
        }
    }
}
