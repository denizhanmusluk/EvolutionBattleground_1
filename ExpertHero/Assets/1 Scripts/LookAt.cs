using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 relativeVector = transform.InverseTransformPoint(target.position);
            relativeVector /= relativeVector.magnitude;
            float newSteer = (relativeVector.x / relativeVector.magnitude) * 50;
            transform.Rotate(0, newSteer * Time.deltaTime * 10, 0);
        }
    }
}
