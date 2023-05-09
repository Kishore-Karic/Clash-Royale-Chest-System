using ChestSystem.Chest;
using ChestSystem.Service;
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
        [SerializeField] private SlotSaveScriptableObject saveScriptableObject;
        [SerializeField] private int zero;

        public ChestView ChestView { get; private set; }
        public int QueueIndex { get; private set; }

        private void Awake()
        {
            if (button.IsInteractable())
            {
                button.onClick.AddListener(OnButtonClick);
            }
            QueueIndex = saveScriptableObject.QueueIndex;
        }

        private void Start()
        {
            button.interactable = false;

            ChestView = Instantiate(chestPrefab, transform);
            ChestView.gameObject.SetActive(false);
        }

        public void LoadSlotController()
        {
            if (saveScriptableObject.IsSaved)
            {
                ChestService.Instance.CreateChest(false, saveScriptableObject.ChestScriptableObject, ChestView, saveScriptableObject.SlotIndex, this, saveScriptableObject.ChestStatus, saveScriptableObject.IsAddedToQueue, saveScriptableObject.IsUnlocking, saveScriptableObject.QueueIndex);
                chestController.SetRemainingTime(saveScriptableObject.RemainingTime);
                SlotService.Instance.SetSlotRemaining();
                SlotIsTaken();
                saveScriptableObject.IsSaved = false;
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
                saveScriptableObject.IsSaved = true;
                saveScriptableObject.ChestScriptableObject = chestController.GetChestScriptableObject();
                saveScriptableObject.ChestStatus = chestController.CurrentStatus;
                saveScriptableObject.RemainingTime = chestController.UnlockTimeInSeconds;
                saveScriptableObject.SlotIndex = chestController.SlotsControllersListIndex;
                saveScriptableObject.IsAddedToQueue = chestController.IsAddedToQueue;
                saveScriptableObject.IsUnlocking = chestController.IsUnlocking;
                saveScriptableObject.QueueIndex = chestController.QueueIndex;
            }
        }

        public void ResetChestDetails()
        {
            saveScriptableObject.IsSaved = false;
            saveScriptableObject.ChestScriptableObject = null;
            saveScriptableObject.ChestStatus = Enum.ChestStatus.Locked;
            saveScriptableObject.RemainingTime = zero;
            saveScriptableObject.SlotIndex = zero;
            saveScriptableObject.IsAddedToQueue = false;
            saveScriptableObject.IsUnlocking = false;
            saveScriptableObject.QueueIndex = zero;

            if (chestController != null)
            {
                chestController.StopTimer();
            }
            EmptySlot();
        }

        private void OnDestroy()
        {
            StoreChestDetails();
        }
    }
}