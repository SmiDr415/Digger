using UnityEngine;

namespace MultiTool
{
    public class PlatformDetector : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _iteractibleWindowIcon;
        [SerializeField] private Sprite _hand;
        public string DeviceType { get; private set; }

        private void Start()
        {
            DeviceType = "Unknown"; // Значение по умолчанию
        }

        // Этот метод вызывается из JavaScript
        public void ReceiveDeviceType(string deviceType)
        {
            DeviceType = deviceType;
            Debug.Log("Device type: " + DeviceType);

            HandleDeviceType(DeviceType);
        }

        private void HandleDeviceType(string deviceType)
        {
            if(deviceType == "Mobile")
            {
                _iteractibleWindowIcon.sprite = _hand;
                // Выполнить действия для мобильного устройства
            }
            else if(deviceType == "Desktop")
            {
                // Выполнить действия для десктопа
            }
        }
    }

}