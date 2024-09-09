using System;

namespace MultiTool
{
    [Serializable]
    public class UpgradeLevel
    {
        public int Level;          // Уровень улучшения
        public float ChangeValue;  // Изменение значения
        public int Cost;           // Стоимость
    }
}
