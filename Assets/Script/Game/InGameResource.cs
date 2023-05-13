using System.IO;
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
        [SerializeField] string saveLocationString;
        [SerializeField] private int zero;

        private void Start()
        {
            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                GameSaveData savedData = JsonUtility.FromJson<GameSaveData>(json);

                if (savedData.Coins == zero && savedData.Gems == zero)
                {
                    UpdateResource(defaultCoins, defaultGems);
                }
                else
                {
                    UpdateResource(savedData.Coins, savedData.Gems);
                }
            }
            else
            {
                UpdateResource(defaultCoins, defaultGems);
            }
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
            GameSaveData gameSave = new GameSaveData();
            
            gameSave.Coins = coins;
            gameSave.Gems = gems;

            string json = JsonUtility.ToJson(gameSave, true);
            File.WriteAllText(Application.persistentDataPath + saveLocationString, json);
        }

        public void ResetGame()
        {
            if (File.Exists(Application.dataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                GameSaveData savedData = JsonUtility.FromJson<GameSaveData>(json);

                coins = defaultCoins;
                gems = defaultGems;
                RefreshUI();
            }
        }
    }
}