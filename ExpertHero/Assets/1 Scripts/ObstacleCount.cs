using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCount : MonoBehaviour
{
    public int obsCount;
    private void Start()
    {
        obsCount = transform.childCount;
    }
}
