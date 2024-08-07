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
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
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
}
