using System;
using UnityEngine;

namespace Digger
{
    public class FormController
    {
        private IForm _currentForm;
        private readonly IForm[] _forms;

        public event Action<int> OnFormSwitched;

        public FormController(FormsData formsData)
        {
            _forms = new IForm[formsData.AllForms.Length];

            for(int i = 0; i < formsData.AllForms.Length; i++)
            {
                _forms[i] = new ScytheForm(formsData.AllForms[i]);
            }

        }

        public void SwitchForm(FormType formType)
        {
            for(int i = 0; i < _forms.Length; i++)
            {
                IForm form = _forms[i];
                if(formType.ToString() == form.FormName)
                {
                    _currentForm = form;
                    OnFormSwitched?.Invoke(i);
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
