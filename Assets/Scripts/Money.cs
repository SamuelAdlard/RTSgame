using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class Money : NetworkBehaviour
{
    [SyncVar]public int money = 10;
    public Text text;
    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            text.text = "€" + money.ToString();
            

        }
    }
    [Server]
    public void TakeMoney(int amount)
    {
        money -= amount;
    }
    [Server]
    public void AddMoney(int amount)
    {
        money += amount;
    }
}
