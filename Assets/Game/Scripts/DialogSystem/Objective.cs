using System;
using UnityEngine;

namespace DialogSystem
{
    [Serializable]
    public class Objective
    {
        [SerializeField] private string _title;
        [SerializeField, TextArea(2, 5)] private string _description;
        [SerializeField] private string[] _tags;
        [SerializeField] private int _prize;
        [SerializeField] private string _nextMission;

        public string Title => _title;
        public string Description => _description;
        public string[] Tags => _tags;
        public int Prize => _prize;
        public string NextMission => _nextMission;
    }
}
