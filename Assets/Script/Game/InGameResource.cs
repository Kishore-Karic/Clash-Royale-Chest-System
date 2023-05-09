using ChestSystem.Game;
using TMPro;
using UnityEngine;

namespace ChestSystem.Resource
{
    public class InGameResource : MonoBehaviour
    {
        private int coins;
        private int gems;

        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI gemsText;

        [SerializeField] private int defaultCoins;
        [SerializeField] private int defaultGems;
        [SerializeField] GameSaveScriptableObject gameSaveScriptableObject;

        private void Start()
        {
            UpdateResource(gameSaveScriptableObject.Coins, gameSaveScriptableObject.Gems);
        }

        public void UpdateResource(int _coins, int _gems)
        {
            coins += _coins;
            gems += _gems;

            RefreshUI();
        }

        public void DeductPurchaseCost(int cost)
        {
            gems -= cost;
            RefreshUI();
        }

        private void RefreshUI()
        {
            coinsText.text = "" + coins;
            gemsText.text = "" + gems;
        }

        public bool IsRequireGemAvailable(int costValue)
        {
            if (gems < costValue)
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

        public void SaveResource()
        {
            gameSaveScriptableObject.Coins = coins;
            gameSaveScriptableObject.Gems = gems;
        }

        public void ResetGame()
        {
            coins = defaultCoins;
            gems = defaultGems;
            RefreshUI();
        }

        private void OnDestroy()
        {
            SaveResource();
        }
    }
}