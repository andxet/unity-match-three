using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    TypeSelect,
    TypeMove,
    TypePop,
    TypeGameOver
};

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> clips;
    public static SoundManager Instance;
    AudioSource Source;

    private void Awake()
    {
        Instance = this;
        Source = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType clipType)
    {
        Source.PlayOneShot(clips[(int)clipType]);
    }
}