using System;
using UnityEngine;

namespace Digger
{
    public class GUIWindowManager : MonoBehaviour
    {
        public static GUIWindowManager Instance;

        [SerializeField]
        private GameObject _interactionWindow;

        private RectTransform _interactionWindowRectTransform;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _interactionWindowRectTransform = _interactionWindow.GetComponent<RectTransform>();
        }


        public void HideWindow()
        {
            _interactionWindow.SetActive(false);
        }

        public void ShowWindowAbovePosition(Vector3 position)
        {
            _interactionWindow.SetActive(true);
            _interactionWindow.transform.position = position + Vector3.up; 
        }
    }

}