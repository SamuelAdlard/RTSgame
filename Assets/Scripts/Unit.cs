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
    public GameObject manager;

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


    [ServerCallback]
    private void Awake()
    {
        manager = GameObject.Find("GameManager");
        print("Add object unit side");
        manager.GetComponent<Game>().AddObject(gameObject, team);
    }
    
    private void Start()
    {
        StartCoroutine(TeamColour());
    }

  

    IEnumerator TeamColour()
    {

        yield return new WaitForSeconds(0.25f);
        
        model.GetComponent<MeshRenderer>().material = materials[team];
    }

    [ServerCallback]
    private void OnDestroy()
    {
        manager.GetComponent<Game>().RemoveObject(gameObject, team);

    }


}
