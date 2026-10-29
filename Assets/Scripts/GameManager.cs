/*
 * file:        GameManager.cs
 * project:     Chiikawa Defense
 * description: shows the win or lose screen when a tower is destroyed,
 *              freezes the game, and restarts it when the button is pressed.
 */

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // lets any script reach this with GameManager.instance
    public static GameManager instance;

    public GameObject endPanel;          // the win/lose screen, hidden at the start
    public TMP_Text endText;             // says "You Win!" or "You Lose!"

    private bool gameOver;               // so the end screen only shows once

    // runs first. saves this as the shared instance.
    void Awake()
    {
        instance = this;
    }

    // runs once. hides the end screen and makes sure the game isn't frozen.
    void Start()
    {
        Time.timeScale = 1f;

        if (endPanel != null) {
            endPanel.SetActive(false);
        }
    }

    // called by a tower when it hits zero. shows the end screen and freezes the game.
    public void GameOver(bool playerWon)
    {
        if (gameOver == true) {
            return;
        }
        gameOver = true;

        if (endText != null) {
            if (playerWon == true) {
                endText.text = "You Win!";
            } else {
                endText.text = "You Lose!";
            }
        }

        if (endPanel != null) {
            endPanel.SetActive(true);
        }

        if (AudioManager.instance != null) {
            AudioManager.instance.PlayGameOver(playerWon);
        }

        Time.timeScale = 0f;             // freezes everything that uses Time.deltaTime
    }

    // called by the restart button. reloads the level from the start.
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
