using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class HintToPaper : MonoBehaviour
{
    public static int idToSend = -1;
    public static string hintName;
    public static bool addHint;
    public Animator[] gaudians;

    private int id = -1;
    private List<Transform> allPaper = new List<Transform>();
    private bool[] haveHint = new bool[16];


    void Start()
    {
        Transform transform = this.transform;
        foreach (Transform child in transform)
        {
            allPaper.Add(child);
        }
        for (int i = 0; i < 16; i++)
        {
            //print(allPaper[i]);
            //print(haveHint[i]);
        }
    }


    void Update()
    {
        
    }

    // normal method for master scene triggered by button click
    public void OnClickPaperFloat()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        hintName = button.name;
        //button.GetComponent<Image>().color = new Color(181f, 115f, 115f);
        id = button.GetComponent<ItemInfo>().id - 20;
        idToSend = id;
        if (haveHint[id] == true)
        {
            addHint = false;
            button.GetComponent<Image>().color = Color.white;
            //allPaper[id].gameObject.GetComponent<Animator>().SetTrigger("TriggerFu");
            allPaper[id].position -= new Vector3(0f, 1.5f, 0f);
            haveHint[id] = false;
        }
        else
        {
            addHint = true;
            button.GetComponent<Image>().color = new Color(255f/255f, 192f/255f, 192f/255f);
            haveHint[id] = true;
            allPaper[id].gameObject.GetComponent<Animator>().SetTrigger("TriggerFu");
            allPaper[id].position += new Vector3(0f, 1.5f, 0f);
            // play anim of gaudian
            playGaudianAnim();
        }
    }

    // overloading method for hider and seeker client when there is a hint sent from Master
    public void OnClickPaperFloat(int _id)
    {
        if (haveHint[_id] == true)
        {
            allPaper[_id].position -= new Vector3(0f, 1.5f, 0f);
            haveHint[_id] = false;
        }
        else
        {
            haveHint[_id] = true;
            // allPaper[id].gameObject.GetComponent<Animator>().SetTrigger("TriggerFu");
            allPaper[_id].position += new Vector3(0f, 1.5f, 0f);
            // play anim of gaudian
            playGaudianAnim();
        }
    }

    public void ResetPaper()
    {
        foreach (Transform paper in allPaper)
        {

        }
    }

    private void playGaudianAnim()
    {
        foreach (Animator anim in gaudians)
        {
            anim.SetTrigger("hint");
        }
    }
}
