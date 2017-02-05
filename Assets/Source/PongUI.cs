using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PongUI : MonoBehaviour
{
    public Text[] scoresText;
    public Text[] playerNamesText;
    public Button[] difficultyButtons;
    public Animator menuAnimator;

    private int[] lastScore;

    private PongGameState GameState;
    private bool menuVisible = true;

    void Start()
    {
        GameState = FindObjectOfType<PongGameState>();
        lastScore = new int[PongGameState.MAX_PLAYERS];
        OnSelectDifficulty((int)GameState.currentDifficulty);
    }

    void Update()
    {
        for (int p = 0; p < PongGameState.MAX_PLAYERS; p++)
        {
            int CurrentScore = GameState.GetPlayerScore(p);
            if (lastScore[p] != CurrentScore)
            {
                scoresText[p].text = CurrentScore.ToString();
                scoresText[p].GetComponent<Animator>().SetTrigger("OnPlayerScored");
                lastScore[p] = CurrentScore;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuVisible)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }
    }

    public void HideMenu()
    {
        menuVisible = false;
        menuAnimator.SetBool("MenuVisible", menuVisible);
    }

    public void ShowMenu()
    {
        menuVisible = true;
        menuAnimator.SetBool("MenuVisible", menuVisible);
    }

    public void SetPlayerNameText(int playerIndex, string name)
    {
        playerNamesText[playerIndex].text = name;
    }

    public void OnResumeButton()
    {
        HideMenu();
    }

    public void OnStartPlayerGame()
    {
        GameState.StartGame(PongGameType.PlayerAndAI);
        HideMenu();
    }

    public void OnStartAIGame()
    {
        GameState.StartGame(PongGameType.AIOnly);
        HideMenu();
    }

    public void OnSelectDifficulty(int difficultyIndex)
    {
        foreach (Button button in difficultyButtons)
        {
            button.interactable = true;
        }

        difficultyButtons[difficultyIndex].interactable = false;
        GameState.currentDifficulty = (AIDifficulty)difficultyIndex;
    }
}
