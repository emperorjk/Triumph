using Assets.Scripts.Main;
using Assets.Scripts.Units;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource audioSource;

        private Dictionary<UnitTypes, Dictionary<UnitSoundType, AudioClip[]>> soundsDictionary =
            new Dictionary<UnitTypes, Dictionary<UnitSoundType, AudioClip[]>>();

        private void Awake() 
        {
            ReadJSONAudio();

            audioSource = Camera.main.gameObject.AddComponent<AudioSource>();

            foreach (UnitTypes unitType in Enum.GetValues(typeof(UnitTypes)))
            {
                var dictionary = new Dictionary<UnitSoundType, AudioClip[]>();

                foreach (UnitSoundType unitSoundType in Enum.GetValues(typeof(UnitSoundType)))
                {
                    dictionary.Add(unitSoundType,
                        Resources.LoadAll<AudioClip>(FileLocations.soundsFolder + unitType + "/" +
                                                     unitSoundType).ToArray());
                }
                soundsDictionary.Add(unitType, dictionary);
            }
        }

        private void ReadJSONAudio()
        {
            if (File.Exists(Application.persistentDataPath + "/audio.json"))
            {
                InitSoundsFromJSON(JSON.Parse(GetJSONString()));
            }
            else
            {
                File.WriteAllText(Application.persistentDataPath + "/audio.json",
                    Resources.Load<TextAsset>("JSON/Audio/audio").text);

                InitSoundsFromJSON(JSON.Parse(GetJSONString()));
            }
        }

        private static string GetJSONString()
        {
            using (StreamReader sr = new StreamReader(Application.persistentDataPath + "/audio.json"))
            {
                return sr.ReadToEnd();
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
            JSONNode node = JSON.Parse(GetJSONString());
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

            if (audioSource == null)
                audioSource = Camera.main.gameObject.AddComponent<AudioSource>();

            if (!audioSource.isPlaying)
            {
                audioSource.clip = audioClipArray[randomNumber];
                audioSource.Play();
            }
        }
    }
}