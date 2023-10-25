using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight : MonoBehaviour
{

    public Light lightCompo;
    private float timeOn = 2f;
    private float timeOff = 0.5f;
    private float changeTime = 0;

    void Update()
    {
        timeOn = Random.Range(2, 5);
        timeOff = Random.Range(0.1f, 0.5f);
        if (Time.time > changeTime)
        {
            lightCompo.enabled = !lightCompo.enabled;
            if (lightCompo.enabled)
            {
                changeTime = Time.time + timeOn;
            }
            else
            {
                changeTime = Time.time + timeOff;
            }
        }
    }

}