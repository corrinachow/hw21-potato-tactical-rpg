using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCube : MonoBehaviour
{
    private void FixedUpdate()
    {
        this.transform.Rotate(0.0f, 1.0f, 0.0f);   
    }
}
