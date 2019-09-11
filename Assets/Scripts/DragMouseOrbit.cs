using UnityEngine;
using System.Collections;
 
[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class DragMouseOrbit : MonoBehaviour {
 
    public Transform target;
   
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
 
    private Rigidbody rigidbody;
 
    float x = 0.0f;
    float y = 0.0f;


    
    // Use this for initialization
    void Start ()  
    {
           
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
        rigidbody = GetComponent<Rigidbody>();
 
        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }
 
    void LateUpdate () 
    {
        if (Input.GetKey(KeyCode.Alpha3)) //that equation doesn't work yet
            x -= (0.005f / (1/Time.deltaTime));//probably includea  square function here
        
        if (Input.GetKey(KeyCode.Alpha4))
            x += (0.005f / (1/Time.deltaTime));//probably includea  square function here
    }
 
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void updateMouseOrbit(float distance, Vector3 target)
    {
        if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift))
        {
            
           /* Vector3 targetPos = Vector3.zero;
            if (target)
            {
                targetPos = target.transform.position;
                distance = Vector3.Distance(camObj.transform.position, target.transform.position);
            }
            else
            {
                distance = Vector3.Distance(camObj.transform.position, Vector3.zero);
            }
            */
            
            x += Input.GetAxis("Mouse X") * xSpeed * (distance / 10) * 0.02f;//probably includea  square function here
            y -= Input.GetAxis("Mouse Y") * ySpeed * (distance / 50) * 0.02f;

            //  y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);


            Vector3 position;
            position = rotation * new Vector3(0.0f, 0.0f, -distance) + target;

            transform.LookAt(target);
            transform.position = position;
        }
    }
   
    public void rotateLeft(float distance, Vector3 target)
    {
            
        x -=  0.1f;//probably includea  square function here
          

        //  y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);


        Vector3 position;
        position = rotation * new Vector3(0.0f, 0.0f, -distance) + target;

        transform.LookAt(target);
        transform.position = position;
        
    }
    
    public void rotateRight(float distance, Vector3 target)
    {
            
            x +=  0.1f;//probably includea  square function here
          

            //  y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);


            Vector3 position;
            position = rotation * new Vector3(0.0f, 0.0f, -distance) + target;

            transform.LookAt(target);
            transform.position = position;
        
    }
}

//make it so you can follow an object