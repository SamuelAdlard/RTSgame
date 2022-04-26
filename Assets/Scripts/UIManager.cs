using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public GameObject health;
    public Text healthText;
    public Text generationText;
    public GameObject generation;
    public Camera playerCamera;
    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Building"))
            {
                health.SetActive(true);
                healthText.text = hit.transform.GetComponent<Building>().health.ToString() + " / " + hit.transform.GetComponent<Building>().maxHealth.ToString();
                if (hit.transform.GetComponent<Turbine>())
                {
                    generation.SetActive(true);
                    generationText.text = "€" + hit.transform.GetComponent<Turbine>().moneyGeneration.ToString();
                }
                else
                {
                    generation.SetActive(false);
                }    
            }
            else if(hit.transform.CompareTag("Unit"))
            {
                generation.SetActive(false);
                health.SetActive(true);
                healthText.text = hit.transform.GetComponent<Unit>().health.ToString() + " / " + hit.transform.GetComponent<Unit>().maxHealth.ToString();
            }
            else
            {
                health.SetActive(false);
                generation.SetActive(false);
            }
        }
    }
}
