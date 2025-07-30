using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Countdown UI")]
    public GameObject countdownTextObj;
    public Text countdownText;
    public Text holdCountdownText;

    [Header("Win UI")]
    public GameObject winPanel;
    public GameObject winTextObj;
    public Text winText;
    public Button playAgainButton;
    public Button mainMenuButton;

    [Header("Gameplay Settings")]
    public float holdTimeToWin = 3f;

    [Header("Audio")]
    public AudioClip countdownTickSound;
    public AudioClip countdownEndSound;
    public AudioClip backgroundMusic;

    private bool gameStarted = false;
    private float countdown = 3f;
    private int playerInZone = -1;
    private float holdTimer = 0f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        countdownTextObj.SetActive(true);

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            player.canMove = false;
        }

        winPanel.SetActive(false);
        winTextObj.SetActive(false);

        if (holdCountdownText != null)
            holdCountdownText.text = "";

        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdown > 0)
        {
            countdownText.text = Mathf.Ceil(countdown).ToString();

            if (countdownTickSound != null)
                audioSource.PlayOneShot(countdownTickSound);

            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        countdownText.text = "Go!";

        if (countdownEndSound != null)
            audioSource.PlayOneShot(countdownEndSound);

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
            float secondsLeft = Mathf.Max(0f, holdTimeToWin - holdTimer);

            if (holdCountdownText != null)
                holdCountdownText.text = "🏁 Holding: " + Mathf.Ceil(secondsLeft).ToString() + "s";

            if (holdTimer >= holdTimeToWin)
            {
                WinGame(playerInZone);
                if (holdCountdownText != null)
                    holdCountdownText.text = "";
            }
        }
        else
        {
            holdTimer = 0f;
            if (holdCountdownText != null)
                holdCountdownText.text = "";
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

        winText.text = "Player " + (winnerId + 1) + " Wins!";
        winTextObj.SetActive(true);
        winPanel.SetActive(true);

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            player.canMove = false;
        }

        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(RestartGame);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
