using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light thisLight;
    [SerializeField] private bool isDirectionalLght = false;
    [SerializeField] private float lightIntensity = 1.5f;

    private void Awake()
    {
        thisLight = GetComponent<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (InputController.Instance.GetPlatform() == Platform.Web || InputController.Instance.GetPlatform() == Platform.Mobile)
        {
            if (isDirectionalLght)
            {
                thisLight.intensity = lightIntensity;
            }
            else
            {
                thisLight.enabled = false;
            }
        }
    }
}
