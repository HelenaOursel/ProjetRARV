using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSave : MonoBehaviour
{
    public Material MaterialRefPassed;
    public Material MaterialRefFailed;
    public GameObject CheckpointLight;
    public string PlayerTag;
    public int checkpointNumber;

    Transform position; 

    // Start is called before the first frame update
    void Start()
    {
        CheckpointLight.GetComponent<MeshRenderer>().material = MaterialRefFailed;
    }

    // Update is called once per frame
    void Update()
    {
    }
 
    public Transform _GetPosition()
    {
        return position;
    }

    void _SetPosition(Transform position)
    {
       this.position = position;
    }

    void GetPlayerGameObject(GameObject player, Transform transformCheckpoint)
    {
        player.GetComponentInParent<PlayerController>().SetCheckpointNumber(checkpointNumber);
        Vector3 newPosition = transformCheckpoint.position + new Vector3(-1f, 1f, 0f);
        player.GetComponentInParent<PlayerController>().SetCheckpoint(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag == PlayerTag)
        {
            CheckpointLight.GetComponent<MeshRenderer>().material = MaterialRefPassed;
            this._SetPosition(transform);
            other.GetComponentInParent<PlayerController>().SetCheckpointNumber(checkpointNumber);
            
            this.GetPlayerGameObject(other.gameObject, transform);
        }
        
    }
}
