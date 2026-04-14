using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace sounds
{
    public class SoundManager : MonoBehaviour
    {
        //================================================================CLASSES

        [Serializable]
        public class PersistentSound
        {
            public AudioSource source;
            public bool shouldPlay;
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private AudioSource audioSourceFX;
        [SerializeField] private AudioMixer audioMixer;
        //================================================================GETTER SETTER

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("VolumeMaster", Mathf.Log10(volume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("VolumeMusic", Mathf.Log10(volume) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            audioMixer.SetFloat("VolumeSFX", Mathf.Log10(volume) * 20);
        }
        
        //================================================================FUNCTIONALITY
        private Dictionary<string, PersistentSound> persistentSounds = new Dictionary<string, PersistentSound>();
        
        public static SoundManager instance;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        public void PlaySoundEffect(AudioClip clip, float volume, Vector3 position)
        {
            AudioSource instancedSFX = InstantiateAudioSource(clip, volume, position, transform);
            instancedSFX.Play();
            
            Destroy(instancedSFX.gameObject, clip.length);
        }

        public void PlaySoundEffect(AudioClip clip, float volume, Transform parent)
        {
            AudioSource instancedSFX = InstantiateAudioSource(clip, volume, Vector3.zero, parent);
            instancedSFX.Play();
            
            Destroy(instancedSFX.gameObject, clip.length);
        }

        public void PlayPersistentSound(AudioClip clip, float volume, Transform parent, string soundName)
        {
            PersistentSound targetPersistentSound;
            if (!persistentSounds.ContainsKey(soundName))
            {
                targetPersistentSound = new PersistentSound();
                targetPersistentSound.source = InstantiateAudioSource(clip, volume, Vector3.zero, parent);
                targetPersistentSound.source.loop = true;
                
                persistentSounds.Add(soundName, targetPersistentSound);
            }
            else
            {
                targetPersistentSound = persistentSounds[soundName];
            }

            if (!targetPersistentSound.source.isPlaying)
            {
                targetPersistentSound.source.gameObject.SetActive(true);
                targetPersistentSound.source.Play();
                targetPersistentSound.shouldPlay = true;
            }
        }

        public void StopPersistentSounds(string soundName)
        {
            if (persistentSounds.ContainsKey(soundName) && persistentSounds[soundName].shouldPlay)
            {
                StartCoroutine(FadePersistentSound(persistentSounds[soundName]));
            }
        }

        private IEnumerator FadePersistentSound(PersistentSound sound)
        {
            sound.shouldPlay = false;
            float initialVolume = sound.source.volume;
            while (sound.source.volume > 0 && !sound.shouldPlay)
            {
                sound.source.volume -= Time.deltaTime * 10;
                yield return null;
            }
            if(!sound.shouldPlay)
                sound.source.Stop();
            sound.source.volume = initialVolume;
            sound.source.gameObject.SetActive(false);
        }

        private AudioSource InstantiateAudioSource(AudioClip clip, float volume, Vector3 position, Transform parent)
        {
            AudioSource instancedAudioSource = Instantiate(audioSourceFX, position, Quaternion.identity, parent);
            
            instancedAudioSource.clip = clip;
            instancedAudioSource.volume = volume;
            return instancedAudioSource;
        }
    }
}
