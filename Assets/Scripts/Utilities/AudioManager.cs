using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    void Awake()
    {
        if(instance != null)
		{
			Debug.LogWarning("More than one instance of Audio manager found");
			return;
		}
		instance = this;
        gameObject.AddComponent<AudioListener>();
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        PlayTheme();
        DontDestroyOnLoad(this.gameObject);
    }
    public void PlayTheme()
    {
        string[] themes = new string[] { "Theme1", "Theme2" };

        Sound s = Play( themes[ UnityEngine.Random.Range(0,themes.Length) ] );
        StartCoroutine( ThemeCountDown(s.source.clip.length) );
    }
    public Sound Play(string name)
    {
        try
        {
            Sound s  = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
            return s;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw e;
        }
    }
    public void Stop(string name)
    {
        try
        {
            Sound s  = Array.Find(sounds, sound => sound.name == name);
            s.source.Stop();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw e;
        }
    }
    public void Mute(string name)
    {
        try
        {
            Sound s  = Array.Find(sounds, sound => sound.name == name);
            s.source.mute = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw e;
        }
    }
    public void Unmute(string name)
    {
        try
        {
            Sound s  = Array.Find(sounds, sound => sound.name == name);
            s.source.mute = false;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw e;
        }
    }
    public bool IsMuted(string name)
    {
        try
        {
            Sound s  = Array.Find(sounds, sound => sound.name == name);
            return s.source.mute;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw e;
        }
    }
    IEnumerator ThemeCountDown(float t)
    {
        yield return new WaitForSeconds(t);
        PlayTheme();
    }

}