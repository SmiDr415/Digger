using UnityEngine;

namespace MultiTool
{
    public class BlockHitController : MonoBehaviour
    {
        [SerializeField] private Animator _animatorDestroy;
        [SerializeField] private Animator _animatorError;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _correctToolSound;
        [SerializeField] private AudioClip _wrongToolSound;

        private static readonly int HitBlockHash = Animator.StringToHash("HitBlock");
        private static readonly int HitWrongToolHash = Animator.StringToHash("HitWrongTool");

        public void TileDestroy(bool isCorrectTool, Vector3 pos)
        {
            if(isCorrectTool)
            {
                _animatorDestroy.transform.position = pos;
                _animatorDestroy.CrossFade(HitBlockHash, 0.1f);
                PlaySound(_correctToolSound);
            }
            else
            {
                _animatorError.transform.position = pos;
                _animatorError.CrossFade(HitWrongToolHash, 0.1f);
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
