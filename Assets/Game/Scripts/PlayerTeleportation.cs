using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiTool
{
    public class PlayerTeleportation : MonoBehaviour
    {
        [Header("Время каста телепортации")]
        [Tooltip("Столько секунд кастуется телепорт в хаб")]
        [Range(0, 10)]
        [SerializeField] private float _teleportationDelay = 3f;
        [SerializeField] private Transform _hubPosition;

        private PlayerAnimation _playerAnimation;
        private bool _isTeleporting = false;
        private Coroutine _teleportCoroutine;

        public bool IsTeleporting => _isTeleporting;

        private void Awake()
        {
            _playerAnimation = GetComponent<PlayerAnimation>();
        }

        public void StartTeleport()
        {
            if(_isTeleporting)
                return;
            _isTeleporting = true;
            _teleportCoroutine = StartCoroutine(TeleportRoutine());
        }

        private IEnumerator TeleportRoutine()
        {
            _playerAnimation.Teleport(true);
            GameManager.Instance.UIController.ShowCancelButton(true);
            yield return new WaitForSeconds(_teleportationDelay);
            if(_isTeleporting)
            {
                //transform.position = _hubPosition.position;
                _isTeleporting = false;
                _playerAnimation.Teleport(false);
                GameManager.Instance.UIController.ShowCancelButton(false);
                GameManager.Instance.Win();
            }
        }

        public void CancelTeleport()
        {
            if(!_isTeleporting)
                return;
            _isTeleporting = false;
            if(_teleportCoroutine != null)
            {
                StopCoroutine(_teleportCoroutine);
                _teleportCoroutine = null;
            }
            _playerAnimation.Teleport(false);
        }
    }
}
