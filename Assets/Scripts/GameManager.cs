using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject PlayAgainButton;
    public Text GameDeclare;
    public Text Score;

    private int _score;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        ResetScore();
        GameDeclare.gameObject.SetActive(false);
        Board.Instance.ResetGameData();
        Board.Instance.OnAllSnakeCovered += OnGameWon;
        Snake.Instance.enabled = true;
        Snake.Instance.OnDead += OnGameLost;
        Snake.Instance.OnAte += Scored;
        StartCoroutine(CustomUpdate());
    }

    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Snake.Instance.Move();
        }
    }

    private void StopGame()
    {
        StopAllCoroutines();
        PlayAgainButton.SetActive(true);
        Snake.Instance.OnDead -= StopGame;
        Snake.Instance.OnAte -= Scored;
        Snake.Instance.enabled = false;
    }

    private void OnGameWon()
    {
        GameFinished("You Win !");
    }

    private void OnGameLost()
    {
        GameFinished("You Loose");
    }

    private void GameFinished(string gameStatus)
    {
        GameDeclare.text = gameStatus;
        GameDeclare.gameObject.SetActive(true);
        StopGame();
    }

    private void Scored()
    {
        _score += 1;
        Score.text = _score.ToString();
    }

    private void ResetScore()
    {
        _score = 0;
        Score.text = _score.ToString();
    }
}
