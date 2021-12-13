using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay;

    float timeBetweenDisplay = 1.0f;
    float interval = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        fpsDisplay = transform.FindDeepChild("FPS Display").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > interval)
        {
            float fps = Mathf.Floor(1 / Time.unscaledDeltaTime);
            fpsDisplay.text = "" + fps;
            interval += timeBetweenDisplay;
        }
    }
}
