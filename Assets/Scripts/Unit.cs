using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class Unit : NetworkBehaviour
{
    [SyncVar] public int team = 2;
    [SyncVar] public int health = 10;
    [SyncVar] public int maxHealth = 10;
    [SyncVar] public bool garrisoned = false;
    public string walkableTag = "Ground";
    public string unitType = "Soldier";
    public Material[] materials;
    public GameObject model;

    [ServerCallback]
    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }        
    }

    private void Update()
    {
        if (garrisoned)
        {
            gameObject.GetComponent<Unit>().model.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Unit>().model.GetComponent<MeshRenderer>().enabled = true;
        }
    }



    private void Awake()
    {
        StartCoroutine(TeamColour());
    }

  

    IEnumerator TeamColour()
    {

        yield return new WaitForSeconds(0.25f);
        
        model.GetComponent<MeshRenderer>().material = materials[team];
    }
}
