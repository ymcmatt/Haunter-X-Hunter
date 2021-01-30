using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetSeekerHint : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    // Start is called before the first frame update
    void Start()
    {
        hintText.SetText("Hint text here");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
