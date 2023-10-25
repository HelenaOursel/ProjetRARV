using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBlowing : MonoBehaviour
{

    public int powerWind = 10;
    public float speedWheel = 2;
    private float speedFactorWheel = 10;
    public string tagObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WheelAnimation()
    {
        var newRotation = transform.eulerAngles;
        speedFactorWheel += 50;
        transform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, speedWheel * speedFactorWheel * Time.deltaTime);
        
    }

    private void Repulsion(GameObject gameObject, int power)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * power);
    }



    private void OnTriggerStay(Collider other)
    {
        string tag = other.tag;

        if (tagObject == tag)
        {
            Debug.Log("Get For animation");
            this.WheelAnimation();
            this.Repulsion(other.gameObject.transform.parent.gameObject, this.powerWind);
        }

    }


}
