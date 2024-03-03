using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float Speed = 4;

    [SerializeField]
    private float BottomBoundry = -10.0f;

    [SerializeField]
    private float LeftBoundry = -10.0f;

    [SerializeField]
    private float RightBoundry = 10.0f;

    [SerializeField]
    private bool Die_on_range_end = true;

    private Animator animator;
    private Vector3 StartingPoint;
    private static Player player;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        StartingPoint = transform.position;
        if (player == null)
        {
            var tmp = GameObject.Find("Player2d");
            if (tmp != null)
            {
                player = tmp.GetComponent<Player>();
            }
        }
        if (animator == null)
            animator = GetComponent<Animator>();
        audioManager = AudioManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
        HandleBounds();
    }

    private void HandleBounds()
    {
        if (transform.position.y < BottomBoundry)
        {
            if (!Die_on_range_end)
            {
                var randomX = Random.Range(LeftBoundry, RightBoundry);
                transform.position = new Vector3(randomX, StartingPoint.y, StartingPoint.z); ;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        int score = 0;
        switch (other.tag)
        {
            case "Laser":
                score = 5;
                Destroy(other.gameObject);
                Death();
                break;
            case "Player":
                if (player != null)
                {
                    score = 3;
                    player.RecieveDamage();
                    Death();
                }
                break;
            default:
                return;
        }

        if (player == null)
        {
            Debug.LogError("Enemy::OnTriggerEnter2D: cant find player object to add score to it.");
        }
        else
        {
            player.AddScore(score);
        }

        //Add Score
    }
    private bool IsDead = false;

    private void Death()
    {
  
        Speed = 0;
        if (animator == null)
            return;

        if (IsDead)
            return;
        IsDead = true;

        animator.SetTrigger("OnEnemyDeath");
        if (audioManager != null)
            audioManager.PlaySoundEffect(AudioManager.SoundEffects.Explosion);
        Destroy(this.gameObject, 0.5f);

    }



}
