using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public AudioSource[] sources;

    public bool sfxOn = true;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(AudioSource source in sources)
        {
            if (sfxOn)
            {
                source.volume = 0.3f;
            }
            else
                source.volume = 0;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        PlaySFX(clip, false, false);
    }

    public void PlaySFX(AudioClip clip, bool priority)
    {
        PlaySFX(clip, priority, false);
    }

    public void PlaySFX(AudioClip clip, bool priority, bool looping)
    {
        AudioSource toUse = null;
        

        foreach(AudioSource source in sources)
        {
            if(!source.isPlaying)
            {
                toUse = source;
                break;
            }
        }

        if(toUse == null && priority)
        {
            // get the sfx that's closest to ending and replace it with the new one
            AudioSource furthestAlong = null;
            float percentage = 0;
            foreach(AudioSource source in sources)
            {
                if(source.time > percentage)
                {
                    furthestAlong = source;
                    percentage = source.time;
                }
            }

            toUse = furthestAlong;
        }

        if (toUse != null)
        {
            toUse.loop = false;
            if (looping)
                toUse.loop = true;

            toUse.clip = clip;
            toUse.Play();
        }
            
    }

    public void StopSFX(AudioClip clip)
    {
        foreach(AudioSource source in sources)
        {
            if (source.clip == clip)
                source.Stop();
        }
    }
}
