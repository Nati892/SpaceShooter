using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private GameObject LaserPrefab;

    [SerializeField]
    private GameObject TripleShotPrefab;

    [SerializeField]
    private GameObject ShieldGameObject;

    [SerializeField]
    private float x_speed_modifier = 0.1f;

    [SerializeField]
    private float y_speed_modifier = 0.1f;

    [SerializeField]
    private float Player_bounds_x = 9.0f;

    [SerializeField]
    private float Player_bounds_y = 4.0f;

    [SerializeField]
    private float laser_start_offset = 0.7f;

    [SerializeField]
    private float CoolDownSeconds = 0.5f;

    [SerializeField]
    private int Lives = 3;

    [SerializeField]
    private int Score = 0;

    #endregion

    #region Privates
    private Vector3 current_movement;
    private float NextCanFireTime = 0.0f;
    private SpawnManager spawn_man = null;
    private float axis_input_speed;
    private CapsuleCollider2D this_collider = null;
    private UIManager UIMan = null;
    private GameManager gameManager = null;

    //PowerUps
    private float tripleShot_StopTime;
    private float SpeedBoost_StopTime;
    private float Shields_StopTime;
    private bool IsTripleShotActive = false;
    private float SpeedBoost = 0f;
    private bool ShildsEnabled = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //set start transform
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
        current_movement = new Vector3();

        //retrieve spawn_manager
        spawn_man = GameObject.Find("spawn_manager").GetComponent<SpawnManager>();
        PlayerSpawned();
        tripleShot_StopTime = Time.time;
        if (ShieldGameObject != null)
            ShieldGameObject.SetActive(false);
        ShrinkCollider();
        var obj = GameObject.Find("GUI Canvas");
        if (obj != null)
            UIMan = obj.GetComponent<UIManager>();
        if (gameManager == null)
        {
            var tmp = GameObject.Find("Game_Manager");
            if (tmp != null)
                gameManager = tmp.GetComponent<GameManager>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        CalcMovement();
        HandleFire();
    }

    private void PlayerSpawned()
    {
        if (spawn_man != null)
            spawn_man.ReportPlayerSpawned();
        else
        {
            Debug.LogError("Player::PlayerSpawned, Spawn manager is null");
        }

    }

    void CalcMovement()
    {
        #region movement speed
        //set movement
        axis_input_speed = Input.GetAxis("Horizontal");
        if (axis_input_speed < 0 && axis_input_speed > -0.2)
            axis_input_speed = 0;
        if (axis_input_speed > 0 && axis_input_speed < 0.2)
            axis_input_speed = 0;
        current_movement.x = axis_input_speed * (x_speed_modifier + SpeedBoost) * Time.deltaTime;

        axis_input_speed = Input.GetAxis("Vertical");
        if (axis_input_speed < 0 && axis_input_speed > -0.2)
            axis_input_speed = 0;
        if (axis_input_speed > 0 && axis_input_speed < 0.2)
            axis_input_speed = 0;

        current_movement.y = axis_input_speed * (y_speed_modifier + SpeedBoost) * Time.deltaTime;
        transform.Translate(current_movement);
        #endregion

        #region boundries
        //set boundries
        float curr_x = transform.position.x;
        float curr_y = transform.position.y;
        if (curr_x < -1 * Player_bounds_x)
            transform.position = new Vector3(Player_bounds_x, transform.position.y);

        if (curr_x > Player_bounds_x)
            transform.position = new Vector3(-1 * Player_bounds_x, transform.position.y);

        if (curr_y < -1 * Player_bounds_y)
            transform.position = new Vector3(transform.position.x, -1 * Player_bounds_y);

        if (curr_y > Player_bounds_y)
            transform.position = new Vector3(transform.position.x, Player_bounds_y);
        #endregion
    }

    void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time > NextCanFireTime)
            {
                NextCanFireTime = Time.time + CoolDownSeconds;

                if (!IsTripleShotActive)
                {
                    Instantiate(LaserPrefab, transform.position + new Vector3(0.0f, laser_start_offset, 0f), Quaternion.identity);
                    Debug.Log("Fire triple shot!");
                }
                else
                {
                    Instantiate(TripleShotPrefab, transform.position, Quaternion.identity);
                    Debug.Log("Fire laser!");
                }
            }
        }

    }

    public void RecieveDamage()
    {
        if (!ShildsEnabled)
            Lives--;

        UIMan.SetPlayerLives(Lives);
        if (Lives == 0)
        {
            UIMan.EndGame();
            gameManager.SetGameOverState(true);
            if (spawn_man != null)
                spawn_man.ReportPlayerDeath();
            else
            {
                Debug.LogError("Player::DecLives, Spawn manager is null");
            }
            Destroy(this.gameObject);
        }
    }


    #region PowerUps

    #region triple shot
    public void EnablePowerUp_TripleShot(float CoolDown)
    {
        tripleShot_StopTime = Time.time + CoolDown;
        IsTripleShotActive = true;
        StartCoroutine(TripleShotCoroutine());
    }
    IEnumerator TripleShotCoroutine()
    {
        yield return new WaitForSeconds(5);
        IsTripleShotActive = false;
    }

    #endregion
    #region Speed boost
    internal void EnablePowerUp_SpeedBoost(float CoolDown)
    {
        SpeedBoost_StopTime = Time.time + CoolDown;
        SpeedBoost = 5.0f;
        StartCoroutine(SpeedBoostCoroutine());
    }
    IEnumerator SpeedBoostCoroutine()
    {
        yield return new WaitForSeconds(5);
        SpeedBoost = 0.0f;
    }
    #endregion
    #region Shields
    internal void EnablePowerUp_Shields(float cooldown)
    {
        StartShields(cooldown);
        StartCoroutine(ShieldsCoroutine());
    }
    IEnumerator ShieldsCoroutine()
    {
        yield return new WaitForSeconds(5);
        StopShields();
    }

    private void StartShields(float cooldown)
    {
        Shields_StopTime = Time.time + cooldown;
        ShildsEnabled = true;
        if (ShieldGameObject != null)
        {
            ShieldGameObject.SetActive(true);
        }
        MagnifyCollider();
    }

    private void StopShields()
    {
        ShieldGameObject.SetActive(false);
        ShildsEnabled = false;
        ShrinkCollider();
    }

    private void MagnifyCollider()
    {
        if (this_collider == null)
        {
            this_collider = this.GetComponent<CapsuleCollider2D>();
            if (this_collider == null)
                Debug.LogError("Player::MagnifyCollider cant find Collider2D object");
        }
        if (this_collider == null)
            return;
        this_collider.offset = new Vector2(0f, 0f);
        this_collider.size = new Vector2(6.0f, 7.8f);


    }

    private void ShrinkCollider()
    {
        if (this_collider == null)
        {
            this_collider = this.GetComponent<CapsuleCollider2D>();
            if (this_collider == null)
                Debug.LogError("Player::MagnifyCollider cant find Collider2D object");
        }
        if (this_collider == null)
            return;
        this_collider.offset = new Vector2(0f, 0.2f);
        this_collider.size = new Vector2(2.0f, 3.8f);
    }

    #endregion

    #endregion


    #region UI
    public void AddScore(int score = 0)
    {
        this.Score += score;
        if (UIMan != null)
            UIMan.SetScore(this.Score);
    }

    #endregion
}
