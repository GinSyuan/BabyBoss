using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DuckProjectile : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Slight initial spin
        rb.AddTorque(Random.Range(-20f, 20f));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Small bounce spin when hitting something
        rb.AddTorque(Random.Range(-10f, 10f));
    }
}
