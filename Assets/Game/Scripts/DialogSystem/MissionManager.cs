using UnityEngine;
using System.Collections;
using MultiTool;

namespace DialogSystem
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField] private ObjectiveManager _objectiveManager;
        [SerializeField] private Mission[] _missions;

        private int _currentMissionIndex = int.MaxValue;
        private int _currentObjectiveIndex;

        public void StartMission(string nameMission)
        {
            for(int i = 0; i < _missions.Length; i++)
            {
                foreach(Objective mission in _missions[i].Objectives)
                {
                    if(mission.Title == nameMission)
                    {
                        _currentMissionIndex = i;
                        break;
                    }

                }
            }

            _currentObjectiveIndex = 0;

            if(_missions.Length > 0 && _currentMissionIndex < int.MaxValue)
            {
                StartMission(_missions[_currentMissionIndex]);
            }
            else
            {
                Debug.Log("not mission");
            }

            if(nameMission == "Осмотрись")
            {
                PlayerInput.OnCursorEdgeReached += CheckTag;
            }
            else if(nameMission == "Управление")
            {
                PlayerInput.OnPlayerMove += CheckTag;
            }

        }

        private void OnDestroy()
        {
            PlayerInput.OnCursorEdgeReached -= CheckTag;
            PlayerInput.OnPlayerMove -= CheckTag;
        }

        private void StartMission(Mission mission)
        {
            if(mission.Objectives.Length > 0)
            {
                _currentObjectiveIndex = 0;
                _objectiveManager.SetObjective(mission.Objectives[_currentObjectiveIndex]);
            }
        }

        private void CheckTag(string tagName)
        {
            var tags = _objectiveManager.CurrentObjective.Tags;

            foreach(var tag in tags)
            {
                if(tag == $"{tagName}")
                {
                    _objectiveManager.CompleteObjective(tag);
                    break;
                }
            }

        }

        public void HandleObjectiveCompleted(string objective)
        {
            if(objective == "Осмотрись")
            {
                PlayerInput.OnCursorEdgeReached -= CheckTag;
            }
            else if(objective == "Управление")
            {
                PlayerInput.OnPlayerMove -= CheckTag;
            }

            StartCoroutine(CompleteCurrentObjective());
        }

        private IEnumerator CompleteCurrentObjective()
        {
            yield return new WaitForSeconds(1); // Можно добавить задержку, если нужно

            //_currentObjectiveIndex++;

            _objectiveManager.InitPrizeButoon();

            if(_currentObjectiveIndex >= _missions[_currentMissionIndex].Objectives.Length)
            {
                //CompleteMission();
            }
            else
            {
                //_objectiveManager.SetObjective(_missions[_currentMissionIndex].Objectives[_currentObjectiveIndex]);
            }
        }

        public void StartNextMission()
        {
            _currentMissionIndex++;

            if(_currentMissionIndex < _missions.Length)
            {
                StartMission(_missions[_currentMissionIndex]);
            }
            else
            {
                Debug.Log("Все миссии завершены!");
                // Дополнительная логика при завершении всех миссий
            }
        }
    }
}
