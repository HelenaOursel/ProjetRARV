using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineScript : MonoBehaviour
{
    
    // Player's Tag
    public string playerTag;
    // Power of propulsion
    public int powerLevel = 100;

    // Direction of propulsion
    public Vector3 direction = new Vector3(-1f, 1f, 0f);
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Collider Event To Make Up Object In Contact
    private void OnCollisionEnter(Collision collision)
    {
        // Tag's Object touched
        string tagObject = collision.gameObject.tag;

        if (tagObject == playerTag)
        {            
           collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * powerLevel);
        }
        
    }
}
