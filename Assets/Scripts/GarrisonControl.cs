using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GarrisonControl : NetworkBehaviour
{
    public GameObject selected;
    public Camera playerCamera;
    bool isSelected = false;
    [Client]
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && isLocalPlayer)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                
                if (hit.transform.CompareTag("Unit") && hit.transform.Find("GarrisonRange") != null)
                {
                    
                    if (hit.transform.GetComponent<Unit>().team == gameObject.GetComponent<Build>().team)
                    {
                        selected = hit.transform.gameObject;
                        isSelected = true;
                        
                    }

                }
            }
        }

        if (Input.GetKeyDown("g") && selected != null)
        {
            
            CmdGarrison(selected);
            
        }

        if (Input.GetKeyDown("c") && isSelected)
        {
            isSelected = false;
            selected = null;
        }
    }

    [Command]
    void CmdGarrison(GameObject selectedObject)
    {
        
        
        if (!selectedObject.transform.Find("GarrisonRange").GetComponent<Garrison>().unitsGarrisoned)
        {
            
            selectedObject.transform.Find("GarrisonRange").GetComponent<Garrison>().GarrisonUnits(gameObject);
        }
        else
        {
            selectedObject.transform.Find("GarrisonRange").GetComponent<Garrison>().UnGarrisonUnits();
        }
        
    }

}
