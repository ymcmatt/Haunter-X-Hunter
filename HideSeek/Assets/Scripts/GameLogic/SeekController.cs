using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekController : MonoBehaviour
{
    public Camera cam;
    public Text paperLeft;
    private int selctionleft = 5;
    private int possessedId = -1;
    public static int evictID;
    public SeekerUI seekerUI;
    public GameObject UI;

    public GameObject UICenter;
    public GameObject UICenterPossess;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //paperLeft.text = selctionleft.ToString();
        possessedId = SeekerClient.possessionNum;
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit))
        {
            //print(hit.transform.gameObject);
            if (hit.transform.gameObject.CompareTag("pocessable"))
            {
                // set the center UI
                UICenter.SetActive(false);
                UICenterPossess.SetActive(true);
                if (Input.GetMouseButtonDown(0) && selctionleft > 0)
                {
                    evictID = hit.transform.gameObject.GetComponentInParent<ItemInfo>().id;
                    
                    // check if the chosen item is possessed
                    seekerUI.UpdateSeekerUI();
                }
            }
            else
            {
                // set the center UI
                UICenter.SetActive(true);
                UICenterPossess.SetActive(false);
            }

        }
        else
        {
            // set the center UI
            UICenter.SetActive(true);
            UICenterPossess.SetActive(false);
        }
    }
}
