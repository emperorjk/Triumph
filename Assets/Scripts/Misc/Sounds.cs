using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Sounds
{
    public static string archerAttack1 = "Archer_attackmove1";
    public static string archerAttack2 = "Archer_attackmove2";
    public static string archerMove1 = "Archer_ordermove1";
    public static string archerMove2 = "Archer_ordermove2";
    public static string archerSelect1 = "Archer_what1";
    public static string archerSelect2 = "Archer_what2";

    public static string knightAttack1 = "knight_attackmove1";
    public static string knightAttack2 = "knight_attackmove2";
    public static string knightMove1 = "knight_ordermove1";
    public static string knightMove2 = "knight_ordermove2";
    public static string knightSelect1 = "knight_what1";
    public static string knightSelect2 = "knight_what2";

    public static string swordsmanAttack1 = "swordsman_attackmove1";
    public static string swordsmanAttack2 = "swordsman_attackmove2";
    public static string swordsmanMove1 = "swordsman_ordermove1";
    public static string swordsmanMove2 = "swordsman_ordermove2";
    public static string swordsmanSelect1 = "swordsman_what1";
    public static string swordsmanSelect2 = "swordsman_what2";

    private AudioSource audioSource;

    public Sounds()
    {
        GameObject camera = GameObject.Find("Main Camera");
        camera.gameObject.AddComponent<AudioSource>();

        audioSource = camera.GetComponent<AudioSource>();
    }

    public void PlaySound(string source)
    {
        AudioClip clip = Resources.Load<AudioClip>(FileLocations.soundsFolder + source);
        audioSource.clip = clip;

        if (!audioSource.isPlaying) 
        {
            audioSource.Play();
        }

    }

    public void PauseSound(AudioSource source)
    {
        if (source.isPlaying)
        {
            source.Pause();
        }
    }

    public void StopSound(AudioSource source)
    {
        if (source.isPlaying) 
        { 
            source.Stop();
        }
    }
}
