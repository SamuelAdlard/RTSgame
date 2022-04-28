using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class Production : NetworkBehaviour
{
    public string placeableTag = "Ground";
    public GameObject[] units;
    public int[] prices;
    public Transform point, pivot;
    public Dropdown dropdown;
    
    public Text moneyText;
    [SyncVar]public int storedMoney = 0;
    public int maximumStorage = 400;
    public List<int> producing = new List<int>();
    public float[] productionTimes;
    float nextUnit = 0;

    [ServerCallback]
    private void FixedUpdate()
    {
        
        
        if (producing.Count > 0)
        {
            if (Time.time > nextUnit)
            {
                SpawnUnit(producing[0]);
                if (producing.Count > 1)
                {
                    nextUnit = Time.time + productionTimes[producing[1]];
                }

                producing.RemoveAt(0);
            }
        }
    }

    private void Update()
    {

        if (moneyText != null)
        {
            moneyText.text = "Stored Money = €" + storedMoney;
        }
        
    }

    [Client]
    public void BuyUnit()
    {
        NetworkClient.localPlayer.gameObject.GetComponent<ProductionControl>().PlayerAddUnit(gameObject,dropdown.value);
    }


    [Server]
    public void AddUnit(GameObject player, int unitIndex)
    {
        
        if (storedMoney >= prices[unitIndex] && player.GetComponent<Build>().team == gameObject.GetComponent<Building>().team)
        {
            if (producing.Count == 0)
            {
                nextUnit = Time.time + productionTimes[unitIndex];
            }
            storedMoney -= prices[unitIndex];
            producing.Add(unitIndex);
        }

    }

    [Server]
    void SpawnUnit(int index)
    {
        bool spawned = false;
        
        RaycastHit hit;
        while (!spawned)
        {
            if (Physics.Raycast(point.transform.position, Vector3.down, out hit))
            {
                if (hit.transform.CompareTag(placeableTag))
                {
                    spawned = true;
                    GameObject unit = Instantiate(units[index], hit.point, Quaternion.identity);
                    NetworkServer.Spawn(unit);
                    unit.GetComponent<Unit>().team = gameObject.GetComponent<Building>().team;
                }
            }
            pivot.Rotate(0, 10, 0);
        }
    }

    [Server]
    public void PickUp(GameObject unit)
    {
        if (storedMoney >= unit.GetComponent<Delivery>().maxCapacity)
        {
            unit.GetComponent<Delivery>().carrying = unit.GetComponent<Delivery>().maxCapacity;
            storedMoney -= unit.GetComponent<Delivery>().maxCapacity;
        }
        else if (storedMoney >= 0)
        {
            unit.GetComponent<Delivery>().carrying = storedMoney;
            storedMoney = 0;
        }

    }

    [Server]
    public void DropOff(GameObject unit)
    {
        storedMoney += unit.GetComponent<Delivery>().carrying;
        unit.GetComponent<Delivery>().carrying = 0;
        if (storedMoney > maximumStorage)
        {
            storedMoney = maximumStorage;
        }
    }


}
