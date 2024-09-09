using System;
using System.Collections.Generic;

namespace MultiTool
{
    [Serializable]
    public class Upgrade
    {
        public UpgradeType UpgradeType; // Тип улучшения (например, "Speed", "Damage", "Loot")
        public List<UpgradeLevel> Levels; // Список уровней улучшения
    }
}
