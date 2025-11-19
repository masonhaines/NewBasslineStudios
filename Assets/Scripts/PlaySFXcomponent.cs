using UnityEngine;

public class PlaySFXcomponent : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip sound;

    public void PlaySound()
    {
        if (sfxSource == null || sound == null)
        {
            Debug.LogWarning($"SingleSfxPlayer on {gameObject.name} is missing sfxSource or sound reference.");
            return;
        }

        sfxSource.PlayOneShot(sound);
    }
}
