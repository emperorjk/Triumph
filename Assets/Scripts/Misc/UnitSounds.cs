using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitSounds
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

    public static string typeSelect = "select";
    public static string typeAttack = "attack";
    public static string typeMove = "move";

    private List<AudioClip> audioclips;
    private AudioSource audioSource;



    public void Init()
    {
        audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
        audioclips = Resources.LoadAll<AudioClip>("Sounds/").ToList();
    }

    public void PlaySound(string source)
    {
        audioSource.clip = audioclips.First(x => x.name.Equals(source));
        audioSource.Play();
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