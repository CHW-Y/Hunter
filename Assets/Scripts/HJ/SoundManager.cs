using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    // ���带 ���� �о�ͼ� ��ųʸ� ���·� ������ ����
    Dictionary<string, AudioClip> audioSource_;

    [Tooltip("����Ʈ ����� Audio Source ������Ʈ")]
    public AudioSource effectAudioPlayer;

    [Tooltip("��׶��� ����� Audio Source ������Ʈ")]
    public AudioSource backgroundAudioPlayer;

    [ReadOnly]
    [Tooltip("���� �о�� ȿ�������� Key��")]
    public List<string> keyList = new List<string>();

    void Awake()
    {
        audioSource_ = new Dictionary<string, AudioClip>();

        object[] readSounds = Resources.LoadAll("Sounds/Effect/", typeof(AudioClip));

        foreach (object readSound in readSounds)
        {
            audioSource_.Add(readSound.ToString().Split(' ')[0], (AudioClip)readSound);
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
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlayEffectSound("hammer_hit3", 1f);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayEffectSound("headstunattack2", 0.5f);
        }
    }

    /// <summary>
    /// Key�� Volume���� �޾Ƽ� Key���� �ش��ϴ� ���带 ����ϴ� �Լ�.
    /// </summary>
    /// <param name="audioKey">���� Key(string)</param>
    /// <param name="volume">���� ����</param>
    public void PlayEffectSound(string audioKey, float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        if (keyList.Contains(audioKey))
        {
            effectAudioPlayer.PlayOneShot(audioSource_[audioKey], volume);
        }
        else
        {
            print("�ش� Ű���� ���� ����");
        }
    }
}