using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode leftKey;                  
    public KeyCode rightKey;                 
    public KeyCode shootKey;
    public bool canMove = true;

    public GameObject duckPrefab;            
    public Transform duckSpawnPoint;         

    public float forwardForce = 5f;          
    public float rotationSpeed = 100f;       
    public float duckCooldown = 1.5f;          
    public int maxDucks = 6;                

    private Rigidbody2D rb;
    private float duckTimer;
    private int duckCount;

    private bool hasPushedThisPress = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canMove) return;

        HandleMovement();
        HandleDuckRecharge();

        if (Input.GetKeyDown(shootKey) && duckCount > 0)
        {
            ShootDuck();
        }

      
        Debug.DrawRay(transform.position, transform.up * 2f, Color.green);
    }

    private void HandleMovement()
    {
        bool leftPressed = Input.GetKey(leftKey);
        bool rightPressed = Input.GetKey(rightKey);

        // When left and right key been pressed together, fo forward one time
        if (Input.GetKeyDown(leftKey) && rightPressed && !hasPushedThisPress)
        {
            rb.AddForce(transform.up * forwardForce);
            hasPushedThisPress = true;
        }
        else if (Input.GetKeyDown(rightKey) && leftPressed && !hasPushedThisPress)
        {
            rb.AddForce(transform.up * forwardForce);
            hasPushedThisPress = true;
        }

        // when realease any key, reset trigger
        if (!leftPressed || !rightPressed)
        {
            hasPushedThisPress = false;
        }

        // press left or right key can adjust the angle
        if (leftPressed && !rightPressed)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (rightPressed && !leftPressed)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }

        // simulate the water force
        rb.velocity *= 0.95f;
        rb.angularVelocity *= 0.7f;
    }

    private void HandleDuckRecharge()
    {
        duckTimer += Time.deltaTime;
        if (duckTimer >= duckCooldown && duckCount < maxDucks)
        {
            duckCount++;
            duckTimer = 0f;
        }
    }

    private void ShootDuck()
    {
        GameObject duck = Instantiate(duckPrefab, duckSpawnPoint.position, Quaternion.identity);
        duck.GetComponent<Rigidbody2D>().velocity = transform.up * 50f;
        duckCount--;
    }
}
