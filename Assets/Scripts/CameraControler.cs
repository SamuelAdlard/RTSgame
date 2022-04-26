using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    float yRotation = 0f;
    public float Mousesensitivity =600f;
    public float verticalSpeed = 0.5f;
    public float horizontalSpeed = 4f;
    
    void Update()
    {
        

        
        
        
        if (Input.GetMouseButton(1))
        {
            float MouseX = Input.GetAxis("Mouse X") * Mousesensitivity * Time.deltaTime;
            yRotation -= MouseX;
            transform.localRotation = Quaternion.Euler(0,transform.localRotation.y + -yRotation, 0f);
        }
        
        float Y = 0;
        Mathf.Clamp(Y, -1, 1);
        if(Input.GetKey("e"))
        {
            Y = Y + verticalSpeed;
            
        }
        else 
        {
            Y = 0;
        }
        if (Input.GetKey("q"))
        {
            Y = Y + -verticalSpeed;
        }
        else if(!Input.GetKey("e"))
        {
            Y = 0;
        }
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(X, Y, Z) * horizontalSpeed * Time.deltaTime);

    }
}
