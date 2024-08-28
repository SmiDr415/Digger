using System;
using UnityEngine;

namespace DialogSystem
{
    [Serializable]
    public class Voice
    {
        [SerializeField, TextArea(1, 5)] private string _text;
        [SerializeField] private AudioClip _audio;
        [SerializeField] private Sprite _personImage;
        public string Text => _text;
        public AudioClip Audio => _audio;
        public Sprite PersonImage => _personImage;
    }
}
