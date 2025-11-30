using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam; //Main Camera
    Vector3 camStartPos;
    // float distance; // This only tracks X distance, let's track both distances

    // NEW: Vector2 to hold both X and Y distance from start
    Vector2 camDistance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++) //find the farthest background
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++) //set the speed of bacground
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void LateUpdate()
    {
        // STEP 1: Calculate the total distance the camera has moved on both axes
        camDistance.x = cam.position.x - camStartPos.x;
        camDistance.y = cam.position.y - camStartPos.y; // NEW: Calculate Y distance

        // STEP 2: Update the parent's position to follow the camera on BOTH axes
        // Note: The Z should remain 0 for 2D. 
        transform.position = new Vector3(cam.position.x, cam.position.y, 0); // CHANGE: Used to lock Y, now it follows cam.y

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;

            // STEP 3: Apply texture offset using both X and Y distance
            // camDistance.x moves the texture horizontally, camDistance.y moves it vertically
            mat[i].SetTextureOffset("_MainTex", camDistance * speed); // CHANGE: Passed Vector2 camDistance
        }
    }
}