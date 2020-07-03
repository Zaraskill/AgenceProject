using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ApplicationSettings : MonoBehaviour
{

    public static bool _postProcessStatus;
    public static bool _Renderer;
    public static bool isMobileDevice;

    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        isMobileDevice = GetDevice();
        if (PlayerData.instance.timerNumber[0] == 0)
            if (CheckDeviceMemory(4095))
            {
                PlayerData.instance.parameter[0] = true;
                PlayerData.instance.parameter[1] = true;

                SystemeGraphicAuto();
            }

    }

    bool GetDevice()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            return true;
        }
        return false;
    }

    static public void PostProcessActive()
    {
        _postProcessStatus = UIManager.uiManager.postProcessSettings.isOn;
        PlayerData.instance.parameter[0] = _postProcessStatus;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = _postProcessStatus;
    }

    static public void ChangeCameraRenderer()
    {
        _Renderer = UIManager.uiManager.rendererSettings.isOn;
        PlayerData.instance.parameter[1] = _Renderer;
        if (_Renderer)
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
        else
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
    }

    static public void SystemeGraphicAuto()
    {
        _postProcessStatus = PlayerData.instance.parameter[0];
        UIManager.uiManager.postProcessSettings.isOn = _postProcessStatus;
        _Renderer = PlayerData.instance.parameter[1];
        UIManager.uiManager.rendererSettings.isOn = _Renderer;
        /****/
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = _postProcessStatus;

        if (_Renderer)
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
        else
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
        /****/
        Debug.Log(SystemInfo.systemMemorySize);
    }

    static public bool CheckDeviceMemory(int targetMemory)
    {
        int deviceMemory = SystemInfo.systemMemorySize;

        if (deviceMemory > targetMemory)
            return true;
        else
            return false;
    }
    
}
