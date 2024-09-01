using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    public class ForgeController : MonoBehaviour
    {
        [SerializeField] private Image _formIcon;
        [SerializeField] private Text _currentDamageText;
        [SerializeField] private Text _currentProductionSpeedText;
        [SerializeField] private Text _currentProductionText;

        [SerializeField] private Text _upgradeValueDamageText;
        [SerializeField] private Text _upgradeValueProductionSpeedText;
        [SerializeField] private Text _upgradeValueProductionText;

        [SerializeField] private Text _priceDamageUpgradeText;
        [SerializeField] private Text _priceProductionSpeedUpgradeText;
        [SerializeField] private Text _priceProductionUpgradeText;

        [SerializeField] private Button[] _upgradeButtons;

        private int _defaultPriceUpgrade = 100;
        private int _defaultUpgradeValue = 1;

        private PlayerForm _selectedPlayerForm;

        private FormController _formController;

        private void OnEnable()
        {
            _selectedPlayerForm = PlayerController.Instance.Form;
            UpdateFormInfo();
        }

        private void Start()
        {
            _formController = GameManager.Instance.FormController;
        }


        private void UpdateFormInfo()
        {
            _formIcon.sprite = _selectedPlayerForm.Sprite;

            _currentDamageText.text = _selectedPlayerForm.Damage.ToString();
            _currentProductionSpeedText.text = $"{_selectedPlayerForm.Cooldown}c";
            _currentProductionText.text = _selectedPlayerForm?.Production.ToString();

            _upgradeValueDamageText.text = $"+{_defaultUpgradeValue}";
            _upgradeValueProductionSpeedText.text = $" -0.1c ";
            _upgradeValueProductionText.text = $"+{_defaultUpgradeValue}";

            _priceDamageUpgradeText.text = _defaultPriceUpgrade.ToString();
            _priceProductionSpeedUpgradeText.text = _defaultPriceUpgrade.ToString();
            _priceProductionUpgradeText.text = _defaultPriceUpgrade.ToString();

            for(int i = 0; i < _upgradeButtons.Length; i++)
            {
                Button button = _upgradeButtons[i];
                if(i == 1)
                {
                    var isMaxUpgrade = _selectedPlayerForm.Cooldown < 0.15f;
                    button.interactable = PlayerController.Instance.MoneyAmount >= _defaultPriceUpgrade && !isMaxUpgrade;
                    if(isMaxUpgrade)
                    {
                        
                    }
                }
                else
                {
                    button.interactable = PlayerController.Instance.MoneyAmount >= _defaultPriceUpgrade;
                }

            }
        }

        public void Switch(bool isUp)
        {
            var newFormIndex = isUp ? _selectedPlayerForm.Index + 1 : _selectedPlayerForm.Index - 1;
            var formsCount = _formController.AllForms.Length;
            if(newFormIndex < 0)
            {
                _selectedPlayerForm = _formController.AllForms[formsCount - 1];
            }
            else if(newFormIndex >= formsCount)
            {
                _selectedPlayerForm = _formController.AllForms[0];
            }
            else
            {
                _selectedPlayerForm = _formController.AllForms[newFormIndex];
            }

            UpdateFormInfo();

        }

        public void Upgrade(int numStat)
        {
            switch(numStat)
            {
                case 1:
                    PlayerController.Instance.AddMoney(-_defaultPriceUpgrade);
                    _selectedPlayerForm.DamageUpgrade();
                    break;
                case 2:
                    PlayerController.Instance.AddMoney(-_defaultPriceUpgrade);
                    _selectedPlayerForm.ProductionSpeedUpgrade();
                    break;
                case 3:
                    PlayerController.Instance.AddMoney(-_defaultPriceUpgrade);
                    _selectedPlayerForm.ProductionUpgrade();
                    break;
                default:
                    Debug.Log("error");
                    break;
            }

            UpdateFormInfo();
        }

    }
}

