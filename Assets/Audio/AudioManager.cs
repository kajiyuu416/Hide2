using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource morning_bgm;
    [SerializeField] AudioSource day_bgm;
    [SerializeField] AudioSource evening_bgm;
    [SerializeField] AudioSource night_bgm;
    [SerializeField] AudioClip openOptionSe;
    [SerializeField] AudioClip closeOptionSe;
    [SerializeField] AudioClip resetSe;
    [SerializeField] AudioClip selectSe;
    [SerializeField] AudioClip makeoverSe;
    [SerializeField] AudioClip makeover_UnlockSe;
    public AudioClip launch_fireworkSe;
    public AudioClip blast_fireworkSe;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    private AudioSource selectSeAudioSource;
    private GameObject seObj;
    public static AudioManager Instance
    {
        get; private set;
    }
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        audioMixer.GetFloat("BGM_Volume", out float bgmVoium);
        bgmSlider.value = bgmVoium;
        audioMixer.GetFloat("SE_Volume", out float seVoium);
        seSlider.value = seVoium;
        seObj = transform.GetChild(1).gameObject;
        selectSeAudioSource = seObj.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
    }
    public void MorningBGM()
    {
        morning_bgm.Play();
    }
    public void DayBGM()
    {
        day_bgm.Play();
    }
    public void Eveningbgm()
    {
        evening_bgm.Play();
    }
    public void NightBGM()
    {
        night_bgm.Play();
    }
    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGM_Volume", volume);
    }
    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SE_Volume", volume);
    }
    public void OpenOptionSE()
    {
        selectSeAudioSource.PlayOneShot(openOptionSe);
    }
    public void CloseOptionSE()
    {
        selectSeAudioSource.PlayOneShot(closeOptionSe);
    }   
    public void ResetSeSE()
    {
        selectSeAudioSource.PlayOneShot(resetSe);
    }
    public void SelectSE()
    {
        selectSeAudioSource.PlayOneShot(selectSe);
    }
    public void MakeOverSE()
    {
        selectSeAudioSource.PlayOneShot(makeoverSe);
    }
    public void MakeOver_UnlockSE()
    {
        selectSeAudioSource.PlayOneShot(makeover_UnlockSe);
    }
    public void Launch_FireworkSe()
    {
        selectSeAudioSource.PlayOneShot(launch_fireworkSe);
    }
    public void Blast_FireworkSe()
    {
        selectSeAudioSource.PlayOneShot(blast_fireworkSe);
    }

}
