using UnityEngine;

namespace MultiTool
{
    public class ScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject _blockedVerticalScreen;

        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
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
            bool isVerticalOrientation = Screen.width < Screen.height;
            _blockedVerticalScreen.SetActive(isVerticalOrientation);
            _camera.depth = isVerticalOrientation ? 1 : 0;
        }
    }
}
