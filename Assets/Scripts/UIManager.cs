using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverPanel, drawGamePanel, AIWonPanel, playerWonPanel, gameBoard, quitGamePanel, settingsPanel;

    void Start()
    {
        TurnOffAllPanels();
    }

    void TurnOffAllPanels()
    {
        gameBoard.SetActive(true);
        gameOverPanel.SetActive(false);
        drawGamePanel.SetActive(false);
        AIWonPanel.SetActive(false);
        playerWonPanel.SetActive(false);
    }

    public void GameRestart()
    {
        TTTGameManager.Instance.GameRestart();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ShowGameOverPanel()
    {
        gameBoard.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void DisplayAIWonPanel()
    {
        AIWonPanel.SetActive(true);
    }

    public void DisplayPlayerWonPanel()
    {
        playerWonPanel.SetActive(true);
    }

    public void DisplayDrawGamePanel()
    {
        drawGamePanel.SetActive(true);
    }

    public void DontQuitGame()
    {

    }

    public void DisplaySettingsPanel()
    {
        gameBoard.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        gameBoard.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
