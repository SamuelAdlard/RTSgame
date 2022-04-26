using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Building : NetworkBehaviour
{
    [SyncVar]public int team = 2;
    [SyncVar] public int health = 10;
    [SyncVar] public int maxHealth = 10;
    
    public Material[] materials;
    
    private void Start()
    {
        StartCoroutine(TeamColour());
    }
    
    

    [ServerCallback]
    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    IEnumerator TeamColour()
    {
        
        yield return new WaitForSeconds(0.25f);
        
        gameObject.GetComponent<MeshRenderer>().material = materials[team];
    }
}
