using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PickTeam : NetworkBehaviour
{
    
    
    [ServerCallback]
    private void Awake()
    {

        
        int connectedPlayers = GameObject.Find("Team").GetComponent<TeamManager>().connectedPlayers;
        int firstPlayerTeam = GameObject.Find("Team").GetComponent<TeamManager>().firstPlayerTeam;
        


        if (connectedPlayers == 0)
        {
            
            firstPlayerTeam = Random.Range(0, 2);
            
            gameObject.GetComponent<Build>().team = firstPlayerTeam;
            

        }
        else if (firstPlayerTeam != 1)
        {
            
            gameObject.GetComponent<Build>().team = 1;
            
        }
        else
        {
            
            gameObject.GetComponent<Build>().team = 0;
            
        }
        GameObject.Find("Team").GetComponent<TeamManager>().connectedPlayers++;
        GameObject.Find("Team").GetComponent<TeamManager>().firstPlayerTeam = firstPlayerTeam;

    }

    
    
    
    

   

    
    


}
