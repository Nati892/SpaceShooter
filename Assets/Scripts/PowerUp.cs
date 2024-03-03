using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpId
    {
        TripleShot = 0,
        SpeedBoost = 1,
        Shields = 2,
    }

    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float YBounds = -14f;

    [SerializeField]
    private PowerUpId PowerUpType = PowerUpId.TripleShot;

    private AudioManager AMan;


    void Start()
    {
        AMan = AudioManager.GetInstance();
    }

    void Update()
    {
        HandleMovement();
        HandleBounds();
    }

    private void HandleMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void HandleBounds()
    {
        if (transform.position.y < YBounds)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (PowerUpType)
                {
                    case PowerUpId.TripleShot:
                        player.EnablePowerUp_TripleShot(5f);
                        break;
                    case PowerUpId.SpeedBoost:
                        player.EnablePowerUp_SpeedBoost(5f);
                        break;
                    case PowerUpId.Shields:
                        player.EnablePowerUp_Shields(5f);
                        break;
                }
                AMan.PlaySoundEffect(AudioManager.SoundEffects.PowerUp);
            }
            Destroy(this.gameObject);
        }
    }

}
