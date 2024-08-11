using UnityEngine;

namespace MultiTool
{
    public class ScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject _blockedVerticalScreen;

        private int _lastScreenWidth;
        private int _lastScreenHeight;

        private void Start()
        {
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            UpdateScreenOrientation();
        }

        private void Update()
        {
            if(_lastScreenWidth != Screen.width || _lastScreenHeight != Screen.height)
            {
                UpdateScreenOrientation();
                _lastScreenWidth = Screen.width;
                _lastScreenHeight = Screen.height;
            }
        }

        private void UpdateScreenOrientation()
        {
            _blockedVerticalScreen.SetActive(Screen.width < Screen.height);
        }
    }
}
