using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ApplicationSettings : MonoBehaviour
{

    public static bool _postProcessStatus;
    public static bool _Renderer;

    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (PlayerData.instance.timerNumber[0] == 0)
            if (CheckDeviceMemory(4095))
            {
                PlayerData.instance.parameter[0] = true;
                PlayerData.instance.parameter[1] = true;

                SystemeGraphicAuto(true);
            }
    }

    static public void ToggleGraphicSettings()
    {
        _postProcessStatus = UIManager.uiManager.graphicsSettings.isOn;
        _Renderer = UIManager.uiManager.graphicsSettings.isOn;
        PlayerData.instance.parameter[0] = _postProcessStatus;
        PlayerData.instance.parameter[1] = _Renderer;
        SystemeGraphicAuto(false);
    }

    static public void SystemeGraphicAuto(bool require)
    {
        if (require)
        {
            _postProcessStatus = PlayerData.instance.parameter[0];
            _Renderer = PlayerData.instance.parameter[1];
        }
        
        /****/
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = _postProcessStatus;

        if (_Renderer)
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
        else
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
        /****/
        //Debug.Log(SystemInfo.systemMemorySize);
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
