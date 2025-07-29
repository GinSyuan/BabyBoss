using UnityEngine;

public class FlagZone : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
            gameManager.SetPlayerInZone(0);
        else if (other.CompareTag("Player2"))
            gameManager.SetPlayerInZone(1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
            gameManager.ClearPlayerInZone();
    }
}