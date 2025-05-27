using UnityEngine;

public class PortalCameraFollow : MonoBehaviour
{
    public Transform cameraFollow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraFollow.transform.position;
        transform.rotation = Quaternion.Euler(cameraFollow.rotation.eulerAngles.x, 0, 0);

    }
}
