using UnityEngine;
using UnityEngine.UI;

namespace Digger
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Image[] _formIconsBack;
        [SerializeField] private Color _selectedColor = Color.cyan;
        [SerializeField] private Text[] _formStrenghts;

        private Image _currentIcon;

        public void UpdateFormUI(PlayerForm form)
        {
            if(_currentIcon != null)
                _currentIcon.color = Color.white;
            _currentIcon = _formIconsBack[form.Index];
            _currentIcon.color =_selectedColor;
        }

        public void SetStrenghtValue(int index, int val)
        {
            _formStrenghts[index].text = $"{val}%";
        }
    }
}
