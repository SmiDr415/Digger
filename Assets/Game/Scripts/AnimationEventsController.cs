using UnityEngine;

namespace MultiTool
{
    public class AnimationEventsController : MonoBehaviour
    {
        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] private ParticleSystem m_ParticleSystem;

        public void PlaySfx()
        {
            m_AudioSource.Play();
        }

        public void PlayParticle()
        {
            m_ParticleSystem.Play();
        }
    }

}