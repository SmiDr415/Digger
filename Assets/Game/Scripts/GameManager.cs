using UnityEngine;

namespace Digger
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FormsData _formsData;
        [SerializeField] private UIController _uiController;
        [SerializeField] private PlayerController _playerController;
        private FormController _formController;

        private void Start()
        {
            _formController = new FormController(_formsData);
            _formController.OnFormSwitched += _uiController.UpdateFormUI;
            _formController.OnFormSwitched += _playerController.ChangePlayerSprite;

            _formController.SwitchForm(FormType.Form_Sickle);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _formController.UseCurrentFormAbility();
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                _formController.SwitchForm(FormType.Form_Sickle);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                _formController.SwitchForm(FormType.Form_Pickaxe);
            }
        }

        private void OnDestroy()
        {
            _formController.OnFormSwitched -= _uiController.UpdateFormUI;
            _formController.OnFormSwitched -= _playerController.ChangePlayerSprite;
        }
    }
}
