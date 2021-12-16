using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenTest : MonoBehaviour
{

    LTBezierPath ltPath;

    public Vector3 startControl;
    public Vector3 endControl;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float speed;

    void Start()
    {
        ltPath = new LTBezierPath(new Vector3[] { startPoint, startControl, endControl, endPoint });

        LeanTween.move(gameObject, ltPath, speed).setOrientToPath(true).setEase(LeanTweenType.easeInOutQuad); // animate
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
