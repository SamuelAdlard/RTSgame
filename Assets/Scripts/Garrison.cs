using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class Garrison : MonoBehaviour
{
    public List<GameObject> garrisonedUnits = new List<GameObject>();
    public List<GameObject> unitsInRange = new List<GameObject>();
    public bool unitsGarrisoned = false;
    public int maxUnits = 5;
    public int numUnits = 0;
    public Transform pivot, point;
    public string placeableTag = "Ground";

    [Server]
    public void GarrisonUnits(GameObject player)
    {
        
        foreach (GameObject unit in unitsInRange)
        {
            if (numUnits < maxUnits && unit.GetComponent<Unit>().team == transform.parent.GetComponent<Unit>().team)
            {
                garrisonedUnits.Add(unit);
                numUnits++;
                
                player.GetComponent<UnitControl>().selectedUnits.Remove(unit);
                unitsGarrisoned = true;
                unit.GetComponent<NavMeshAgent>().enabled = false;
                unit.GetComponent<Unit>().garrisoned = true;

                unit.transform.position = new Vector3(0, -10, 0);


            }
           
        }

        foreach (GameObject unit in garrisonedUnits)
        {
            unitsInRange.Remove(unit);
        }
    }
    


    [Server]
    public void UnGarrisonUnits()
    {
        
        
        RaycastHit hit;
        for (int i = 0; i < 36; i++)
        {
            if (Physics.Raycast(point.transform.position, Vector3.down, out hit))
            {
                if (hit.transform.CompareTag(placeableTag))
                {
                    
                    foreach (GameObject unit in garrisonedUnits)
                    {
                        
                        unit.GetComponent<Unit>().garrisoned = false;
                        unit.transform.position = hit.point;
                        unitsGarrisoned = false;
                        numUnits = 0;
                        
                        unit.GetComponent<NavMeshAgent>().enabled = true;
                    }

                    for (int a = 0; a < garrisonedUnits.Count; a++)
                    {
                        garrisonedUnits.RemoveAt(0);
                    }
                    return;
                }
            }
            pivot.Rotate(0, 10, 0);
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.CompareTag("Unit") && !garrisonedUnits.Contains(other.gameObject))
        {
            
            if (other.gameObject.GetComponent<Unit>().team == transform.parent.GetComponent<Unit>().team
            && other.gameObject.GetComponent<Unit>().unitType == "Soldier" || other.gameObject.GetComponent<Unit>().unitType == "Tank"
            || other.gameObject.GetComponent<Unit>().unitType == "Builder" || other.gameObject.GetComponent<Unit>().unitType == "Truck")
            {
                unitsInRange.Add(other.gameObject);
            }
            
        } 
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {

        if (unitsInRange.Contains(other.gameObject))
        {
            
            unitsInRange.Remove(other.gameObject);
        }
    }
}
