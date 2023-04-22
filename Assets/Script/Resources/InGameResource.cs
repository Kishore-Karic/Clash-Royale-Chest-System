using TMPro;
using UnityEngine;

namespace ChestSystem.Resource
{
    public class InGameResource : MonoBehaviour
    {
        public int Coins { get; private set; }
        public int Gems { get; private set; }

        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI gemsText;

        public void UpdateResource(int _coins, int _gems)
        {
            Coins += _coins;
            Gems += _gems;

            RefreshUI();
        }

        public void DeductPurchaseCost(int cost)
        {
            Gems -= cost;
            RefreshUI();
        }

        private void RefreshUI()
        {
            coinsText.text = "" + Coins;
            gemsText.text = "" + Gems;
        }
    }
}