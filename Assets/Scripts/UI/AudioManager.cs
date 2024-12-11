using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource masterSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource enviSource;
    [SerializeField] AudioSource UISource;

    [Header("----- Audio Clip -----")]
    public AudioClip background;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
        /* AudioManager audioManager;
         * private void Awake()
         * {
         *      audioManager = GameObject.FindGameObjectwithTag("Audio").GetComponent<AudioManager>();
         * }
         * 
         * audioManager.PlaySFX(audioManager.name); */
    }

    void Update()
    {
        
    }
}
