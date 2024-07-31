using UnityEngine;

namespace MultiTool
{
    public class BlockHitController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _correctToolSound;
        [SerializeField] private AudioClip _wrongToolSound;

        private static readonly int HitBlockHash = Animator.StringToHash("HitBlock");
        private static readonly int HitWrongToolHash = Animator.StringToHash("HitWrongTool");

        public void TileDestroy(bool isCorrectTool, Vector3 pos)
        {
            if(isCorrectTool)
            {
                _animator.transform.position = pos;
                _animator.CrossFade(HitBlockHash, 0.1f);
                PlaySound(_correctToolSound);
            }
            else
            {
                _animator.CrossFade(HitWrongToolHash, 0.1f);
                PlaySound(_wrongToolSound);
            }
        }

        private void PlaySound(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
