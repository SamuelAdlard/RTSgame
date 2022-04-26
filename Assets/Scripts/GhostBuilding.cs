using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class GhostBuilding : NetworkBehaviour
{
    public GameObject player;
    public bool atBuilder = false;
    public int team = 2;
    Collider enteredCollider;
    [Client]
    private void Awake()
    {
        gameObject.layer = 2;
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        tag = "Ghost";
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Builder") && player.GetComponent<Build>().team == other.transform.parent.GetComponent<Unit>().team)
        {
            team = other.transform.parent.GetComponent<Unit>().team;
            atBuilder = true;
            
            
            enteredCollider = other;
        }
    }
    [Client]
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }

    [Client]
    private void OnTriggerExit(Collider other)
    {
        if (other == enteredCollider)
        {
            atBuilder = false;
        }
    }
}
