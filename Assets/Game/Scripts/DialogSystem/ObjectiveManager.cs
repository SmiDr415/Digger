using MultiTool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogSystem
{
    public class ObjectiveManager : MonoBehaviour
    {
        [SerializeField] private MissionManager _missionManager;
        [SerializeField] private DialogManager _dialogManager;
        [SerializeField] private GameObject _objectivePanel;
        [SerializeField] private Text _title;
        [SerializeField] private Text _objectiveText;
        [SerializeField] private Text _prizeText;
        [SerializeField] private Button _prizeButton;

        private Objective _currentObjective;
        private bool _isObjectiveComplete;
        private List<string> _complitedTags = new();

        public Objective CurrentObjective => _currentObjective;
        public bool IsObjectiveComplete => _isObjectiveComplete;

        public void SetObjective(Objective objective)
        {
            _complitedTags.Clear();
            _objectivePanel.SetActive(true);
            _currentObjective = objective;
            _isObjectiveComplete = false;
            _prizeText.text = objective.Prize.ToString();
            UpdateObjectiveText();
        }

        public void InitPrizeButoon()
        {
            _prizeButton.onClick.RemoveAllListeners();
            _prizeButton.onClick.AddListener(() =>
            {
                PlayerController.Instance.AddMoney(_currentObjective.Prize);
                _prizeButton.interactable = false;
                _dialogManager.StartDialog(_currentObjective.NextMission);
                _objectivePanel.SetActive(false);
            });
            _prizeButton.interactable = true;
        }


        public void CompleteObjective(string tag)
        {
            if(_complitedTags.Count < _currentObjective.Tags.Length && !_complitedTags.Contains(tag))
            {
                _complitedTags.Add(tag);
            }

            if(_complitedTags.Count >= _currentObjective.Tags.Length && _currentObjective != null)
            {
                _isObjectiveComplete = true;
            }
            UpdateObjectiveText();
        }

        private void UpdateObjectiveText()
        {
            if(_isObjectiveComplete)
            {
                _title.text = "Готово";
                _objectiveText.text = $"Все задания сделаны";
                _missionManager.HandleObjectiveCompleted(_currentObjective.Title);
                _isObjectiveComplete = false;
            }
            else
            {
                _title.text = $"{_currentObjective.Title} {_complitedTags.Count}/{_currentObjective.Tags.Length}";

                var missionText = $"{_currentObjective.Description}";

                foreach(string tag in _currentObjective.Tags)
                {
                    if(_complitedTags.Contains(tag))
                    {
                        missionText += $"\n<color='green'>{tag}</color>";
                    }
                    else
                    {
                        missionText += $"\n{tag}";
                    }
                }
                _objectiveText.text = missionText;
            }
        }
    }
}
