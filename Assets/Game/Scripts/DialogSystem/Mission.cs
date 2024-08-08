using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(fileName = "NewMission", menuName = "Dialog System/Mission")]
    public class Mission : ScriptableObject
    {
        [SerializeField] private Objective[] _objectives;

        public Objective[] Objectives => _objectives;
    }
}
