using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class SeekerUI : MonoBehaviour
{
    public GameObject UI;
    public Text paperLeft;
    public AnimationController AC;
    public Text victoryUI;
    public GameObject preistWin;
    public GameObject ghostWin;
    public PlayableDirector preistwin_timeline;
    public PlayableDirector ghostwin_timeline;

    public static bool hasEvictUI;
    public static int selctionleft = 5;
    public static int hasWon = 0;    // 0 for ongoing, 1 for seekerWin, 2 for seekerLose

    private bool needUpdate;
    public static int _evictID = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && needUpdate)
        {
            GameObject.Find("Seeker(Clone)").GetComponentInChildren<Animator>().SetTrigger("evict");
            _evictID = SeekController.evictID;
            hasEvictUI = false;
            selctionleft--;
            paperLeft.text = "Paper left to evict: " + selctionleft.ToString();
            UI.SetActive(false);
            AC.PlayAnimation(_evictID, true);
            needUpdate = false;
            print("selected");
            if (_evictID == SeekerClient.possessionNum && SeekerClient.possessionNum != -1)
            {
                preistwin_timeline.Play();
                victoryUI.gameObject.SetActive(true);
                hasWon = 1;
                //StartCoroutine(beforeEnding());
                //preistWin.SetActive(true);
                Invoke("PriestWin", 10f);
            }
        }
    }

    private void LateUpdate()
    {
        if (selctionleft <= 0 && hasWon != 1)
        {
            hasWon = 2;
            victoryUI.gameObject.SetActive(true);
            victoryUI.text = "You Lose :(";
            StartCoroutine(beforeEnding());
            ghostwin_timeline.Play();
            Invoke("GhostWin", 10f);
        }
    }

    public void UpdateSeekerUI()
    {
        needUpdate = true;
        UI.SetActive(true);
        hasEvictUI = true;
        Invoke("DisableSeekerUI", 3f);
        
    }

    private void DisableSeekerUI()
    {
        needUpdate = false;
        UI.SetActive(false);
        hasEvictUI = false;
    }

    IEnumerator beforeEnding()
    {
        yield return new WaitForSeconds(4f);
    }

    private void GhostWin()
    {
        ghostWin.SetActive(true);
    }

    private void PriestWin()
    {
        preistWin.SetActive(true);
    }

    private void PreistWinTimeline()
    { 
    }
}
