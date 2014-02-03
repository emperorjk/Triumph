using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitSounds
{
    private AudioSource audioSource;
    private Dictionary<UnitTypes, Dictionary<UnitSoundType, AudioClip[]>> soundsDictionary = new Dictionary<UnitTypes, Dictionary<UnitSoundType, AudioClip[]>>();

    public UnitSounds()
    {
        audioSource = Camera.main.gameObject.AddComponent<AudioSource>();

        foreach (UnitTypes unitType in Enum.GetValues(typeof(UnitTypes)))
        {
            Dictionary<UnitSoundType, AudioClip[]> dictionary = new Dictionary<UnitSoundType, AudioClip[]>();

            foreach (UnitSoundType unitSoundType in Enum.GetValues(typeof(UnitSoundType)))
            {
                dictionary.Add(unitSoundType, Resources.LoadAll<AudioClip>(FileLocations.soundsFolder + unitType.ToString() + "/" + unitSoundType.ToString()).ToArray());
            }
            soundsDictionary.Add(unitType, dictionary);
        }
    }

    public void PlaySound(UnitTypes unitType, UnitSoundType soundType)
    {
        AudioClip[] audioClipArray = soundsDictionary[unitType][soundType];

        System.Random ran = new System.Random();
        int randomNumber = ran.Next(audioClipArray.Length);

        if(audioSource == null)
            audioSource = Camera.main.gameObject.AddComponent<AudioSource>();

        if (!audioSource.isPlaying) 
        {
            audioSource.clip = audioClipArray[randomNumber];
            audioSource.Play();
        }
    }
}