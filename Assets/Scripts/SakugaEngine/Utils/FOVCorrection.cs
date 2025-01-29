using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVCorrection : MonoBehaviour
{
    Transform cam;
    [Range(0, 1)]
    public float CorrectionIntensity = 0.5f;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        //Vector3 newDirection = -cam.position;
        //transform.LookAt(newDirection);
        Vector3 relativePos = new Vector3(transform.position.x - cam.position.x, 0.0f, transform.position.z - cam.position.z);
        //the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Lerp(Quaternion.identity, rotation, CorrectionIntensity);
    }
}
