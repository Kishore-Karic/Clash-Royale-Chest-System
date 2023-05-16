using ChestSystem.Service;
using System.IO;
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

        public bool IsRequireGemAvailable(int costValue)
        {
            if (Gems < costValue)
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

        public void SaveGame()
        {
            GameSaveData gameSave = new GameSaveData();

            gameSave.Coins = Coins;
            gameSave.Gems = Gems;
            gameSave.LastSavedTimeInSeconds = TimeService.Instance.CurrentTime;
            gameSave.LastDate = TimeService.Instance.CurrentDate;
            gameSave.LastMonth = TimeService.Instance.CurrentMonth;
            gameSave.LastYear = TimeService.Instance.CurrentYear;

            string json = JsonUtility.ToJson(gameSave, true);
            File.WriteAllText(Application.persistentDataPath + saveLocationString, json);

            UIService.Instance.GameSaved(Enum.SaveType.GameSave);
        }

        public void ResetGame()
        {
            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                GameSaveData savedData = JsonUtility.FromJson<GameSaveData>(json);

                Coins = defaultCoins;
                Gems = defaultGems;
                RefreshUI();
            }
        }
    }
}