using UnityEngine;

public class PlaySoundOnMove : MonoBehaviour
{
    [Header("Assign your sound effect here")]
    public AudioClip moveSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = moveSound;
        audioSource.loop = true; // Ensure the sound continues while the key is held
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Start playing the sound on key press if not already playing
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
             Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) &&
             moveSound != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // Stop playing the sound when any key is released
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            audioSource.Stop();
        }
    }
}
