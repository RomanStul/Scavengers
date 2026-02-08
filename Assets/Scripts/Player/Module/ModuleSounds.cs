using System;
using sounds;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Player.Module
{
    public class ModuleSounds : ModuleBaseScript
    {
        //================================================================CLASSES

        [Serializable]
        public class SoundClip
        {
            public AudioClip clip;
            public float volume;
            public SoundName name;
            public bool persistent;
        }

        public enum SoundName
        {
            Thrusters,
            DrillUse,
            DrillStart
        }
        //================================================================EDITOR VARIABLES
        [SerializeField] private SoundClip[] soundClips;
        //================================================================GETTER SETTER
        //================================================================FUNCTIONALITY


        public void PlaySound(SoundName soundName, Transform parent = null)
        {
            for (int i = 0; i < soundClips.Length; i++)
            {
                if (soundClips[i].name == soundName)
                {
                    if (soundClips[i].persistent)
                    {
                        SoundManager.instance.PlayPersistentSound(soundClips[i].clip, soundClips[i].volume, parent, soundClips[i].name.ToString());
                    }
                    else
                    {
                        SoundManager.instance.PlaySoundEffect(soundClips[i].clip, soundClips[i].volume, parent);
                    }
                }
            }
        }
        
        public void PlaySound(SoundName soundName, Vector3 position = new Vector3())
        {
            for (int i = 0; i < soundClips.Length; i++)
            {
                if (soundClips[i].name == soundName)
                {
                    if (soundClips[i].persistent)
                    {
                        SoundManager.instance.PlayPersistentSound(soundClips[i].clip, soundClips[i].volume, transform, soundClips[i].name.ToString());
                    }
                    else
                    {
                        SoundManager.instance.PlaySoundEffect(soundClips[i].clip, soundClips[i].volume, position);
                    }
                }
            }
        }

        public void StopSound(SoundName soundName)
        {
            SoundManager.instance.StopPersistentSounds(soundName.ToString());
        }
    }
}
