using System;
using UnityEngine;

namespace Digger
{
    public class FormController
    {
        private PlayerForm _currentForm;
        private readonly PlayerForm[] _forms;

        public event Action<PlayerForm> OnFormSwitched;

        public FormController(FormsData formsData)
        {
            _forms = new PlayerForm[formsData.AllForms.Length];

            for(int i = 0; i < formsData.AllForms.Length; i++)
            {
                _forms[i] = new PlayerForm(formsData.AllForms[i], i);
            }

        }

        public void SwitchForm(FormType formType)
        {
            for(int i = 0; i < _forms.Length; i++)
            {
                var form = _forms[i];
                if(formType.ToString() == form.FormName)
                {
                    _currentForm = form;
                    OnFormSwitched?.Invoke(form);
                    Debug.Log($"Switched to {_currentForm.FormName}");
                    return;
                }
            }

            Debug.LogWarning("Unknown form type!");
        }

        public void UseCurrentFormAbility()
        {
            _currentForm?.UseAbility();
        }
    }
}
