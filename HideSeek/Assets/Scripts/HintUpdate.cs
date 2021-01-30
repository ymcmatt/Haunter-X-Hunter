using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintUpdate : MonoBehaviour
{
    public string theHint;
    public GameObject inputField;
    public GameObject textDisplay1;
    public GameObject textDisplay2;
    public GameObject textDisplay3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(theHint);
        theHint = inputField.GetComponent<TMP_InputField>().text;
        textDisplay1.GetComponent<TextMeshProUGUI>().text = "The hint is " + theHint;
        textDisplay2.GetComponent<TextMeshProUGUI>().text = "The hint is " + theHint;
        textDisplay3.GetComponent<TextMeshProUGUI>().text = "The hint is " + theHint;
    }
}
