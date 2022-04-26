using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SupplyCenter : MonoBehaviour
{

    public GameObject teamPlayer;
    
    [ServerCallback]
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<Build>().team == gameObject.GetComponent<Building>().team)
            {
                teamPlayer = player;
            }
        }

    }


    public void PickUp(GameObject unit)
    {
        if (teamPlayer.GetComponent<Money>().money >= unit.GetComponent<Delivery>().maxCapacity)
        {
            unit.GetComponent<Delivery>().carrying = unit.GetComponent<Delivery>().maxCapacity;
            teamPlayer.GetComponent<Money>().TakeMoney(unit.GetComponent<Delivery>().maxCapacity);
        }
        else if(teamPlayer.GetComponent<Money>().money >= 0)
        {
            unit.GetComponent<Delivery>().carrying = teamPlayer.GetComponent<Money>().money;
            teamPlayer.GetComponent<Money>().TakeMoney(teamPlayer.GetComponent<Money>().money);
        }
        
    }
   
}
