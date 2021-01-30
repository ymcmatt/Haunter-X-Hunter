using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitAfterEnding : MonoBehaviour
{
    public GameObject preistWinVid;
    public GameObject ghostWinVid;
    public GameObject quitUI;

    bool escapeOnce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (preistWinVid.activeSelf || ghostWinVid.activeSelf)
        {
            Invoke("QuitGame", 29f);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitUI.SetActive(true);
            escapeOnce = true;
            Invoke("BackToGame", 2f);
        }
    }

    private void LateUpdate()
    {
        if (escapeOnce && Input.GetKeyDown(KeyCode.Return))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
        print("quit");
        Application.Quit();
    }

    void BackToGame()
    {
        print("QQQ");
        escapeOnce = false;
        quitUI.SetActive(false);
    }
}
