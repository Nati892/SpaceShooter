using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource EffectsAudioSource;

    [SerializeField]
    private AudioClip ExplosionClip;

    [SerializeField]
    private AudioClip PowerUpClip;
    // Start is called before the first frame update
    public enum SoundEffects
    {
        Explosion = 0,
        PowerUp = 1,
    }
    private static AudioManager AManInstance = null;
    public static AudioManager GetInstance()
    {
        if (AManInstance == null)
        {
            AManInstance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        return AManInstance;
    }
    public void PlaySoundEffect(SoundEffects effect)
    {
        if (EffectsAudioSource == null)
            return;
        switch (effect)
        {
            case SoundEffects.Explosion:
                if (ExplosionClip != null)
                    EffectsAudioSource.PlayOneShot(ExplosionClip);
                break;
            case SoundEffects.PowerUp:
                if (PowerUpClip != null)
                    EffectsAudioSource.PlayOneShot(PowerUpClip);
                break;
        }

    }
}
