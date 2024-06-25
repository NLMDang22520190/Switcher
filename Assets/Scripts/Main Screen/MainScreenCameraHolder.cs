using Cinemachine;
using UnityEngine;

public class MainScreenCameraHolder : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera homeCamera;
    [SerializeField] private CinemachineVirtualCamera settingsCamera;
    [SerializeField] private CinemachineVirtualCamera startCamera;

    private void Start()
    {
        // set main camera to current camera in scene
        ShowHomeCamera();
    }

    public void ShowHomeCamera()
    {
        ActivateCamera(homeCamera);
        Debug.Log("Home Camera Activated");
    }

    public void ShowSettingsCamera()
    {
        ActivateCamera(settingsCamera);
        Debug.Log("Settings Camera Activated");
    }

    public void ShowStartCamera()
    {
        ActivateCamera(startCamera);
        Debug.Log("Start Camera Activated");
    }

    private void ActivateCamera(CinemachineVirtualCamera cameraToActivate)
    {
        // Ensure only the specified camera is active
        homeCamera.gameObject.SetActive(cameraToActivate == homeCamera);
        settingsCamera.gameObject.SetActive(cameraToActivate == settingsCamera);
        startCamera.gameObject.SetActive(cameraToActivate == startCamera);
    }
}
