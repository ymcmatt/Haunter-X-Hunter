﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHider()
    {
        SceneManager.LoadScene("HiderNew");
    }

    public void LoadSeeker()
    {
        SceneManager.LoadScene("SeekerNew");
    }
}
