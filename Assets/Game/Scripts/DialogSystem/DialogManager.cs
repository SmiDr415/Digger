using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using MultiTool;

namespace DialogSystem
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogPanel;
        [SerializeField] private Text _dialogText;
        [SerializeField] private Image _characterImage;
        [SerializeField] private Dialog[] _dialogList;
        [SerializeField] private ObjectiveManager _objectiveManager;
        [SerializeField] private Animator _animator;
        [SerializeField] private MissionManager _missionManager;

        private Queue<string> _sentences;
        private Coroutine _coroutine;

        private Dialog _currentDialog;

        private void Start()
        {
            _sentences = new Queue<string>();
        }

        public void StartDialog(string name)
        {
            foreach(var d in _dialogList)
            {
                if(d.MissionName == name)
                {
                    _currentDialog = d;
                }
            }

            PlayerController.Instance.gameObject.SetActive(false);
            _dialogPanel.SetActive(true);
            _characterImage.sprite = _currentDialog.CharacterSprite;

            _sentences.Clear();
            foreach(string sentence in _currentDialog.Sentences)
            {
                _sentences.Enqueue(sentence);
            }

            _dialogText.text = _sentences.Dequeue();

            Invoke(nameof(DisplayNextSentence), 3.0f);
        }

        public void DisplayNextSentence()
        {
            _coroutine ??= StartCoroutine(NextSentence());
        }


        private IEnumerator NextSentence()
        {

            _animator.SetTrigger("Next");
            if(_sentences.Count == 0)
            {
                EndDialog();
                yield break;
            }

            yield return new WaitForSeconds(1.0f);
            string sentence = _sentences.Dequeue();
            _dialogText.text = sentence;
            _coroutine = null;

            yield return new WaitForSeconds(3.0f);
            {
                if(_sentences.Count > 0 && _coroutine == null)
                {
                    DisplayNextSentence();
                }
            }
        }

        private void EndDialog()
        {
            _dialogPanel.SetActive(false);
            PlayerController.Instance.gameObject.SetActive(true);
            if(_currentDialog.MissionName != "Конец")
            {
                _missionManager.StartMission(_currentDialog.MissionName);
            }
        }
    }
}
