using UnityEngine;

namespace MultiTool
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private FormsData _formsData;

        [SerializeField]
        private DropItemDatabase _dropItemDatabase;

        [SerializeField]
        private UIController _uiController;

        private FormController _formController;

        public FormController FormsController => _formController;
        public UIController UIController => _uiController;

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    GameManager[] instances = FindObjectsByType<GameManager>(FindObjectsSortMode.InstanceID);
                    if(instances.Length > 0)
                    {
                        _instance = instances[0];
                    }

                    if(instances.Length > 1)
                    {
                        Debug.LogError("More than one GameManager instance found!");
                    }

                    if(_instance == null)
                    {
                        GameObject singletonObject = new();
                        _instance = singletonObject.AddComponent<GameManager>();
                        singletonObject.name = typeof(GameManager).ToString() + " (GameManagerSingleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return _instance;
            }
        }

        public FormController FormController => _formController;
        public DropItemDatabase DropItemDatabase => _dropItemDatabase;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            InitializeControllers();
        }

        private void InitializeControllers()
        {
            _formController = new FormController(_formsData);
            if(_uiController == null)
            {
                _uiController = FindAnyObjectByType<UIController>();
            }
        }

        private void SubscribeToEvents()
        {
            _formController.OnGetDamage += UIController.Instance.SetStrengthValue;
        }

        private void UnsubscribeFromEvents()
        {
            _formController.OnGetDamage -= UIController.Instance.SetStrengthValue;
        }

        private void Start()
        {
            _formController.SwitchForm(FormType.Form_Pickaxe);
            _formController.GetDamage(0);
            _formController.SwitchForm(FormType.Form_Sickle);
            _formController.GetDamage(0);
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
    }
}
