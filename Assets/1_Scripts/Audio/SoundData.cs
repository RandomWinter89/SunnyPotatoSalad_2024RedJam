using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData")]
public class SoundData : ScriptableObject
{
    public string _audioName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    public bool loop;
}
