using ChestSystem.Chest;
using ChestSystem.Service;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Slot
{
    public class SlotsController : MonoBehaviour
    {
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private bool isEmpty;
        [SerializeField] private ChestController chestController;
        [SerializeField] private Button button;
        [SerializeField] private int zero;
        [SerializeField] private string saveLocationString;

        public ChestView ChestView { get; private set; }
        public int QueueIndex { get; private set; }

        private void Awake()
        {
            if (button.IsInteractable())
            {
                button.onClick.AddListener(OnButtonClick);
            }

            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                SlotSaveData savedData = JsonUtility.FromJson<SlotSaveData>(json);

                QueueIndex = savedData.QueueIndex;
            }
            else
            {
                QueueIndex = zero;
            }
        }

        private void Start()
        {
            button.interactable = false;

            ChestView = Instantiate(chestPrefab, transform);
            ChestView.gameObject.SetActive(false);
        }

        public void LoadSlotController()
        {
            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                SlotSaveData savedData = JsonUtility.FromJson<SlotSaveData>(json);

                if (savedData.IsSaved)
                {
                    ChestService.Instance.CreateChest(false, savedData.ChestScriptableObject, ChestView, savedData.SlotIndex, this, savedData.ChestStatus, savedData.IsAddedToQueue, savedData.IsUnlocking, savedData.QueueIndex);
                    chestController.SetRemainingTime(savedData.RemainingTime);
                    SlotService.Instance.SetSlotRemaining();
                    SlotIsTaken();
                    savedData.IsSaved = false;
                }
            }
        }
        
        public bool GetIsEmpty() => isEmpty;

        public void SlotIsTaken()
        {
            isEmpty = false;
            ChestView.gameObject.SetActive(true);
            button.interactable = true;
        }

        public void SetChestController(ChestController _chestController)
        {
            chestController = _chestController;
        }

        public void OnButtonClick()
        {
            chestController.ButtonClick();
        }

        public void EmptySlot()
        {
            isEmpty = true;
            chestController = null;
            ChestView.gameObject.SetActive(false);
            button.interactable = false;
        }

        public void StoreChestDetails()
        {
            if (!isEmpty && chestController != null)
            {
                SlotSaveData slotData = new SlotSaveData();
                
                slotData.IsSaved = true;
                slotData.ChestScriptableObject = chestController.GetChestScriptableObject();
                slotData.ChestStatus = chestController.CurrentStatus;
                slotData.RemainingTime = chestController.UnlockTimeInSeconds;
                slotData.SlotIndex = chestController.SlotsControllersListIndex;
                slotData.IsAddedToQueue = chestController.IsAddedToQueue;
                slotData.IsUnlocking = chestController.IsUnlocking;
                slotData.QueueIndex = chestController.QueueIndex;

                string json = JsonUtility.ToJson(slotData, true);
                File.WriteAllText(Application.persistentDataPath + saveLocationString, json);
            }
        }

        public void ResetChestDetails()
        {
            SlotSaveData slotData = new SlotSaveData();

            slotData.IsSaved = false;
            slotData.ChestScriptableObject = null;
            slotData.ChestStatus = Enum.ChestStatus.Locked;
            slotData.RemainingTime = zero;
            slotData.SlotIndex = zero;
            slotData.IsAddedToQueue = false;
            slotData.IsUnlocking = false;
            slotData.QueueIndex = zero;

            string json = JsonUtility.ToJson(slotData, true);
            File.WriteAllText(Application.persistentDataPath + saveLocationString, json);

            if (chestController != null)
            {
                chestController.StopTimer();
            }
            EmptySlot();
        }
    }
}