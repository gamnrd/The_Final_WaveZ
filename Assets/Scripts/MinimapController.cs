using UnityEngine;
using UnityEngine.Rendering;

public class MinimapController : MonoBehaviour
{
    private Transform player;
    private Light dirLightMinimap;
    private Light dirLightMain;

    private void Awake()
    {
        /*RenderPipelineManager.beginCameraRendering += PreRender;
        RenderPipelineManager.endCameraRendering += PostRender;*/
    }

    private void Start()
    {
        dirLightMain = GameObject.Find("Directional Light").GetComponent<Light>();
        //dirLightMinimap = GameObject.Find("Directional Light MiniMap").GetComponent<Light>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }
    /*
    private void PreRender(ScriptableRenderContext context, Camera camera)
    {
        Debug.Log("Prerender called");
        if (camera.gameObject.name == "MiniMapCamera")
        {
            dirLight.color = new Color(0, 0, 0, 1);
            dirLight.intensity = 10;
            RenderSettings.fog = false;
        }
    }

    private void PostRender(ScriptableRenderContext context, Camera camera)
    {
        Debug.Log("Postrender called");
        if (camera.gameObject.name == ("MiniMapCamera"))
        {
            dirLight.color = new Color(0, 0.4f, 1, 1);
            dirLight.intensity = 1;
            RenderSettings.fog = true;
        }
    }    */
    /*
    private void OnPreRender()
    {
        Debug.Log("Prerender called");
        dirLightMain.enabled = false;
        dirLightMinimap.enabled = true;
        //RenderSettings.fog = false;
       
    }

    private void OnPostRender()
    {
        Debug.Log("Postrender called");
        dirLightMain.enabled = true;
        dirLightMinimap.enabled = false;
        //RenderSettings.fog = true;

    }/*

    private void OnApplicationQuit()
    {
        RenderPipelineManager.beginCameraRendering -= PreRender;
        RenderPipelineManager.endCameraRendering -= PostRender;
    }*/
}
