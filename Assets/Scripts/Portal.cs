using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Portal : MonoBehaviour
{
    public Renderer renderer;
    public Portal partner;
    public Camera portalCam;
    private RenderTexture texture;
    public Camera viewCam;
    public bool useCamera = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        texture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        renderer = gameObject.GetComponent<Renderer>();
        partner.portalCam.targetTexture = texture;
        renderer.material.SetTexture(0, texture);
    }
    private float SignedAngle(Vector3 a, Vector3 b, Vector3 n)
    {
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        float signedAngle = angle * sign;

        while (signedAngle < 0) signedAngle += 360;

        return signedAngle;
    }
    private void RotateCam()
    {
        Transform viewCamTransform = viewCam.transform;
        Transform camTransform = portalCam.transform;
        Transform partnerTransform = partner.transform;

        Vector3 cameraEuler = Vector3.zero;

        Vector3 pos = partnerTransform.InverseTransformPoint(viewCamTransform.position);
        camTransform.localPosition = new Vector3(-pos.x, pos.y, -pos.z);

        Transform prevParent = viewCamTransform.parent;
        viewCamTransform.SetParent(transform);

        cameraEuler.x = viewCamTransform.localEulerAngles.x;
        viewCamTransform.SetParent(prevParent);

        Vector3 oldRot = viewCamTransform.localEulerAngles;
        viewCamTransform.localRotation = Quaternion.Euler(0, oldRot.y, oldRot.z);

        cameraEuler.y = SignedAngle(-partnerTransform.forward, viewCamTransform.forward, Vector3.up);

        viewCamTransform.localRotation = Quaternion.Euler(oldRot);

        camTransform.localRotation = Quaternion.Euler(cameraEuler);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!useCamera)return; 
        RotateCam();
    }
}
