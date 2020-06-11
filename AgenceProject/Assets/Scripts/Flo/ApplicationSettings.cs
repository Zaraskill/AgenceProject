using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ApplicationSettings
{
    
    static bool _postProcessStatus;
    static bool _Renderer;

    /*
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    */

    static public bool PostProcessActive()
    {
        _postProcessStatus = !_postProcessStatus;
        PlayerData.instance.parameter[0] = _postProcessStatus;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = _postProcessStatus;

        return _postProcessStatus;
    }

    static public bool ChangeCameraRenderer()
    {
        _Renderer = !_Renderer;
        PlayerData.instance.parameter[1] = _Renderer;
        if (_Renderer)
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
        else
            Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);

        return _Renderer;
    }
    
}
