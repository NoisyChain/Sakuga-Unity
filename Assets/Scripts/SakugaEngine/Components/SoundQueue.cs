using UnityEngine;
using System;

namespace SakugaEngine
{
    public class SoundQueue : MonoBehaviour
    {
        private AudioSource source;
        private bool Queued;

        public void Awake()
        {
            source = GetComponent<AudioSource>();
        }
        public void Update()
        {
            PlayQueue();
        }

        public void SimpleQueueSound()
        {
            Queued = true;
        }

        public void QueueSound(AudioClip sound)
        {
            if (Queued && source.clip == sound) return;

            source.clip = sound;
            Queued = true;
        }

        public void PlayQueue()
        {
            if (!Queued) return;
            if (source.clip == null) return;
            if (source.isPlaying) return;

            source.Play();
            Queued = false;
        }
    }
}
