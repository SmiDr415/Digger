using System;
using System.Collections.Generic;

namespace MultiTool
{
    public sealed class GameEventManager
    {
        private static GameEventManager _instance;
        public static GameEventManager Instance
        {
            get
            {
                _instance ??= new GameEventManager();
                return _instance;
            }
        }

        private readonly Dictionary<GameEvent, Action> _eventDictionary = new();

        private GameEventManager() { }

        public void Subscribe(GameEvent eventName, Action listener)
        {
            if(_eventDictionary.ContainsKey(eventName))
            {
                _eventDictionary[eventName] += listener;
            }
            else
            {
                _eventDictionary.Add(eventName, listener);
            }
        }

        public void Unsubscribe(GameEvent eventName, Action listener)
        {
            if(_eventDictionary.ContainsKey(eventName))
            {
                _eventDictionary[eventName] -= listener;
            }
        }

        public void TriggerEvent(GameEvent eventName)
        {
            if(_eventDictionary.ContainsKey(eventName))
            {
                _eventDictionary[eventName]?.Invoke();
            }
        }
    }
}
