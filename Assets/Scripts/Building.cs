using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Building : NetworkBehaviour
{
    [SyncVar]public int team = 2;
    [SyncVar] public int health = 10;
    [SyncVar] public int maxHealth = 10;
    public GameObject manager;
    
    public Material[] materials;
    
    private void Start()
    {
        StartCoroutine(TeamColour());
    }

    [ServerCallback]
    private void Awake()
    {
        manager = GameObject.Find("GameManager");
        
        manager.GetComponent<Game>().AddObject(gameObject,team);
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

    [ServerCallback]
    private void OnDestroy()
    {
        manager.GetComponent<Game>().RemoveObject(gameObject, team);
    }
}
