using UnityEngine;

namespace MultiTool
{
    public class AnimationEventsController : MonoBehaviour
    {
        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] private ParticleSystem m_ParticleSystem;


        private void OnEnable()
        {
            Camera.main.transform.position = Vector3.back * 100;
        }


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