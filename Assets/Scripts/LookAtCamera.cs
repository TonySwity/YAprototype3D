using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    private void Update()
    {
        Vector3 toCamera = transform.position - _cameraTransform.position;
        transform.rotation = Quaternion.LookRotation(toCamera);
    }
}
