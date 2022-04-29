using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class Delivery : NetworkBehaviour
{
    public int carrying = 0;
    public int maxCapacity = 50;
    public GameObject pickUp, dropOff;
    public bool delivering = false;
    public bool deliveringToDropOff = false;
    public bool deliveringToPickUp = false;
    public bool testing = false;
    [SyncVar]public GameObject teamPlayer;
    public NavMeshAgent agent;
    
    private void Awake()
    {
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            
            if (player.GetComponent<Build>().team == gameObject.GetComponent<Unit>().team)
            {
                teamPlayer = player;
            }
        }
    }
    
    [ServerCallback]
    private void FixedUpdate()
    {
        if (pickUp != null && dropOff != null)
        {
            delivering = true;
            if (carrying > 0)
            {
                if (gameObject.GetComponent<Unit>().unitType == "Ship")
                {
                    agent.SetDestination( new Vector3(dropOff.transform.position.x, -4.09f, dropOff.transform.position.z));
                }
                else
                {
                    agent.SetDestination(new Vector3(dropOff.transform.position.x, -3.6f, dropOff.transform.position.z));
                }
                
                deliveringToDropOff = true;
                if (testing)
                {
                    print("Going to drop off");
                }

            }
            else
            {
                deliveringToDropOff = false;
            }


            if (carrying <= 0)
            {
                if (gameObject.GetComponent<Unit>().unitType == "Ship")
                {
                    agent.SetDestination(new Vector3(pickUp.transform.position.x, -4.09f, pickUp.transform.position.z));
                }
                else
                {
                    agent.SetDestination(new Vector3(pickUp.transform.position.x, -3.6f, pickUp.transform.position.z));
                }
                deliveringToPickUp = true;
                if (testing)
                {
                    print("Going to pickup");
                }
            }
            else
            {
                deliveringToPickUp = false;
            }

            if (Vector3.Distance(transform.position, pickUp.transform.position) < 1.75f && deliveringToPickUp == true)
            {

                
                if (pickUp.GetComponent<SupplyCenter>())
                {
                    pickUp.GetComponent<SupplyCenter>().PickUp(gameObject);
                }
                else
                {
                    pickUp.GetComponent<Production>().PickUp(gameObject);
                }

            }
            

            if (Vector3.Distance(transform.position, dropOff.transform.position) < 1.75f && deliveringToDropOff == true)
            { 
                dropOff.GetComponent<Production>().DropOff(gameObject);
                dropOff.GetComponent<Production>().DropOff(gameObject);
            }
        }
        else
        {
            delivering = false;
        }
    }

    [Client]
    public void SelectPickUp()
    {
        
        teamPlayer.GetComponent<DeliveryControl>().SelectPickUp();
    }    


    [Client]
    public void SelectDropOff()
    {
        
        teamPlayer.GetComponent<DeliveryControl>().SelectDropOff();
    }

    [Client]
    public void Clear()
    {
        teamPlayer.GetComponent<DeliveryControl>().Clear();
    }

}
