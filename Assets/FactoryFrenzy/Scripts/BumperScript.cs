using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{
    // Player's Tag
    public string playerTag;
    // Power of propulsion (sensible)!
    public int powerLevel = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Collision Detection
    private void OnCollisionEnter(Collision other)
    {
        string tagObject = other.gameObject.tag;

        if (playerTag == tagObject)
        {
            // Not Necessary in Network Environnement
            // other.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            // Try to get contact of collision
            foreach (ContactPoint contact in other.contacts)
            {
                contact.otherCollider.GetComponent<Rigidbody>().AddForce(-1 * contact.normal * powerLevel, ForceMode.Impulse);
            }
        }
    }


}
