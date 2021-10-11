using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    // 사운드를 전부 읽어와서 딕셔너리 형태로 변수에 저장
    Dictionary<string, AudioClip> audioSource_;

    [Tooltip("이펙트 사운드용 Audio Source 컴포넌트")]
    public AudioSource effectAudioPlayer;

    [Tooltip("백그라운드 사운드용 Audio Source 컴포넌트")]
    public AudioSource backgroundAudioPlayer;

    [Tooltip("전투 사운드용 Audio Source 컴포넌트")]
    public AudioSource battleBGMPlayer;
    public AudioClip completeBGM;
    bool isBattle;

    [ReadOnly]
    [Tooltip("현재 읽어온 효과음들의 Key값")]
    public List<string> keyList = new List<string>();

    void Awake()
    {
        audioSource_ = new Dictionary<string, AudioClip>();

        object[] readSounds = Resources.LoadAll("Sounds/Effect/", typeof(AudioClip));

        foreach (AudioClip readSound in readSounds)
        {
            audioSource_.Add(readSound.ToString().Split(' ')[0], readSound);
        }

        foreach (KeyValuePair<string, AudioClip> audios in audioSource_)
        {
            keyList.Add(audios.Key);
            print(audios.Key);
        }
    }

    private void Start()
    {
        if (effectAudioPlayer == null) effectAudioPlayer = transform.Find("Effect").GetComponent<AudioSource>();
        if (backgroundAudioPlayer == null) backgroundAudioPlayer = transform.Find("BGM").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            BattleOff();
        }
    }


    /// <summary>
    /// Key와 Volume값을 받아서 Key값에 해당하는 사운드를 재생하는 함수.
    /// </summary>
    /// <param name="audioKey">사운드 Key(string)</param>
    /// <param name="volume">사운드 볼륨</param>
    public void PlayEffectSound(string audioKey, float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        if (keyList.Contains(audioKey))
        {
            effectAudioPlayer.PlayOneShot(audioSource_[audioKey], volume);
        }
        else
        {
            print("해당 키값의 사운드 없음");
        }
    }

    /// <summary>
    /// 전투가 개시될 때 SoundManager에서 전투 BGM을 실행시키는 함수
    /// </summary>
    public void BattleOn()
    {
        if (!isBattle)
        {
            backgroundAudioPlayer.volume = 0.2f;
            battleBGMPlayer.Play();
            isBattle = true;
        }
    }

    public void BattleOff()
    {
        isBattle = false;
        battleBGMPlayer.Stop();
        battleBGMPlayer.clip = completeBGM;
        battleBGMPlayer.Play();
    }
}