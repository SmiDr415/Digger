using UnityEngine;

namespace Digger
{
    [CreateAssetMenu(fileName = "FormsData", menuName = "Digger/FormsData", order = 1)]
    public class FormsData : ScriptableObject
    {
        public FormData[] AllForms;
    }
}
