using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform hiderLoc;
    public Material lineMat;
    private GameObject prevLine = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawLine(transform.position, hiderLoc.position + new Vector3(0,-2,0));
    }

    void drawLine(Vector3 start, Vector3 end)
    {
        if (prevLine != null)
            GameObject.Destroy(prevLine, 0.1f);

        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = lineMat;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        prevLine = myLine;
    }
}
