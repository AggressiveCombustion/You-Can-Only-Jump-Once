using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Transform[] options;
    int index = 0;

    float prevH = 0;
    float prevV = 0;
    bool canH = true;
    bool canV = true;
    float confirmTime = 0;
    float vTime = 0;
    float hTime = 0;

    public AudioClip move;
    public AudioClip confirm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i < options.Length; i++)
        {
            if (index == i)
            {
                options[i].localScale = new Vector3(1.1f, 1.1f, 1);
            }
            else
                options[i].localScale = new Vector3(0.8f, 0.8f, 1);
        }

        options[1].GetComponent<Text>().text = "SFX: " + (SFXManager.instance.sfxOn ? "ON" : "OFF");
        options[2].GetComponent<Text>().text = "BGM: " + (BGMManager.instance.bgmOn ? "ON" : "OFF");

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(h!=0 || v != 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if(!canV)
        {
            if (v > -0.1f && v < 0.1f)
            {
                canV = true;
                vTime = 0;
            }

            if(prevV == v)
            {
                vTime += Time.deltaTime;

                if(vTime > 0.25f)
                {
                    canV = true;
                    vTime = 0;
                }
            }
        }

        if (!canH)
        {
            if (h > -0.1f && h < 0.1f)
            {
                canH = true;
                hTime = 0;
            }

            if (prevH == h)
            {
                hTime += Time.deltaTime;

                if( hTime > 0.25f)
                {
                    canH = true;
                    hTime = 0;
                }
            }
        }

        if (canV && v < -0.1f)
            Down();
        if (canV && v > 0.1f)
            Up();
        if (canH && h < -0.1f)
            Left();
        if (canH && h > 0.1f)
            Right();
        if (Input.GetButtonDown("Jump"))
            Confirm();

        prevH = h;
        prevV = v;
    }

    void Down()
    {
        index += 1;
        if (index > options.Length - 1)
            index = 0;
        canV = false;

        SFXManager.instance.PlaySFX(move);
    }

    void Up()
    {
        index -= 1;
        if (index < 0)
            index = options.Length - 1;
        canV = false;

        SFXManager.instance.PlaySFX(move);
    }

    void Left()
    {
        if(index == 1 || index == 2)
        {
            Confirm();
            canH = false;
        }
    }

    void Right()
    {
        if (index == 1 || index == 2)
        {
            Confirm();
            canH = false;
        }
    }

    public void Confirm()
    {
        SFXManager.instance.PlaySFX(confirm);
        switch (index)
        {
            // resume
            case 0:
                GameManager.instance.UnpauseGame();
                break;
            // sfx off
            case 1:
                SFXManager.instance.sfxOn = !SFXManager.instance.sfxOn;
                break;
            // bgm off
            case 2:
                BGMManager.instance.bgmOn = !BGMManager.instance.bgmOn;
                break;
            // quit game
            case 3:
                Application.Quit();
                break;
        }
    }

    public void SetIndex(int i)
    {
        index = i;
        SFXManager.instance.PlaySFX(move);
    }
}
