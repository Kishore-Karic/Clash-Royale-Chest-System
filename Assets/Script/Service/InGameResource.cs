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

        [SerializeField] private int coins;
        [SerializeField] private int gems;

        private void Start()
        {
            UpdateResource(coins, gems);
        }

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

        public bool IsRequireGemAvailable(int value)
        {
            if (Gems < value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdatePurchaseCost(int cost)
        {
            DeductPurchaseCost(cost);
        }

        public void AddRewards(int coins, int gems)
        {
            UpdateResource(coins, gems);
        }
    }
}