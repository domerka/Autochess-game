using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCanvas : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
    }
}
