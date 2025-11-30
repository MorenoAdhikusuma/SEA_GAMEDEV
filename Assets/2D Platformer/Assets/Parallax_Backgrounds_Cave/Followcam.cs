using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform cam;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - cam.position;
    }

    void LateUpdate()
    {
        transform.position = cam.position + offset;
    }
}

