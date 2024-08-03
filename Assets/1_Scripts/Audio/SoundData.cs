using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData")]
public class SoundData : ScriptableObject
{
    public string _audioName;
    public AudioClip clip;
    public bool loop;
}
