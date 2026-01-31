using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip soundW;
    public AudioClip soundA;
    public AudioClip soundS;
    public AudioClip soundD;
    public AudioClip soundSpace;

    void Start()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && soundW != null)
        {
            audioSource.PlayOneShot(soundW);
        }
        if(Input.GetKeyDown(KeyCode.A) && soundA != null)
        {
            audioSource.PlayOneShot(soundA);
        }
        if(Input.GetKeyDown(KeyCode.S) && soundS != null)
        {
            audioSource.PlayOneShot(soundS);
        }
        if(Input.GetKeyDown(KeyCode.D) && soundD != null)
        {
            audioSource.PlayOneShot(soundD);
        }
        if(Input.GetKeyDown(KeyCode.Space) && soundSpace != null)
        {
            audioSource.PlayOneShot(soundSpace);
        }
    }
}