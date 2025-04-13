using System.Collections.Generic;
using UnityEngine;

public class AudioManager : DontDestroySingleton<AudioManager>
{
    [SerializeField] private AudioSource audioSourceBGM; // BGMのスピーカー
    [SerializeField] private AudioClip[] audioClipsBGM; // BGMの音源
    private Dictionary<string, int> _BGMTable = new Dictionary<string, int>();
    
    [SerializeField] private AudioSource audioSourceSE; // SEのスピーカー
    [SerializeField] private AudioClip[] audioClipsSE; // SEの音源
    private Dictionary<string, int> _SETable = new Dictionary<string, int>();

    public override void Awake()
    {
        base.Awake();
        if (IsAudioSourceNull(audioSourceBGM) || IsAudioSourceNull(audioSourceSE))
        {
            return;
        }
        
        for (int i = 0; i < audioClipsBGM.Length; i++)
        {
            _BGMTable.Add(audioClipsBGM[i].name, i);
        }
        for (int i = 0; i < audioClipsSE.Length; i++)
        {
            _SETable.Add(audioClipsSE[i].name, i);
        }
    }

    public void PlayBGM(string BGMName, float volume = 1f)
    {
        if (IsAudioSourceNull(audioSourceBGM))
        {
            return;
        }
        
        int bgmIndex = _BGMTable[BGMName];
        AudioClip bgmClip = audioClipsBGM[bgmIndex];

        // BGMを設定して再生
        audioSourceBGM.clip = bgmClip;
        audioSourceBGM.volume = volume;
        audioSourceBGM.Play();
    }

    public void StopBGM()
    {
        if (IsAudioSourceNull(audioSourceBGM))
        {
            return;
        }
        
        audioSourceBGM.Stop();
    }
    
    public void PlaySE(string SEName, float volume = 1f)
    {
        if (IsAudioSourceNull(audioSourceSE))
        {
            return;
        }
        
        int seIndex = _SETable[SEName];
        AudioClip seClip = audioClipsSE[seIndex];
        audioSourceSE.PlayOneShot(seClip, volume);
    }

    public void StopSE()
    {
        if (IsAudioSourceNull(audioSourceSE))
        {
            return;
        }
        
        audioSourceSE.Stop();
    }
    
    /// <summary>
    /// デバッグ用にnullを防ぐ
    /// </summary>
    private bool IsAudioSourceNull(AudioSource audioSource)
    {
        bool isNull = audioSource == null;
        if (isNull)
        {
            Debug.LogWarning("audio source is null");
        }
        return isNull;
    }
}