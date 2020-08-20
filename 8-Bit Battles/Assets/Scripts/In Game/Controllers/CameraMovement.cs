using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour {

    //Movement Variables
    public float cameraSpeed = 10.0f;

    Vector3 cameraPosition;
    Vector3 minClamp;
    Vector3 maxClamp;

    //Zoom Variables
    public float zoomSensitivity= 100.0f;
    public float zoomInLimit = 1.5f;
    public float zoomOutLimit = 4.0f;

    float cameraZoom;

    //Clamp Variables
    public GameObject activeTileMap;
    public Vector3 min;
    public Vector3 max;

    // Use this for initialization
    void Start () {
        if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 1)
        {
            activeTileMap = GameObject.Find("map 1");
        }
        else if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 2)
        {
            activeTileMap = GameObject.Find("map 2");
        }
        else if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 3)
        {
            activeTileMap = GameObject.Find("map 3");
        }
        else
        {
            Debug.Log("error no map selected");
        }


        //Initializes variables for Camera Movement
        cameraPosition = this.transform.position;

        //Checks to make sure camera is orthographic
        if(!this.GetComponent<Camera>().orthographic)
        {
            Debug.LogError("Camera is not Set to Orthographic!");
            return;
        }

        //Apply the camera zoom
        cameraZoom = this.GetComponent<Camera>().orthographicSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Calls the Clamp Camera Function / Method
        ClampCamera();

        //Calls the Move Camera Function / Method
        MoveCamera();

        //Call the Zoom Camera Function / Method
        ZoomCamera();

    }


    void MoveCamera()
    {
        // Calculates camera movement 
        cameraPosition.x = Mathf.Clamp(cameraPosition.x += Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, minClamp.x, maxClamp.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y += Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime, minClamp.y, maxClamp.y);

        //Applies camera movement
        this.transform.position = cameraPosition;
    }

    void ZoomCamera()
    {
        //Calculates Camera Zoom
        cameraZoom = Mathf.Clamp(cameraZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * Time.deltaTime, zoomInLimit, zoomOutLimit);

        //Applies Camera zoom
        this.GetComponent<Camera>().orthographicSize = cameraZoom;
    }

    //Public so this can be called from another sript or attached to a button if the screen needs to be resized at runtime
    public void ClampCamera()
    {

        min = activeTileMap.GetComponent<TilemapCollider2D>().bounds.center - activeTileMap.GetComponent<TilemapCollider2D>().bounds.extents;
        max = activeTileMap.GetComponent<TilemapCollider2D>().bounds.extents * 2;

        minClamp = min;
        maxClamp = max;

    }

}
