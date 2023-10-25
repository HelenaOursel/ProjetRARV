using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{
    // Player's Tag
    public string playerTag;
    // Power of propulsion (sensible)!
    public int powerLevel = 100;

    private AudioSource audioSource;

    // Collision Detection
    private void OnCollisionEnter(Collision other)
    {
        string tagObject = other.gameObject.tag;

        if (playerTag == tagObject)
        {
            // Not Necessary in Network Environnement
            // other.gameObject.GetComponent<Rigidbody>().isKinematic = false;

            audioSource = GetComponent<AudioSource>();
            audioSource.Play();

            // Try to get contact of collision
            foreach (ContactPoint contact in other.contacts)
            {
                contact.otherCollider.GetComponentInParent<Rigidbody>().AddForce(-1 * contact.normal * powerLevel, ForceMode.Impulse);
            }
        }
    }


}
