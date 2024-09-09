using UnityEngine.UI;

namespace MultiTool
{
    public partial class ForgeController
    {
        private class UpgradeUIElements
        {
            public Text CurrentText;
            public Text UpgradeValueText;
            public Text PriceText;
            public Button Button;

            public UpgradeUIElements(Text currentText, Text upgradeValueText, Text priceText, Button button)
            {
                CurrentText = currentText;
                UpgradeValueText = upgradeValueText;
                PriceText = priceText;
                Button = button;
            }
        }
    }
}
