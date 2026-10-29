/*
 * file:        AudioManager.cs
 * project:     Chiikawa Defense
 * description: plays the music and all the sound effects, and handles the
 *              mute button. other scripts call AudioManager.instance.PlayHit()
 *              and so on.
 */

using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // lets any script reach this with AudioManager.instance
    public static AudioManager instance;

    // static so the game stays muted after pressing Restart
    private static bool muted = false;

    [Header("Music")]
    public AudioClip music;              // background music (leave empty for none)
    public float musicVolume = 0.4f;     // music is quieter than the sound effects

    [Header("Sound Effects")]
    public AudioClip hitSound;           // a unit takes damage
    public AudioClip clickSound;         // spawning a unit or buying an upgrade
    public AudioClip deathSound;         // a unit dies
    public AudioClip winSound;           // enemy cave destroyed
    public AudioClip loseSound;          // your house destroyed
    public float hitVolume = 0.5f;       // hits happen a lot, so keep them softer

    [Header("UI")]
    public TMP_Text muteText;            // the text on the mute button

    private AudioSource musicSource;     // plays the music on a loop
    private AudioSource effectSource;    // plays the sound effects
    private float lastHitTime;           // so lots of hits at once don't get too loud
    private float lastDeathTime;         // same idea for deaths

    // runs first. saves this as the shared instance and makes the speakers.
    void Awake()
    {
        instance = this;
        musicSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    // runs once. starts the music and sets the mute button text.
    void Start()
    {
        if (music != null) {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }

        UpdateMute();
    }

    // called by Unit.cs when a unit gets hit.
    public void PlayHit()
    {
        // skip it if another hit just played
        if (Time.time - lastHitTime < 0.08f) {
            return;
        }
        lastHitTime = Time.time;
        PlaySound(hitSound, hitVolume);
    }

    // called by Unit.cs when a unit dies.
    public void PlayDeath()
    {
        if (Time.time - lastDeathTime < 0.1f) {
            return;
        }
        lastDeathTime = Time.time;
        PlaySound(deathSound, 0.8f);
    }

    // called by the spawn buttons and the upgrade button.
    public void PlayClick()
    {
        PlaySound(clickSound, 1f);
    }

    // called by GameManager when the game ends. stops the music and plays a jingle.
    public void PlayGameOver(bool playerWon)
    {
        musicSource.Stop();

        if (playerWon == true) {
            PlaySound(winSound, 1f);
        } else {
            PlaySound(loseSound, 1f);
        }
    }

    // called by the mute button. turns all sound on or off.
    public void ToggleMute()
    {
        muted = !muted;
        UpdateMute();
    }

    // plays one sound effect. does nothing if no sound was given.
    void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null) {
            return;
        }
        effectSource.PlayOneShot(clip, volume);
    }

    // applies the mute setting and updates the button text.
    void UpdateMute()
    {
        if (muted == true) {
            AudioListener.volume = 0f;
        } else {
            AudioListener.volume = 1f;
        }

        if (muteText != null) {
            if (muted == true) {
                muteText.text = "Sound: Off";
            } else {
                muteText.text = "Sound: On";
            }
        }
    }
}
