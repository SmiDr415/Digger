using UnityEngine;

namespace MultiTool
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FormsData _formsData;
        [SerializeField] private DropItemDatabase _dropItemDatabase;
        private FormController _formController;

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

        private void Start()
        {
            _formController = new FormController(_formsData);
            PlayerController.Instance.SubscribeToEvents();
            _formController.OnGetDamage += UIController.Instance.SetStrenghtValue;
            _formController.SwitchForm(FormType.Form_Sickle);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(_formController.CurrentForm.Index != (int)FormType.Form_Sickle)
                    PlayerController.Instance.SwitchForm(FormType.Form_Sickle);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                if(_formController.CurrentForm.Index != (int)FormType.Form_Pickaxe)
                    PlayerController.Instance.SwitchForm(FormType.Form_Pickaxe);
            }
        }

        private void OnDestroy()
        {
            _formController.OnGetDamage -= UIController.Instance.SetStrenghtValue;
        }
    }
}
