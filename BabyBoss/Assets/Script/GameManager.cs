using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject countdownTextObj;
    public Text countdownText;
    public GameObject winTextObj;
    public Text winText;
    public GameObject flagZone;
    public float holdTimeToWin = 10f;

    private bool gameStarted = false;
    private float countdown = 3f;
    private int playerInZone = -1;
    private float holdTimer = 0f;

    void Start()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            player.canMove = false;
        }

        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdown > 0)
        {
            countdownText.text = Mathf.Ceil(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);
        countdownTextObj.SetActive(false);
        gameStarted = true;

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            player.canMove = true;
        }
    }

    void Update()
    {
        if (!gameStarted) return;

        if (playerInZone != -1)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTimeToWin)
            {
                WinGame(playerInZone);
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    public void SetPlayerInZone(int playerId)
    {
        playerInZone = playerId;
    }

    public void ClearPlayerInZone()
    {
        playerInZone = -1;
    }

    void WinGame(int winnerId)
    {
        gameStarted = false;
        winTextObj.SetActive(true);
        winText.text = "Player " + (winnerId + 1) + " Wins!";
    }
}