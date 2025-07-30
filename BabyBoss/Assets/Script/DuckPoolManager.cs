using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles object pooling of duck projectiles, reusing them to limit max duck count.
/// </summary>
public class DuckPoolManager : MonoBehaviour
{
    public static DuckPoolManager Instance;

    public GameObject duckPrefab;
    public int poolSize = 50;

    private Queue<GameObject> duckPool = new Queue<GameObject>();

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Pre-instantiate ducks
        for (int i = 0; i < poolSize; i++)
        {
            GameObject duck = Instantiate(duckPrefab);
            duck.SetActive(false);
            duckPool.Enqueue(duck);
        }
    }

    /// <summary>
    /// Fetches a duck from the pool, reusing old ones if needed.
    /// </summary>
    public GameObject GetDuck(Vector3 position, Quaternion rotation)
    {
        GameObject duck = duckPool.Dequeue();

        duck.transform.position = position;
        duck.transform.rotation = rotation;
        duck.SetActive(true);

        // Reset Rigidbody if needed
        Rigidbody2D rb = duck.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        duckPool.Enqueue(duck); // Put back at the end of queue
        return duck;
    }
}
