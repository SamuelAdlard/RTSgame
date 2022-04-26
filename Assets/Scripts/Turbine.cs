using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Turbine : NetworkBehaviour
{
    public int moneyGeneration = 10;
    public float delay = 5f;
    public List<Transform> turbines = new List<Transform>(); 
    GameObject teamPlayer;
    
    float nextPayment = 0;
    [ServerCallback]
    private void Start()
    {
        if (!CompareTag("Ghost"))
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                
                if (player.GetComponent<Build>().team == gameObject.GetComponent<Building>().team)
                {
                    teamPlayer = player;
                }
            }
            
            GameObject[] buildings;
            buildings = GameObject.FindGameObjectsWithTag("Building");
            foreach (GameObject building in buildings)
            {
                if (building.GetComponent<Turbine>() != null)
                {
                    building.GetComponent<Turbine>().NewTurbine();
                }
            }

            moneyGeneration = CalculateProduction();
            nextPayment = Time.time + delay;
            
        }
        
    }


    public void NewTurbine()
    {
        moneyGeneration = CalculateProduction();
    }

    [ServerCallback]
    private void FixedUpdate()
    {
        if (Time.time > nextPayment && !CompareTag("Ghost"))
        {
            if (teamPlayer != null)
            {
                teamPlayer.GetComponent<Money>().AddMoney(moneyGeneration);
                nextPayment = Time.time + delay;
            }
           
        }
    }


    [Server]
    int CalculateProduction()
    {
        GameObject[] buildings;
        buildings = GameObject.FindGameObjectsWithTag("Building");
        float lowestDistance = Mathf.Infinity;

        foreach (GameObject building in buildings)
        {
            if (building.GetComponent<Turbine>() != null && Vector3.Distance(building.transform.position,transform.position) < lowestDistance && building != gameObject)
            {
                lowestDistance = Vector3.Distance(building.transform.position, transform.position);
                print(lowestDistance);
            }
        }
       
        if (lowestDistance > 0.5f * moneyGeneration)
        {
            return Mathf.RoundToInt(moneyGeneration); 
        }
        else
        {
            return Mathf.RoundToInt(moneyGeneration - (moneyGeneration - lowestDistance * 2));
        }

       

    }

}
