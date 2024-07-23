using System.Collections;
using UnityEngine;

namespace MultiTool
{
    public class PlayerShapeshift : MonoBehaviour
    {
        [Header("Время смены формы")]
        [Tooltip("Столько секунд герой сменяет форму")]
        [Range(0, 10)]
        [SerializeField] private float _shapeshiftDelay = 3f;

        private PlayerAnimation _playerAnimation;
        private bool _isShapeshifting = false;
        private Coroutine _shapeshiftCoroutine;
        private FormType _targetFormType;
        private PlayerForm _currentForm;

        public bool IsShapeshifting => _isShapeshifting;

        private void OnValidate()
        {
            _shapeshiftDelay = Mathf.Clamp(_shapeshiftDelay, 0, 10);
        }

        private void Awake()
        {
            _playerAnimation = GetComponent<PlayerAnimation>();
        }

        public void InitializeForm()
        {
            _currentForm = GameManager.Instance.FormController?.CurrentForm;
        }

        public void SwitchForm(FormType formType)
        {
            if(_currentForm != null && formType.ToString() == _currentForm.FormName)
                return;
            if(_isShapeshifting)
                return;

            _targetFormType = formType;
            _isShapeshifting = true;
            _shapeshiftCoroutine = StartCoroutine(ShapeshiftRoutine());
        }

        private IEnumerator ShapeshiftRoutine()
        {
            _playerAnimation.Shapeshift(true);
            yield return new WaitForSeconds(_shapeshiftDelay);
            if(_isShapeshifting)
            {
                PerformShapeshift(_targetFormType);
                _isShapeshifting = false;
                _playerAnimation.Shapeshift(false);
            }
        }

        private void PerformShapeshift(FormType formType)
        {
            GameManager.Instance.FormController.SwitchForm(formType);
        }

        public void CancelShapeshift()
        {
            if(!_isShapeshifting)
                return;
            _isShapeshifting = false;
            if(_shapeshiftCoroutine != null)
            {
                StopCoroutine(_shapeshiftCoroutine);
                _shapeshiftCoroutine = null;
            }
            _playerAnimation.Shapeshift(false);
        }
    }
}
