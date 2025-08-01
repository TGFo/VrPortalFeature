using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class PortalCamera : MonoBehaviour
{
    [SerializeField]
    private Camera portalCamera;

    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private int iterations = 7;

    private RenderTexture tempTexture1;
    private RenderTexture tempTexture2;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

        tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        portals[0].renderer.material.mainTexture = tempTexture1;
        portals[1].renderer.material.mainTexture = tempTexture2;
    }
    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }
    private void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }
    void UpdateCamera(ScriptableRenderContext SRC, Camera camera)
    {
        if (portals[0].renderer.isVisible)
        {
            portalCamera.targetTexture = tempTexture1;
            for(int i = iterations - 1; i >= 0; --i)
            {
                RenderCamera(portals[0], portals[1], i, SRC);
            }
        }
        if(portals[1].renderer.isVisible)
        {
            portalCamera.targetTexture = tempTexture2;
            for(int i = iterations - 1; i >= 0; --i)
            {
                RenderCamera(portals[1], portals[0], i, SRC);
            }
        }
    }
    private void RenderCamera(Portal inPortal, Portal outPortal, int iterationID, ScriptableRenderContext SRC)
    {
        Debug.Log("camera render start");
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform cameraTransform = portalCamera.transform;
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;

        for(int i = 0; i <= iterationID; ++i)
        {
            Vector3 relativePos = inTransform.InverseTransformPoint(cameraTransform.position);
            relativePos = Quaternion.Euler(0, 180, 0) * relativePos;
            cameraTransform.position = outTransform.TransformPoint(relativePos);

            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * cameraTransform.rotation;
            relativeRot = Quaternion.Euler(0, 180, 0) * relativeRot;
            cameraTransform.rotation = outTransform.rotation * relativeRot;
        }

        Plane p = new Plane(-outTransform.forward, outTransform.position);
        Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
        
        //Matrix4x4 m = inPortal.transform.localToWorldMatrix * outPortal.transform.localToWorldMatrix * mainCamera.transform.localToWorldMatrix;
        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = newMatrix;
        //portalCamera.projectionMatrix = mainCamera.projectionMatrix;

        UniversalRenderPipeline.RenderSingleCamera(SRC, portalCamera);
        //UniversalRenderPipeline.SubmitRenderRequest(portalCamera, SRC);
    }
    // Update is called once per frame
    void Update()
    {
        //UpdateCamera();
    }
}
