using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class AudioManager
{
    private AudioSource audioSource;
    private Dictionary<UnitTypes, Dictionary<UnitSoundType, AudioClip[]>> soundsDictionary = new Dictionary<UnitTypes, Dictionary<UnitSoundType, AudioClip[]>>();
    private static string jsonString = null;

    public AudioManager()
    {
        ReadJSONAudio();

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

    private void ReadJSONAudio()
    {
        SetJSONString();
        JSONNode jsonUnit = JSON.Parse(jsonString);

        // On Android devices first time they play this file does not exist. Try fails -> create new json
        try 
        {
            InitSoundsFromJSON(jsonUnit);
        }
        catch(NullReferenceException e)
        {
            File.WriteAllText(Application.persistentDataPath + "/audio.json", "{\"masterVolume\":\"0.8\", \"mute\":\"false\"}");
            SetJSONString();
            
            jsonUnit = JSON.Parse(jsonString);
            InitSoundsFromJSON(jsonUnit);
        }
    }

    private void SetJSONString()
    {
        using (StreamReader sr = new StreamReader(Application.persistentDataPath + "/audio.json"))
        {
            jsonString = sr.ReadToEnd();
        }
    }

    private void InitSoundsFromJSON(JSONNode jsonUnit)
    {
        float masterVolume = jsonUnit["masterVolume"].AsFloat;
        bool isMuted = jsonUnit["mute"].AsBool;

        AudioListener.volume = masterVolume;

        if (isMuted)
        {
            AudioListener.volume = 0f;
        }
    }

    public static void MuteAudio(bool mute)
    {
        JSONNode node = JSON.Parse(jsonString);
        node["mute"].AsBool = mute;
        
        File.WriteAllText(Application.persistentDataPath + "/audio.json", node.ToString());

        if (mute)
        {
            AudioListener.volume = 0f;
        }
        else 
        {
            AudioListener.volume = node["masterVolume"].AsFloat;
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