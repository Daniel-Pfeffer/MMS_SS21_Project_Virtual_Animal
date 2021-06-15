using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isPlaying;

    public void MoveSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void MoveStop()
    {
        audioSource.Stop();
    }
}