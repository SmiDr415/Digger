using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog System/Dialog")]
    public class Dialog : ScriptableObject
    {
        [SerializeField] private string _missionName;
        [SerializeField] private Sprite _characterSprite;
        [SerializeField, TextArea(1, 5)]
        private string[] _sentences;

        public Sprite CharacterSprite => _characterSprite;
        public string[] Sentences => _sentences;
        public string MissionName => _missionName;
    }
}
