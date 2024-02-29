using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _ScoreText;

    [SerializeField]
    private TextMeshProUGUI _GameOverText;

    [SerializeField]
    private TextMeshProUGUI[] _GameOverTexts;

    [SerializeField]
    private Image _LivesImage;

    [SerializeField]
    private Sprite[] _LiveSprites;

    private bool GameIsOver = true;
    // Start is called before the first frame update
    void Start()
    {
        ResetUI();
    }

    public void ResetUI()
    {
        if (_LiveSprites != null && _LiveSprites.Length > 0)
            SetPlayerLives(_LiveSprites.Length - 1);
        SetScore(0);
        SetGameOver(false);
    }

    public void EndGame()
    {
        SetGameOver(true);
    }


    public void SetScore(int new_score)
    {
        if (_ScoreText != null)
            _ScoreText.text = "Score: " + new_score;
    }

    public void SetPlayerLives(int lives)
    {
        if (_LiveSprites == null || _LivesImage == null || _LiveSprites.Length == 0)
        {
            Debug.LogError("UI::SetPlayerLives: Error missing object");
            return;
        }

        if (lives >= _LiveSprites.Length)
            lives = _LiveSprites.Length - 1;
        if (lives < 0)
            lives = 0;

        _LivesImage.sprite = _LiveSprites[lives];
    }

    private void SetGameOver(bool IsGameOver)
    {
        GameIsOver = IsGameOver;
        if (_GameOverText == null)
        {
            Debug.LogError("Bad game over text");
            return;
        }
        if (IsGameOver)
        {
            if (_GameOverTexts != null)
                foreach (var text in _GameOverTexts)
                {
                    text.gameObject.SetActive(true);
                }
            StartCoroutine(GameOverFlicker());
        }
        else
        {
            _GameOverText.gameObject.SetActive(false);
            if (_GameOverTexts != null)
                foreach (var text in _GameOverTexts)
                {
                    text.gameObject.SetActive(false);
                }
        }

    }

    IEnumerator GameOverFlicker()
    {
        while (GameIsOver)
        {
            _GameOverText.gameObject.SetActive(!_GameOverText.gameObject.activeSelf);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
