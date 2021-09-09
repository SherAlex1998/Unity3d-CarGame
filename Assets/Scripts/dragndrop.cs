using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class dragndrop : MonoBehaviour
{
    public GameObject MC;
    private Vector3 offset;
    public Camera cam;
    private void Start()
    {
        cam = MC.GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) MC.GetComponent<SmoothCamera>().enabled = false;
         else if (Input.GetMouseButtonUp(0))
        {
            MC.GetComponent<SmoothCamera>().enabled = true;
        }
    }
    void OnMouseDown()
    {   
        offset = gameObject.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
    }
   
    void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        transform.position = cam.ScreenToWorldPoint(newPosition) + offset;
    }
}

