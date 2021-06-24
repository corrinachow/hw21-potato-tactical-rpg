using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text winnerText;
    public void Setup(string winningTeam)
    {
        gameObject.SetActive(true);
        winnerText.text = winningTeam;
    }

    public void PlayAgain() {
        // Create new game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        Application.Quit();
    }

}
