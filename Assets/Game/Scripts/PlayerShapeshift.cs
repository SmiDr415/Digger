using System;
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


        public void SwithForm(int index)
        {
            var forms = GameManager.Instance.FormController.AllForms;

            if(index < 0 || index >= forms.Length)
            {
                Debug.LogError($"Invalid index: {index}");
                return;
            }

            // Получаем все значения перечисления FormType
            FormType[] formTypes = (FormType[])Enum.GetValues(typeof(FormType));

            if(_currentForm != forms[index] && index < formTypes.Length)
            {
                SwitchForm(formTypes[index]);
            }
            else
            {
                Debug.LogError($"Invalid form index: {index}");
            }
        }


        private IEnumerator ShapeshiftRoutine()
        {
            _playerAnimation.Shapeshift(true);
            GameManager.Instance.UIController.ShowCancelButton(true);
            yield return new WaitForSeconds(_shapeshiftDelay);
            if(_isShapeshifting)
            {
                _isShapeshifting = false;
                _playerAnimation.Shapeshift(false);
                PerformShapeshift(_targetFormType);
                GameManager.Instance.UIController.ShowCancelButton(false);
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
