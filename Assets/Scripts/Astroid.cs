using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float VectorSpeed = 3.0f;

    [SerializeField]
    private float RotationSpeedDegrees = 30.0f;

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject ExplosionFX;

    private SpawnManager SpawnManager;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {

        player = GetComponent<Player>();
        var tmp = GameObject.Find("spawn_manager");
        if (tmp != null)
            SpawnManager = tmp.GetComponent<SpawnManager>();

        audioManager = AudioManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.back * RotationSpeedDegrees * Time.deltaTime);
        // this.transform.Translate(Vector3.down * VectorSpeed * Time.deltaTime);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Laser":
                Destroy(collision.gameObject);
                StartSpawnManager();
                Death();
                break;
            case "Player":
                if (player != null)
                {
                    player.RecieveDamage();
                    StartSpawnManager();
                    Death();
                }
                break;
            default:
                return;
        }
    }

    private bool IsDead = false;
    void Death()
    {
        if (IsDead)
            return;
        IsDead = true;
        if (audioManager != null)
            audioManager.PlaySoundEffect(AudioManager.SoundEffects.Explosion);
        if (ExplosionFX != null)
        {
            var exp = Instantiate(ExplosionFX, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0f);
        }
        else
        {
            Destroy(this.gameObject, 0f);
        }
    }
    void StartSpawnManager()
    {
        if (SpawnManager != null)
            SpawnManager.StartSpwaning();
    }
}


