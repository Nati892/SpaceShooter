using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool IsGameOver = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
                IsGameOver = false;
            }

        }
    }

    public void SetGameOverState(bool setState)
    {
        IsGameOver = setState;
    }
}
