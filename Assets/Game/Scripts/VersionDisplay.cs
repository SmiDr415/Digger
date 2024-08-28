using UnityEngine;
using UnityEngine.UI;

namespace MultiTool
{
    [ExecuteInEditMode]
    public class VersionDisplay : MonoBehaviour
    {
        private const string VersionKey = "LastLaunchVersion";
        [SerializeField] private Text _versionText; // UI элемент для отображения версии

        private void Awake()
        {
#if UNITY_EDITOR
            UpdateVersion();
            DisplayVersion();
#endif
        }

#if UNITY_EDITOR
        private void UpdateVersion()
        {
            // Получаем текущую дату в формате ддММГГ
            string currentVersion = System.DateTime.Now.ToString("ddMMyy");

            // Сохраняем версию в PlayerPrefs
            PlayerPrefs.SetString(VersionKey, currentVersion);
            PlayerPrefs.Save();
        }

        private void DisplayVersion()
        {
            // Проверяем, установлен ли UI элемент
            if(_versionText != null)
            {
                // Получаем сохраненную версию
                string lastLaunchVersion = PlayerPrefs.GetString(VersionKey, "000000");
                _versionText.text = "Version: " + lastLaunchVersion;
            }
            else
            {
                Debug.LogError("Version Text is not assigned.");
            }
        }
#endif
    }
}
