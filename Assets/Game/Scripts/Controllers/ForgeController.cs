using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MultiTool
{
    public partial class ForgeController : MonoBehaviour
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

        private PlayerForm _selectedPlayerForm;
        private FormController _formController;
        private Dictionary<UpgradeType, UpgradeUIElements> _upgradeUIElements;

        private void OnEnable()
        {
            _selectedPlayerForm = PlayerController.Instance.Form;
            InitializeUIElements();
            UpdateFormInfo();
        }

        private void Start()
        {
            _formController = GameManager.Instance.FormController;
        }

        private void InitializeUIElements()
        {
            _upgradeUIElements = new Dictionary<UpgradeType, UpgradeUIElements>
            {
                { UpgradeType.Damage, new UpgradeUIElements(_currentDamageText, _upgradeValueDamageText, _priceDamageUpgradeText, _upgradeButtons[0]) },
                { UpgradeType.Speed, new UpgradeUIElements(_currentProductionSpeedText, _upgradeValueProductionSpeedText, _priceProductionSpeedUpgradeText, _upgradeButtons[1]) },
                { UpgradeType.Loot, new UpgradeUIElements(_currentProductionText, _upgradeValueProductionText, _priceProductionUpgradeText, _upgradeButtons[2]) }
            };
        }

        private void UpdateFormInfo()
        {
            _formIcon.sprite = _selectedPlayerForm.Icon;

            foreach(var upgradeType in _upgradeUIElements.Keys)
            {
                UpdateUpgradeUI(upgradeType);
            }
        }

        private void UpdateUpgradeUI(UpgradeType upgradeType)
        {
            UpgradeUIElements elements = _upgradeUIElements[upgradeType];
            UpgradeLevel upLevel = _selectedPlayerForm.GetCost(upgradeType);
            elements.Button.gameObject.SetActive(upLevel != null);
            elements.CurrentText.text = GetCurrentUpgradeValue(upgradeType);

            if(upLevel != null)
            {
                var oper = upgradeType == UpgradeType.Speed ? string.Empty : "+";
                elements.UpgradeValueText.text = $"{oper}{upLevel.ChangeValue}";
                elements.PriceText.text = upLevel.Cost.ToString();

                bool isInteractable = PlayerController.Instance.MoneyAmount >= upLevel.Cost && !IsMaxUpgrade(upgradeType);
                elements.Button.interactable = isInteractable;

                if(isInteractable)
                {
                    elements.Button.onClick.RemoveAllListeners();
                    elements.Button.onClick.AddListener(() => Upgrade(upgradeType, upLevel.Cost, upLevel.ChangeValue));
                }
            }
        }

        private string GetCurrentUpgradeValue(UpgradeType upgradeType)
        {
            return upgradeType switch
            {
                UpgradeType.Damage => _selectedPlayerForm.Damage.ToString(),
                UpgradeType.Speed => $"{_selectedPlayerForm.Cooldown}c",
                UpgradeType.Loot => _selectedPlayerForm.Production.ToString(),
                _ => string.Empty,
            };
        }

        private bool IsMaxUpgrade(UpgradeType upgradeType)
        {
            if(upgradeType == UpgradeType.Speed)
                return _selectedPlayerForm.Cooldown < 0.15f;
            // Add conditions for other upgrade types if needed
            return false;
        }

        public void Switch(bool isUp)
        {
            var newFormIndex = isUp ? _selectedPlayerForm.Index + 1 : _selectedPlayerForm.Index - 1;
            var formsCount = _formController.AllForms.Length;
            _selectedPlayerForm = _formController.AllForms[(newFormIndex + formsCount) % formsCount];
            UpdateFormInfo();
        }

        public void Upgrade(UpgradeType upgradeType, int cost, float value)
        {
            switch(upgradeType)
            {
                case UpgradeType.Damage:
                    _selectedPlayerForm.DamageUpgrade((int)value);
                    break;
                case UpgradeType.Speed:
                    _selectedPlayerForm.ProductionSpeedUpgrade(value);
                    break;
                case UpgradeType.Loot:
                    _selectedPlayerForm.ProductionUpgrade((int)value);
                    break;
                default:
                    Debug.Log("Unknown upgrade type.");
                    break;
            }
            PlayerController.Instance.AddMoney(-cost);
            UpdateFormInfo();
        }
    }
}
