using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using th.nx;
/// <summary>
/// 音乐音效
/// </summary>
public class AudioManagerMiniBattle : MonoBehaviour
{
    public static AudioManagerMiniBattle audioManager;
    /// <summary>
    /// 源文件
    /// </summary>
    IDictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    /// <summary>
    /// 声音文件
    /// </summary>
    IDictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    /// <summary>
    /// 声音大小
    /// </summary>
    public float volume = 1.0f;
    /// <summary>
    /// 静音
    /// </summary>
    public bool isMute = false;
    /// <summary>
    /// 音声渐强
    /// </summary>
    public bool smoothIncrease = false;
    /// <summary>
    /// 声音渐弱
    /// </summary>
    public bool smoothDecrease = false;

    // Use this for initialization
    void Start()
    {
        audioManager = this;
        StartCoroutine(loadClips());
    }
    IEnumerator loadClips()
    {
        object[] clips = Resources.LoadAll("sounds");
        yield return clips;
        for (int i = 0; i < clips.Length; i++)
        {
            AudioClip c = clips[i] as AudioClip;
            audioClips.Add(c.name, c);
        }
        playSound(gameObject, "background", true);
    }

    public void playSound(GameObject go, string name, bool loop = false)
    {
        if (audioClips.ContainsKey(name))
        {
            AudioSource source = go.GetComponent<AudioSource>();
            if (source == null)
                source = go.AddComponent<AudioSource>();
            source.clip = audioClips[name];
            source.loop = loop;
            source.volume = volume;
            source.mute = isMute;
            source.Play();

            if (!audioSources.ContainsKey(name))
            {
                audioSources.Add(name, source);
            }
        }
    }

    public void mute()
    {
        foreach (KeyValuePair<string, AudioSource> kv in audioSources)
        {
            kv.Value.mute = true;
        }
    }

    public void setVolume(float volume)
    {
        foreach (KeyValuePair<string, AudioSource> kv in audioSources)
        {
            kv.Value.volume = volume;
        }
    }

    public void stopSound(GameObject go,string name)
    {
        AudioSource source = go.GetComponent<AudioSource>();
        if (source == null) return;
        if (audioClips.ContainsKey(name))
            source.Stop();
    }

    void Update()
    {

    }
}
