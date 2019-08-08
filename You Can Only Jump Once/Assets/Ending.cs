using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void End()
    {
        Debug.Log("shutting down");
        GameObject.Find("Fade").GetComponent<Animator>().SetTrigger("solid");
        TimerManager.instance.AddTimer(QuitGame, 3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
