using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ApplicationSettings : MonoBehaviour
{
    
    static bool _postProcessStatus;
    static bool _Renderer;

    
    private void Awake()
    {
        //Application.targetFrameRate = 60;
        SystemeGraphicAuto();
    }

    public void PostProcessActive()
    {
        _postProcessStatus = !_postProcessStatus;
        PlayerData.instance.parameter[0] = _postProcessStatus;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = _postProcessStatus;
    }

    public void ChangeCameraRenderer()
    {
        _Renderer = !_Renderer;
        PlayerData.instance.parameter[1] = _Renderer;
        if (_Renderer)
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
        else
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
    }

    public void SystemeGraphicAuto()
    {
        Debug.Log(SystemInfo.systemMemorySize);
        Debug.Log(message: SystemInfo.graphicsPixelFillrate);
    }
    
}
