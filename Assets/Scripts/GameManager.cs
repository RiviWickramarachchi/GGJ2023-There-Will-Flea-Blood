using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int startTime = 60;
    private float currentTimeVal;
    private float playerScore;
    private bool isSucking;
    private float pointIncreasePerSecond = 10f;

    public GameStates currentGameState;
	public enum GameStates
    {
        BEGIN,
		GAME,
		END,
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
        StartGame();
    }

    void OnEnable() {
        PlayerController.OnScoreUpdate += SetSuckingStatus;
    }

    // Update is called once per frame
    void Update()
    {
         if(currentGameState == GameStates.GAME) {
            TimerActions();
            UpdateScore();
        }
    }

    private void StartGame() {
        currentGameState = GameStates.GAME;
    }

    private void EndGame() {
        currentGameState = GameStates.END;
    }

    private void InitializeValues() {
        currentTimeVal = startTime;
        playerScore = 0;
        isSucking = false;
        currentGameState = GameStates.BEGIN;
    }

    public void TimerActions()
    {

        currentTimeVal -= 1 * Time.deltaTime;
        if(currentTimeVal <= 0)
        {
            EndGame();
        }
    }
    public void SetSuckingStatus(bool value) {
        isSucking = value;
    }
    private void UpdateScore() {
        if(!isSucking) {
            return;
        }
        playerScore += pointIncreasePerSecond * Time.deltaTime;
        Debug.Log(playerScore);
    }

    void OnDisable() {
        PlayerController.OnScoreUpdate -= SetSuckingStatus;
    }
}
