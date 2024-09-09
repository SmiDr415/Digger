using DialogSystem;
using UnityEngine;

namespace MultiTool
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField]
        private FormsData _formsData;
        [SerializeField]
        private DropItemDatabase _dropItemDatabase;
        [SerializeField]
        private UIController _uiController;
        [SerializeField]
        private PlayerController _playerController;
        [SerializeField]
        private GameObject _menuParentObjects;
        [SerializeField]
        private DialogManager _dialogManager;
        [SerializeField]
        private GameObject _settingPanel;
        [SerializeField]
        private GameObject _winPanel;
        [SerializeField]
        private TileMapGenerator _tileMap;
        [SerializeField]
        private LevelController _levelController;

        private FormController _formController;

        public UIController UIController => _uiController;
        public FormController FormController => _formController;
        public DropItemDatabase DropItemDatabase => _dropItemDatabase;


        private bool _isPlaying;
        private bool _isProceduralMode;

        public bool IsPlaying => _isPlaying;

        private void Awake()
        {
            Instance = this;
            InitializeControllers();
        }


        private void InitializeControllers()
        {
            _formController = new FormController(_formsData);
        }


        private void OnEnable()
        {
            SubscribeToEvents();
        }


        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }


        private void Start()
        {
            _formController.SwitchForm(FormType.Form_Pickaxe);
            _formController.GetDamage(0);
            _formController.SwitchForm(FormType.Form_Sickle);
            _formController.GetDamage(0);

            _playerController.gameObject.SetActive(false);
            _menuParentObjects.SetActive(true);
        }


        public void StartGame(bool isProcedural = false)
        {
            _isProceduralMode = isProcedural;
            _uiController.ShowGameUI(true);
            _tileMap.gameObject.SetActive(true);
            //_dialogManager.StartDialog("Осмотрись");
            _menuParentObjects.SetActive(false);
            _isPlaying = true;
            _playerController.gameObject.SetActive(true);

            _formController.RestoreAllForms();
        }

        public void ShowLevelsMenu()
        {
            _isPlaying = false;
            _playerController.transform.position = Vector3.zero;
            _playerController.gameObject.SetActive(false);
            _tileMap.ClearTileMap();
            _tileMap.gameObject.SetActive(false);
            _uiController.ShowMainMenu(true);
        }


        public void Finish()
        {
            _isPlaying = false;
            _playerController.transform.position = Vector3.zero;
            _playerController.gameObject.SetActive(false);
            _tileMap.ClearTileMap();
            _tileMap.gameObject.SetActive(false);
            if(_isProceduralMode)
            {
                _levelController.StartProceduralMapLevel();
            }
            else
            {
                _winPanel.SetActive(true);
                _uiController.ShowMainMenu(true);
                _levelController.LevelComplete();
            }
        }


        public void ShowOptions(bool isShow)
        {
            _settingPanel.SetActive(isShow);
            _uiController.ShowOptionsUI(!isShow && _isPlaying);
            _playerController.gameObject.SetActive(!isShow && _isPlaying);
            //_mainMenu?.SetActive(!isShow && _isPlaying);
            //_menuParentObjects?.SetActive(!isShow && _isPlaying);
        }


        private void SubscribeToEvents()
        {
            _formController.OnGetDamage += _uiController.SetStrengthValue;
        }


        private void UnsubscribeFromEvents()
        {
            _formController.OnGetDamage -= _uiController.SetStrengthValue;
        }

    }
}
