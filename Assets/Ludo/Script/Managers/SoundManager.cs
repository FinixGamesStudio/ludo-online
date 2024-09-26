using System.Collections.Generic;
using UnityEngine;

namespace Ludo
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource musicAudioSource;
        public AudioSource soundAudioSource;
        public AudioSource timeAudioSource;
        public AudioSource tokenKillAudioSource;

        public AudioClip tokenMoveAudio;
        public AudioClip trunAudio;
        public AudioClip timerAudio;
        public AudioClip killAudio;
        public AudioClip winAudio;
        public AudioClip loseAudio;
        public AudioClip tokenEnterHomeAudio;
        public AudioClip tokenEnterSafeZoneAudio;
        public AudioClip diceAnimationAudio;
        public static SoundManager instance;

        public AudioSource emojiSoundAudio;
        public List<AudioClip> emojiSoundClip;
        internal int emojiSound = 1;

        private void Awake()
        {
            instance = this;
            MusicPlay();
            PlayerPrefs.GetString("isMusic");
        }

        public void MusicPlay()
        {
            if (PlayerPrefs.GetString("isMusic") == "On")
            {
                musicAudioSource.Play();
            }

        }

        public void SoundPlay(AudioClip audioClip)
        {
            if (PlayerPrefs.GetString("isSound") == "On")
            {
                soundAudioSource.clip = audioClip;
                soundAudioSource.Play();
            }
        }

        public void Vibration()
        {
            if (PlayerPrefs.GetString("isVibrate") == "On")
            {
                Handheld.Vibrate();
            }
        }
        public void TokenKill(AudioClip audioClip)
        {
            if (PlayerPrefs.GetString("isSound") == "On")
            {
                tokenKillAudioSource.clip = audioClip;
                tokenKillAudioSource.Play();
            }
        }

        public void TimeSound(AudioClip timeAudioClip)
        {
            timeAudioSource.clip = timeAudioClip;
            timeAudioSource.loop = true;
            timeAudioSource.Play();

        }

        public void TimeSoundStop(AudioClip timeStopAudioClip)
        {
            timeAudioSource.Stop();
            timeAudioSource.clip = timeStopAudioClip;
            timeAudioSource.loop = false;
        }

        public void EmojiSoundPlay(List<AudioClip> emojiSoundList, int number)
        {
            for (int i = 0; i < emojiSoundList.Count; i++)
            {
                emojiSoundAudio.clip = emojiSoundClip[number];
            }
            if (PlayerPrefs.GetString("isSound") == "On")
            {
                if (emojiSound == 1)
                {
                    Debug.Log("on sound");
                    emojiSoundAudio.Play();
                    emojiSoundAudio.playOnAwake = false;
                    emojiSoundAudio.loop = false;
                }
                else
                {
                    Debug.Log("Off sound");
                    emojiSoundAudio.Stop();
                    emojiSoundAudio.playOnAwake = false;
                }
            }
        }
    }
}
