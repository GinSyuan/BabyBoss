using UnityEngine;

public class ArmAnimator : MonoBehaviour
{
    public KeyCode triggerKey;             
    public float rotateAngle = 30f;        
    public float rotateSpeed = 200f;       

    private Quaternion defaultRotation;
    private Quaternion rotatedRotation;
    private bool keyPressed = false;

    void Start()
    {
        defaultRotation = transform.localRotation;
        rotatedRotation = Quaternion.Euler(0, 0, rotateAngle); 
    }

    void Update()
    {
        
        if (Input.GetKey(triggerKey))
        {
            keyPressed = true;
        }
        else
        {
            keyPressed = false;
        }

       
        Quaternion targetRotation = keyPressed ? rotatedRotation : defaultRotation;
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }
}
