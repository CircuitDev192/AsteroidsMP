using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfOffScreen : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        if (!cam)
        {
            Debug.LogError("Main Camera missing :: Start() :: PlayerController.cs");
        }
    }

    void FixedUpdate()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(this.transform.position);

        if (screenPos.x < 0f)
        {
            this.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width, screenPos.y, screenPos.z));
        }
        else if (screenPos.x > Screen.width)
        {
            this.transform.position = cam.ScreenToWorldPoint(new Vector3(0, screenPos.y, screenPos.z));
        }

        if (screenPos.y < 0f)
        {
            this.transform.position = cam.ScreenToWorldPoint(new Vector3(screenPos.x, Screen.height, screenPos.z));
        }
        else if (screenPos.y > Screen.height)
        {
            this.transform.position = cam.ScreenToWorldPoint(new Vector3(screenPos.x, 0, screenPos.z));
        }
    }
}
