using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _ScoreText;

    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetScore(0);
    }

    public void SetScore(int new_score)
    {
        if (_ScoreText != null)
            _ScoreText.text = "Score: " + new_score;
    }

}
