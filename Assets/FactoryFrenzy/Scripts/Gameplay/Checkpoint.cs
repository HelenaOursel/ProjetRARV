using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Material MaterialRefFailed;
    public Material MaterialRefPassed;
    public string TagPlayer;

    public GameObject lightPart;
    public int checkpointNumber;

    // Start is called before the first frame update
    void Start()
    {
        AnimationLight(false, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AnimationLight(bool status, bool isOwner)
    {
        if (!status)
        {
            lightPart.GetComponent<MeshRenderer>().material = MaterialRefFailed;
        } else
        {
            if (isOwner)
            {
                lightPart.GetComponent<MeshRenderer>().material = MaterialRefPassed;
            }
        }
    }

    




    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag == TagPlayer)
        {
            // Debug.Log(other.GetComponentInParent<PlayerController>().IsOwner);
            AnimationLight(true, other.GetComponentInParent<PlayerController>().IsOwner);
            other.GetComponentInParent<PlayerController>().SetCheckpointPassedNumber(checkpointNumber);
            other.GetComponentInParent<PlayerController>().InsertCheckpoint(transform.position + new Vector3(-1, 1f, 0));
        }
    }
}
