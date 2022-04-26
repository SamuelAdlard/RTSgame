using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ProductionControl : NetworkBehaviour
{
    public Camera playerCamera;
    public GameObject selectedBuilding;
    public bool buildingSelected = false;
    
    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !buildingSelected && Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Building") && hit.transform.GetComponent<Production>() != null 
                && !transform.GetComponent<DeliveryControl>().selectingPickUp && !transform.GetComponent<DeliveryControl>().selectingDropOff)
            {
                if (hit.transform.GetComponent<Building>().team == gameObject.GetComponent<Build>().team)
                {
                    selectedBuilding = hit.transform.gameObject;
                    buildingSelected = true;

                    selectedBuilding.transform.Find("Canvas").gameObject.SetActive(true);
                }
                
            }

        }

        if (Input.GetKeyDown("c") && buildingSelected)
        {
            selectedBuilding.transform.Find("Canvas").gameObject.SetActive(false);
            buildingSelected = false;
            selectedBuilding = null;
        }
    }
    [Client]
    public void PlayerAddUnit(GameObject building, int index)
    {
        CmdAddUnit(building, index);
        
    }
  
    [Command]
    void CmdAddUnit(GameObject building, int index)
    {
        
        building.GetComponent<Production>().AddUnit(gameObject,index);
    }


}
