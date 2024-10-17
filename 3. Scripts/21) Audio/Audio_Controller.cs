using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Controller : SingleTon<Audio_Controller>
{
    public AudioClip[] bgm_clips;
    public AudioClip[] se_clips;

    private AudioSource[] audio_sources;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        audio_sources = GetComponentsInChildren<AudioSource>();
    }

    #endregion

    #region "Play"

    public void Play_Audio(int audio_type, string audio_name) //0 = bgm, 1 = se
    {
        AudioSource target_audio_source = audio_sources[audio_type];
        AudioClip[] target_clips = audio_type == 0 ? bgm_clips : se_clips;

        foreach (var target_clip in target_clips)
        {
            if (target_clip.name.Equals(audio_name))
            {
                if (audio_type == 0)
                {
                    target_audio_source.clip = target_clip;
                    target_audio_source.Play();
                }
                else
                {
                    target_audio_source.pitch = Game_Time.Get_Game_Time();
                    target_audio_source.PlayOneShot(target_clip);
                }

                Debug_Manager.Debug_In_Game_Message($"{audio_name} is now playing");

                break;
            }
        }
    }

    #endregion

    #region "Set Sound"

    public void Set_Sound(int audio_type, float value)
    {
        audio_sources[audio_type].volume = value;
    }

    #endregion
}