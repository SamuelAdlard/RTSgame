using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class UnitControl : NetworkBehaviour
{
    public int team = 2;
    public Camera playerCamera;
    public List<GameObject> selectedUnits = new List<GameObject>();
    public float randomFactor = 0.5f;
    
    void Start()
    {
        team = gameObject.GetComponent<Build>().team;

    }

    [Client]
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Unit") )
                {
                    
                    if (hit.transform.GetComponent<Unit>().team == team && !selectedUnits.Contains(hit.transform.gameObject))
                    {
                        

                        selectedUnits.Add(hit.transform.gameObject);
                    }
                }
                else if(selectedUnits.Count > 0)
                {
                    
                    int i = 0;
                    foreach (GameObject unit in selectedUnits)
                    {
                        if (unit != null && isLocalPlayer)
                        {
                            MoveUnits(ray, unit, selectedUnits.Count);
                        }
                        
                        i++;
                    }
                    
                }
                    
            }
        }

        if (Input.GetKeyDown("c"))
        {
            ClearSelection();
        }
    }
   


    
    private void ClearSelection()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits.RemoveAt(0);
        }
    }

    [Command]
    private void MoveUnits(Ray ray, GameObject unit,int length)
    {
        
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (unit.GetComponent<Unit>().team == team && hit.transform.CompareTag(unit.GetComponent<Unit>().walkableTag))
        {
            if (length > 1)     
            {
                Vector3 moveLocation = new Vector3(Random.Range(-randomFactor, randomFactor), 0, Random.Range(-randomFactor, randomFactor));
                unit.GetComponent<NavMeshAgent>().SetDestination(hit.point + moveLocation);
            }
            else
            {
                unit.GetComponent<NavMeshAgent>().SetDestination(hit.point);
            }
        }
        
    }
    

    
}
