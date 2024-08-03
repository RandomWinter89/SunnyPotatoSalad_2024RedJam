using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public SoundData[] _musicData;
    public SoundData[] _sfxData;

    public AudioSource _musicSource;
    public AudioSource _sfxSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Background Music
    }

    #region Play and Stop
    public void OnAction_MusicAudio (string _name, bool _toActivate)
    {
        SoundData _sound = Array.Find(_musicData, _musicCheck => _musicCheck._audioName == _name);

        if (_sound == null)
        {
            Debug.LogWarning("Error: This MUSIC audio, " + _name + ". NOT FOUND");
            return;
        }

        _musicSource.clip = _sound.clip;
        _musicSource.loop = _sound.loop;
        
        if (_toActivate)
            _musicSource.Play();
        else
            _musicSource.Stop();
    }

    public void OnAction_SFXAudio (string _name, bool _toActivate)
    {
        SoundData _sound = Array.Find(_sfxData, _sfxCheck => _sfxCheck._audioName == _name);

        if (_sound == null)
        {
            Debug.LogWarning("Error: This SFX audio, " + _name + ". NOT FOUND");
            return;
        }
        _sfxSource.clip = _sound.clip;
    
        if (_toActivate)
            _musicSource.Play();
        else
            _musicSource.Stop();
    }
    #endregion

    #region Setup . Toggle and Volume
    public void OnToggle_Music()
    {
        _musicSource.mute = !_musicSource.mute;
    }

    public void OnToggle_SFX()
    {
        _sfxSource.mute = !_sfxSource.mute;
    }
    
    public void OnModified_MusicVolume(float _volume)
    {
        _musicSource.volume = _volume;
    }

    public void OnModified_SFXVolume(float _volume)
    {
        _sfxSource.volume = _volume;
    }
    #endregion
}
