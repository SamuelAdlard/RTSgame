using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DeliveryControl : NetworkBehaviour
{

    public GameObject selectedUnit;
    public bool buildingSelected = false;
    public bool selectingPickUp = false;
    public bool selectingDropOff = false;
    public Camera playerCamera;
    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !buildingSelected && Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Unit") && hit.transform.GetComponent<Delivery>() && hit.transform.GetComponent<Unit>().team == gameObject.GetComponent<Build>().team)
            {
                
                buildingSelected = true;
                selectedUnit = hit.transform.gameObject;
                selectedUnit.transform.Find("Canvas").gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown("c") && buildingSelected)
        {
            buildingSelected = false;
            selectingPickUp = false;
            selectingDropOff = false;

            selectedUnit.transform.Find("Canvas").gameObject.SetActive(false);
            selectedUnit = null;
        }


        if (Input.GetMouseButtonDown(0) && selectingPickUp)
        {
            CmdSelectPickUp(selectedUnit, ray);
            selectingPickUp = false;
            print("selecting");

        }
        else if (Input.GetMouseButtonDown(0) && selectingDropOff)
        {
            CmdSelectDropOff(selectedUnit, ray);
            selectingDropOff = false;
            
        }
    }

    
    


    [Client]
    public void SelectPickUp()
    {
        selectingPickUp = true;
        selectingDropOff = false;
        
    }

    [Client]
    public void SelectDropOff()
    {
        selectingPickUp = false;
        selectingDropOff = true;
        
    }

    [Client]
    public void Clear()
    {
        selectingPickUp = false;
        selectingDropOff = false;
        CmdClear(selectedUnit);
    }

    
    [Command]
    void CmdSelectPickUp(GameObject selectedTransport, Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Building") && hit.transform.GetComponent<Building>().team == transform.GetComponent<Build>().team 
                && (hit.transform.GetComponent<Production>() || hit.transform.GetComponent<SupplyCenter>()))
            {
                test();
                selectedTransport.GetComponent<Delivery>().pickUp = hit.transform.gameObject;
            }
        }
    }

    [Client]
    void test()
    {
        print("hello");
    }

    [Command]
    void CmdSelectDropOff(GameObject selectedTransport, Ray ray)
    {
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Building") && hit.transform.GetComponent<Building>().team == transform.GetComponent<Build>().team
                && hit.transform.GetComponent<Production>() && selectedTransport.GetComponent<Delivery>().dropOff != selectedTransport.GetComponent<Delivery>().pickUp)
            {
                selectedTransport.GetComponent<Delivery>().dropOff = hit.transform.gameObject;
            }
        }
    }    

    [Command]
    void CmdClear(GameObject selectedTransport)
    {
        selectedTransport.GetComponent<Delivery>().dropOff = null;
        selectedTransport.GetComponent<Delivery>().pickUp = null;

    }

}
