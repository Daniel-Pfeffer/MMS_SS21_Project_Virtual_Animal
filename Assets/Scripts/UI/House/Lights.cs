using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    Light light;

    public bool lightsOn = true;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightsOn)
        {
            light.intensity = 1;
        }
        else
        {
            light.intensity = 0;
        }
    }
}