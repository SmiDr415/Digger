using UnityEngine;

namespace MultiTool
{
    public class TileGOController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void InitAnimation(RuntimeAnimatorController runtimeAnimatorController)
        {
            _animator.runtimeAnimatorController = runtimeAnimatorController;
            _animator.gameObject.SetActive(true);
        }
    }

}