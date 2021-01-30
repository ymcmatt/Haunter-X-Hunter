using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PossessController : MonoBehaviour
{
    public Camera cam;
    public float maxHeight;
    public float minHeight;
    public float possessCD;
    private bool canPossess = true;
    public bool hiderActive = true;
    public static int possessID = -1;
    public static int interactID = -1;
    public static bool hasInteracted;
    public static bool hasOptionMenu;
    public static bool hasPossessed;
    public static float sliderVal;
    public static int possessChance = 3;
    public static int _evictID = -1;
    public GameObject optionsUI;
    private float interactCD;
    public GameObject evictUI;
    public Text possessLeft;

    private GameObject selectedObject;
    private bool canInteract;
    private bool startCD;
    private List<int> hasEvicted = new List<int>();
    private int selectedObjectID = -1;
    private float timeCount;
    public AnimationController AC;

    public GameObject UICenter;
    public GameObject UICenterPossess;

    public Image cdImage;
    // Start is called before the first frame update
    void Start()
    {
        cdImage.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        possessLeft.text = possessChance.ToString();
        if (_evictID != -1)
        {
            hasEvicted.Add(_evictID);
            _evictID = -1;
        }
        if (timeCount > 3)
        {
            timeCount = 0;
            selectedObjectID = -1;
            canPossess = true;
        }
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("pocessable"))
            {
                // set the center UI
                UICenter.SetActive(false);
                UICenterPossess.SetActive(true);

                if (Input.GetMouseButtonDown(0) && canPossess)
                {
                    selectedObject = hit.transform.gameObject;
                    selectedObjectID = selectedObject.GetComponent<ItemInfo>().id;
                    timeCount += Time.deltaTime;
                    if (hasEvicted.Contains(selectedObjectID))
                    {
                        EvictedUI();
                    }
                    else
                    {
                        print("options");
                        canPossess = false;
                        // cd for possess
                        //Invoke("resumePossess", possessCD);
                        // possess and send to master / seeker               
                        optionsUI.SetActive(true);
                        hasOptionMenu = true;
                    }
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            interactCD = 0f;
            sliderVal = 0f;
            startCD = true;
            Interact();
            Invoke("resumePossess", possessCD);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //startCD = true;
            canPossess = true;
            possess();
        }
        if (startCD)
        {
            print("should start cd");
            sliderVal = interactCD;
            cdImage.fillAmount = interactCD;
            interactCD += Time.deltaTime / 10;
            if (interactCD > 1)
            {
                interactCD = 1f;
                sliderVal = 1f;
                startCD = false;
                cdImage.fillAmount = interactCD;
            }
        }
        canInteract = true;
    }

    private void EvictedUI()
    {
        evictUI.SetActive(true);
        StartCoroutine(countDown(2f));
        evictUI.SetActive(false);
    }

    public void Interact()
    {
        print("interact");
        hasInteracted = true;
        interactID = selectedObject.GetComponent<ItemInfo>().id;
        optionsUI.SetActive(false);
        print(interactID);
        AC.PlayAnimation(interactID);
        hasOptionMenu = false;
        // hasInteracted = false;
    }

    public void possess()
    {
        if (possessChance > 0)
        {
            possessChance--;
            print("process");
            hasPossessed = true;
            int id = selectedObject.GetComponentInParent<ItemInfo>().id;
            possessID = id;
            hasInteracted = true;
            interactID = selectedObject.GetComponent<ItemInfo>().id;
            AC.PlayAnimation(interactID);
            // set hider model unactive
            if (GameObject.Find("Hider(Clone)") && hiderActive)
            {
                GameObject.Find("Hider(Clone)").transform.localScale = Vector3.zero;
                hiderActive = false;
            }

            // move to the possess object
            cam.transform.position = selectedObject.transform.position;
            // special case for wardrobe
            if (possessID == 11)
                cam.transform.position = new Vector3(5, 4.5f, -8.5f);
            // clamp the height
            float height = Mathf.Clamp(cam.transform.position.y, minHeight, maxHeight);
            cam.transform.position = new Vector3(cam.transform.position.x, height, cam.transform.position.z);
            cam.transform.LookAt(Vector3.zero);
            optionsUI.SetActive(false);
            hasOptionMenu = false;
        }
    }

    void resumePossess()
    {
        canPossess = true;
    }

    IEnumerator countDown(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
