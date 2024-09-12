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
        [SerializeField] private LevelController _levelController;
        [SerializeField] private TileMapGenerator _tileMapGenerator;

        private Queue<Voice> _voices;
        private Coroutine _coroutine;
        private Dialog _currentDialog;

        private void Start()
        {
            _voices = new Queue<Voice>();
        }

        public void StartDialog(string name)
        {
            PlayerController.Instance.gameObject.SetActive(false);
            _tileMapGenerator.ClearTileMap();
            foreach(var d in _dialogList)
            {
                if(d.MissionName == name)
                {
                    _currentDialog = d;
                    break;
                }
            }

            UIController.Instance.ShowDialog();

            _voices.Clear();
            foreach(var v in _currentDialog.Voices)
            {
                _voices.Enqueue(v);
            }

            DisplayNextSentence();
        }

        private void DisplayNextSentence()
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(NextSentence());
        }

        public void FastNext()
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if(_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            if(_voices.Count > 0)
            {
                DisplayNextSentence();
            }
            else
            {
                EndDialog();
            }
        }

        private IEnumerator NextSentence()
        {
            _animator.SetTrigger("Next");

            if(_voices.Count == 0)
            {
                EndDialog();
                yield break;
            }

            var voice = _voices.Dequeue();
            _dialogText.text = voice.Text;
            _characterImage.sprite = voice.PersonImage;
            _audioSource.PlayOneShot(voice.Audio);

            // ����, ���� �������� ����������
            yield return new WaitForSeconds(1.0f);

            // ���� ���������� ��������������� �����
            yield return new WaitWhile(() => _audioSource.isPlaying);

            // ���� ��� �������� ������, ����������
            if(_voices.Count > 0)
            {
                _coroutine = null; // ������� �������� ��� ���������� ������
                DisplayNextSentence();
            }
            else
            {
                EndDialog();
            }
        }

        private void EndDialog()
        {
            _levelController.StartProceduralMapLevel();

            _dialogPanel.SetActive(false);
            PlayerController.Instance.gameObject.SetActive(true);
            if(_currentDialog.MissionName != "�����")
            {
                _missionManager.StartMission(_currentDialog.MissionName);
            }

            _audioSource.Stop();
        }
    }
}
