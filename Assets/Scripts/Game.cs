using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Game : MonoBehaviour
{
    public List<GameObject> redObjects = new List<GameObject>();
    public List<GameObject> blueObjects = new List<GameObject>();

    [ServerCallback]
    public void FixedUpdate()
    {
        if (redObjects.Count <= 0)
        {
            print("Blue wins");     
        }
        else if(blueObjects.Count <= 0)
        {
            print("Red wins");
        }
    }

    public void RemoveObject(GameObject gameObject, int team)
    {
        
        if (team == 1)
        {
            redObjects.Remove(gameObject);
        }
        else
        {
            blueObjects.Remove(gameObject);
        }
    }

    public void AddObject(GameObject gameObject, int team)
    {
        if (team == 1)
        {
            redObjects.Add(gameObject);
        }
        else
        {
            blueObjects.Add(gameObject);
        }
    }

}
