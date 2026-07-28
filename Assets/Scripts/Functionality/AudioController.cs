using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bg_adudio;
    [SerializeField] internal AudioSource audioPlayer_wl;
    [SerializeField] internal AudioSource audioPlayer_button;
    [SerializeField] internal AudioSource audioPlayer_Spin;
    [SerializeField] private AudioClip SpinButtonClip;
    [SerializeField] private AudioClip SpinClip;
    [SerializeField] private AudioClip BonusDrumClip;
    [SerializeField] private AudioClip Button;
    [SerializeField] private AudioClip Win_Audio;
    [SerializeField] private AudioClip BonusWin_Audio;
    [SerializeField] private AudioClip BonusLose_Audio;
    [SerializeField] private AudioClip NormalBg_Audio;
    [SerializeField] private AudioClip BonusBg_Audio;

    private bool isForceMuted = false;
    private List<AudioSource> allSources;
    private readonly Dictionary<AudioSource, bool> preFocusMuteState = new Dictionary<AudioSource, bool>();

    private void Start()
    {
        allSources = new List<AudioSource> { bg_adudio, audioPlayer_wl, audioPlayer_button, audioPlayer_Spin };
        playBgAudio();
        //audioPlayer_button.clip = clips[clips.Length - 1];
    }

    internal void PlayWLAudio(string type)
    {

        int index = 0;
        switch (type)
        {
            case "spin":
                index = 0;
                break;
            case "win":
                //index = UnityEngine.Random.Range(1, 2);
                audioPlayer_wl.clip = Win_Audio;
                break;
            case "bonuswin":
                audioPlayer_wl.clip = BonusWin_Audio;
                break;
            case "bonuslose":
                audioPlayer_wl.clip = BonusLose_Audio;
                break;

                //index = 3;

        }
        StopWLAaudio();
        //audioPlayer_wl.clip = clips[index];
        //audioPlayer_wl.loop = true;
        audioPlayer_wl.Play();

    }



    private void OnApplicationFocus(bool focus)
    {
        SetMuteAll(!focus);
    }

    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        foreach (var source in allSources)
        {
            if (source == null) continue;
            if (forceMute)
            {
                preFocusMuteState[source] = source.mute;
                source.mute = true;
            }
            else
            {
                source.mute = preFocusMuteState.TryGetValue(source, out bool prevMuted) ? prevMuted : source.mute;
            }
        }
    }

    internal void PlaySpinBonusAudio(string type = "spin")
    {

        if (audioPlayer_Spin)
        {
            if (type == "spin")
            {
                audioPlayer_Spin.clip = SpinClip;

            }
            else if (type == "bonus")
            {

                audioPlayer_Spin.clip = BonusDrumClip;

            }


            audioPlayer_Spin.Play();
        }

    }

    internal void StopApinBonusAudio()
    {

        if (audioPlayer_Spin) audioPlayer_Spin.Stop();

    }
    internal void playBgAudio(string type = "normal")
    {
        //int randomIndex = UnityEngine.Random.Range(0, Bg_Audio.Length);
        if (bg_adudio)
        {
            if (type == "normal")
                bg_adudio.clip = NormalBg_Audio;
            else if (type == "bonus")
                bg_adudio.clip = BonusBg_Audio;

            bg_adudio.Play();
        }

    }

    internal void PlayButtonAudio(string type = "default")
    {

        if (type == "spin")
            audioPlayer_button.clip = SpinButtonClip;
        else
            audioPlayer_button.clip = Button;

        //StopButtonAudio();
        audioPlayer_button.Play();
        //Invoke("StopButtonAudio", audioPlayer_button.clip.length);

    }

    internal void StopWLAaudio()
    {
        audioPlayer_wl.Stop();
        audioPlayer_wl.loop = false;
    }

    internal void StopButtonAudio()
    {

        audioPlayer_button.Stop();

    }


    internal void StopBgAudio()
    {
        bg_adudio.Stop();

    }


    internal void ToggleMute(bool toggle, string type = "all")
    {
        // A real UI mute/unmute click proves interactive focus — it must always win
        // over a stuck forced-mute flag from a missed/unpaired focus-regain signal.
        isForceMuted = false;

        switch (type)
        {
            case "bg":
                bg_adudio.mute = toggle;
                break;
            case "button":
                audioPlayer_button.mute = toggle;
                audioPlayer_Spin.mute = toggle;
                break;
            case "wl":
                audioPlayer_wl.mute = toggle;
                break;
            case "all":
                audioPlayer_wl.mute = toggle;
                bg_adudio.mute = toggle;
                audioPlayer_button.mute = toggle;
                break;
        }
    }

}
