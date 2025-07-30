using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls player movement, rotation, and duck shooting in water-based physics game.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Keys")]
    public KeyCode leftKey;     // Key to paddle left leg
    public KeyCode rightKey;    // Key to paddle right leg
    public KeyCode shootKey;    // Key to shoot duck

    [Header("Duck Settings")]
    public GameObject duckPrefab;         // Prefab for the duck projectile
    public Transform duckSpawnPoint;      // Where the duck is spawned
    public float duckCooldown = 1.5f;     // Time between each duck regen
    public int maxDucks = 6;              // Max duck count

    [Header("Movement Settings")]
    public float forwardForce = 5f;       // Forward thrust when paddling
    public float rotationSpeed = 100f;    // Rotation speed
    public bool canMove = true;           // Whether player is allowed to move

    [Header("Audio")]
    public AudioClip shootSound;          // Sound when shooting duck
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private float duckTimer;
    private int duckCount;
    private bool hasPushedThisPress = false;

    public AudioClip splashSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!canMove) return;

        HandleMovement();
        HandleDuckRecharge();

        // Handle shooting
        if (Input.GetKeyDown(shootKey) && duckCount > 0)
        {
            ShootDuck();
        }

        // Optional debug line to see direction
        Debug.DrawRay(transform.position, transform.up * 2f, Color.green);
    }

    /// <summary>
    /// Handles movement input and applies force/rotation.
    /// </summary>
    private void HandleMovement()
    {
        bool leftPressed = Input.GetKey(leftKey);
        bool rightPressed = Input.GetKey(rightKey);

        // If both keys are held and one is newly pressed → paddle forward
        if (Input.GetKeyDown(leftKey) && rightPressed && !hasPushedThisPress)
        {
            rb.AddForce(transform.up * forwardForce);
            hasPushedThisPress = true;
            audioSource.PlayOneShot(splashSound);
        }
        else if (Input.GetKeyDown(rightKey) && leftPressed && !hasPushedThisPress)
        {
            rb.AddForce(transform.up * forwardForce);
            hasPushedThisPress = true;
            audioSource.PlayOneShot(splashSound);
        }

        // Reset flag if any key is released
        if (!leftPressed || !rightPressed)
        {
            hasPushedThisPress = false;
        }

        // Rotate player to adjust direction
        if (leftPressed && !rightPressed)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
         
        }
        else if (rightPressed && !leftPressed)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
       
        }

        // Apply drag to simulate water resistance
        rb.velocity *= 0.95f;
        rb.angularVelocity *= 0.7f;
    }

    /// <summary>
    /// Refills duck ammo over time.
    /// </summary>
    private void HandleDuckRecharge()
    {
        duckTimer += Time.deltaTime;
        if (duckTimer >= duckCooldown && duckCount < maxDucks)
        {
            duckCount++;
            duckTimer = 0f;
        }
    }

    /// <summary>
    /// Shoots a duck projectile and plays sound.
    /// </summary>
    private void ShootDuck()
    {
        GameObject duck = DuckPoolManager.Instance.GetDuck(duckSpawnPoint.position, Quaternion.identity);
        duck.GetComponent<Rigidbody2D>().velocity = transform.up * 50f;
        duckCount--;

        // Play shoot sound
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}
