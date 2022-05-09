using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    public GameObject parentObject;
    public Material[] materials;
    private void Start()
    {
        StartCoroutine(TeamColour());
    }

    IEnumerator TeamColour()
    {

        yield return new WaitForSeconds(0.25f);

        transform.GetComponent<MeshRenderer>().material = materials[parentObject.GetComponent<Unit>().team];
    }
}
