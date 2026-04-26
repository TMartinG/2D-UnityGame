using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightScrollController : MonoBehaviour
{
    [SerializeField] private float adjustSpeed = 50f;
    private Light2D light2D;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && light2D != null)
        {
            light2D.pointLightOuterAngle = Mathf.Clamp(
                light2D.pointLightOuterAngle + scroll * adjustSpeed,
                10f,
                130f
            );

            light2D.pointLightInnerAngle = Mathf.Clamp(
                light2D.pointLightOuterAngle - 50f,
                0f,
                light2D.pointLightOuterAngle
            );
        }
    }
}
