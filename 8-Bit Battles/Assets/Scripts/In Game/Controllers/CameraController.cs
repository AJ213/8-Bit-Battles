using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*
    [SerializeField] float zoomLevel;
    float minCamZoom;
    float maxCamZoom;
    float sensitivity;

    float camXValue;
    float camYValue;
    [SerializeField] float maxCamXValue;
    [SerializeField] float minCamXValue;
    [SerializeField] float maxCamYValue;
    [SerializeField] float minCamYValue;
    
    void Start()
    {
        SetCamLimits();  
    }

    bool cameraInLocation = false;
    void Update()
    {
        if(Camera.main.transform.localPosition == new Vector3(25, 25, -20) || cameraInLocation == true)
        {
            UserInputForCamZoom();
            UserInputForCamMovement();
            AdjustCameraPositon();
            cameraInLocation = true;
        }
        else
        {
            Camera.main.transform.localPosition = new Vector3(25, 25, -20);
        }
    }

    void SetCamLimits()
    {
        if (GameObject.Find("Value Carrier").GetComponent<ExtraValues>().mapNum > 0 || GameObject.Find("Value Carrier").GetComponent<ExtraValues>().mapNum < 4)
        {
            minCamZoom = 3f;
            maxCamZoom = 7f;
            zoomLevel = 5.5f;
            sensitivity = 5f;
            SetMaxAndMinOfCamZoom();
            camYValue = 0.12f;
            camXValue = 0.2f;
        }
    }

    void UserInputForCamMovement()
    {
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.position += new Vector3(0, camYValue, 0);
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.position += new Vector3(0, -camYValue, 0);
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.position += new Vector3(-camXValue, 0, 0);
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.position += new Vector3(camXValue, 0, 0);
        }
    }
    void UserInputForCamZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoomLevel -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            zoomLevel = Mathf.Clamp(zoomLevel, minCamZoom, maxCamZoom);
            Camera.main.orthographicSize = zoomLevel;
        }
    }

    void AdjustCameraPositon()
    {
        SetMaxAndMinOfCamZoom();
        while (Camera.main.transform.position.x > maxCamXValue)
        {
            Camera.main.transform.position += new Vector3(-0.01f, 0, 0);
        }
        while (Camera.main.transform.position.x < minCamXValue)
        {
            Camera.main.transform.position += new Vector3(0.01f, 0, 0);
        }
        while (Camera.main.transform.position.y > maxCamYValue)
        {
            Camera.main.transform.position += new Vector3(0, -0.01f, 0);
        }
        while (Camera.main.transform.position.y < minCamYValue)
        {
            Camera.main.transform.position += new Vector3(0, 0.01f, 0);
        }
    }
    void SetMaxAndMinOfCamZoom()
    {
        int zoomLevelInt = (int)(zoomLevel * 10);

        minCamYValue = zoomLevel;
        maxCamYValue = 50f - zoomLevel;

        minCamXValue = ((7.7f / 4f) * zoomLevel) + 0.02f;
        if (GameObject.Find("Value Carrier").GetComponent<ExtraValues>().mapNum > 0 || GameObject.Find("Value Carrier").GetComponent<ExtraValues>().mapNum < 4)
        {
            switch (zoomLevelInt)
            {
                case 30:
                    //minCamXValue = 9.58f;
                    maxCamXValue = 40.42f;
                    break;
                case 35:
                    //minCamXValue = 9.58f;
                    maxCamXValue = 40.42f;
                    break;
                case 40:
                    //minCamXValue = 9.58f;
                    maxCamXValue = 40.42f;
                    break;
                case 45:
                    //minCamXValue = 9.58f;
                    maxCamXValue = 40.42f;
                    break;
                case 50: //Done
                    // = 9.5f;
                    maxCamXValue = 40.42f;
                    break;
                case 55:
                    //minCamXValue = 10.5f;
                    maxCamXValue = 39.46f;
                    break;
                case 60:
                    //minCamXValue = 11.5f;
                    maxCamXValue = 40.42f;
                    break;
                case 65:
                    //minCamXValue = 9.58f;
                    maxCamXValue = 40.42f;
                    break;
                case 70:
                    //minCamXValue = 9.58f;
                    maxCamXValue = 40.42f;
                    break;
            }
            
        }
    }
    */
}
