using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Soldier : MonoBehaviour
{
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public int damage = 1;
    public float fireDelay = 1;
    public float nextFire;
    public string damageBonus = "none";
    public GameObject parentObject;
    public Transform gunBarrel;
    public ParticleSystem shoot;
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.CompareTag("Unit"))
        {
            
            if (other.gameObject.GetComponent<Unit>().team != parentObject.GetComponent<Unit>().team)
            {
                enemiesInRange.Add(other.gameObject);
            }
           
            
        }
        else if (other.transform.CompareTag("Building") && other.GetComponent<Building>().team != parentObject.GetComponent<Unit>().team)
        {
            
            
            enemiesInRange.Add(other.gameObject);
        }
    }

    [ServerCallback]
    void FixedUpdate()
    {
        int i = 0;
        if (enemiesInRange.Count > 0 && i <= enemiesInRange.Count)
        {
            if (enemiesInRange[i] != null)
            {
                transform.parent.LookAt(new Vector3(enemiesInRange[i].transform.position.x, transform.parent.position.y, enemiesInRange[i].transform.position.z));
                if (Time.time > nextFire)
                {
                    RaycastHit hit;
                    gunBarrel.LookAt(enemiesInRange[i].transform);
                    GameObject particles = Instantiate(shoot, gunBarrel.position, gunBarrel.rotation).gameObject;
                    NetworkServer.Spawn(particles);
                    
                    if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out hit, 2f))
                    {
                        if (hit.transform.CompareTag("Unit"))
                        {
                            hit.transform.GetComponent<Unit>().health -= damage;
                            
                        }
                        else if(hit.transform.CompareTag("Building"))
                        {
                            
                            hit.transform.GetComponent<Building>().health -= damage;
                            
                        }

                    }
                    nextFire = Time.time + fireDelay;
                }
            }
            else
            {
                enemiesInRange.RemoveAt(i);
            }
            i++;
        }
        
    }
    
    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        
        if (enemiesInRange.Contains(other.gameObject))
        {
            print(other.name);
            enemiesInRange.Remove(other.gameObject);
        }
    }
}
