using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class Build : NetworkBehaviour
{
    [SyncVar] public int team;
    public GameObject[] buildings;
    public string[] placeableTags;
    public Dropdown dropdown;
    public Camera playerCamera;
    public Material[] materials;
    public int[] prices;
    public Button stopBuilding, build;
    [SyncVar]public bool host = false;
    GameObject ghost;
    public Money moneyScript;
    public GameObject panel;

    bool building = false;

   
    private void Start()
    {
        if (!isLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false);
            dropdown.gameObject.SetActive(false);
            build.gameObject.SetActive(false);
            panel.SetActive(false);
        }

        
    }


    [Client]
    public void BuildBuilding()
    {
        if (isLocalPlayer)
        {
            
            SpawnGhost();
            dropdown.interactable = false;

            building = true;
            build.gameObject.SetActive(false);
            stopBuilding.gameObject.SetActive(true);
        }
        

        
       
        
        
    }


    [Client]
    public void StopBuilding()
    {
        if (isLocalPlayer)
        {
            
            Destroy(ghost);
            building = false;
            build.gameObject.SetActive(true);
            dropdown.interactable = true;
            
            stopBuilding.gameObject.SetActive(false);
        }

    }

    void SpawnGhost()
    {
        
        ghost = Instantiate(buildings[dropdown.value]);
        ghost.AddComponent<GhostBuilding>().player = gameObject;
        ghost.GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Update()
    {
        if (building)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            
            ghost.transform.position = hit.point;
            
            if (Input.GetMouseButtonDown(0))
            {
                Place(dropdown.value, ray, ghost.GetComponent<GhostBuilding>().atBuilder);
                
            }
        }
    }

    

    [Command]
    void Place(int buildingIndex, Ray ray, bool atBuilder)
    {
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag(placeableTags[buildingIndex]) && atBuilder && moneyScript.money >= prices[buildingIndex])
            {
                moneyScript.TakeMoney(prices[buildingIndex]);
                GameObject build = Instantiate(buildings[buildingIndex], hit.point, Quaternion.Euler(-90, 0, 0));
                build.GetComponent<Building>().team = team;
                NetworkServer.Spawn(build);
            }
        }
        
    }

    
    


    
}
