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
        [SerializeField] private AudioSource _audioSource;

        private Queue<Voice> _voices;
        private Coroutine _coroutine;

        private Dialog _currentDialog;

        private void Start()
        {
            _voices = new Queue<Voice>();
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

            _voices.Clear();
            foreach(var v in _currentDialog.Voices)
            {
                _voices.Enqueue(v);
            }

            var voice = _voices.Dequeue();
            _dialogText.text = voice.Text;
            _characterImage.sprite = voice.PersonImage;
            _audioSource.PlayOneShot(voice.Audio);
            Invoke(nameof(DisplayNextSentence), 5.0f);
        }

        public void DisplayNextSentence()
        {
            _coroutine ??= StartCoroutine(NextSentence());
        }


        public void FastNext()
        {
            StopAllCoroutines();
            _coroutine = StartCoroutine(NextSentence());
        }


        private IEnumerator NextSentence()
        {

            _animator.SetTrigger("Next");
            if(_voices.Count == 0)
            {
                EndDialog();
                yield break;
            }

            yield return new WaitForSeconds(1.0f);
            var voice = _voices.Dequeue();
            _dialogText.text = voice.Text;
            _characterImage.sprite = voice.PersonImage;
            _audioSource.Stop();
            _audioSource.PlayOneShot(voice.Audio);

            yield return new WaitWhile(() => _audioSource.isPlaying);
            {
                if(_voices.Count > 0 && _coroutine == null)
                {
                    DisplayNextSentence();
                }
            }
            _coroutine = null;
        }

        private void EndDialog()
        {
            _dialogPanel.SetActive(false);
            PlayerController.Instance.gameObject.SetActive(true);
            if(_currentDialog.MissionName != "Конец")
            {
                _missionManager.StartMission(_currentDialog.MissionName);
                _audioSource.Stop();

            }
        }
    }
}
