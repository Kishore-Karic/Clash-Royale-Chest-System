using ChestSystem.Chest;
using ChestSystem.Service;
using System;
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
        [SerializeField] private int one;
        [SerializeField] private int two;

        public ChestView ChestView { get; private set; }
        public int QueueIndex { get; private set; }

        private void Awake()
        {
            if (button.IsInteractable())
            {
                button.onClick.AddListener(OnButtonClick);
            }

            button.interactable = false;

            ChestView = Instantiate(chestPrefab, transform);
            ChestView.gameObject.SetActive(false);

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

        public void LoadSlotController()
        {
            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                SlotSaveData savedData = JsonUtility.FromJson<SlotSaveData>(json);

                if (savedData.IsSaved)
                {
                    bool isChestUnlocking = false;
                    if (savedData.QueueIndex == two && SlotService.Instance.FirstChestUnlocked)
                    {
                        ChestService.Instance.CreateChest(false, savedData.ChestScriptableObject, ChestView, savedData.SlotIndex, this, Enum.ChestStatus.Unlocking, savedData.IsAddedToQueue, true, one);
                        isChestUnlocking = true;
                    }
                    else if(savedData.QueueIndex == one)
                    {
                        ChestService.Instance.CreateChest(false, savedData.ChestScriptableObject, ChestView, savedData.SlotIndex, this, savedData.ChestStatus, savedData.IsAddedToQueue, savedData.IsUnlocking, savedData.QueueIndex);
                        isChestUnlocking = savedData.IsUnlocking;
                    }
                    else
                    {
                        ChestService.Instance.CreateChest(false, savedData.ChestScriptableObject, ChestView, savedData.SlotIndex, this, savedData.ChestStatus, savedData.IsAddedToQueue, savedData.IsUnlocking, savedData.QueueIndex);
                        isChestUnlocking = false;
                    }

                    if (isChestUnlocking && savedData.ChestStatus != Enum.ChestStatus.Unlocked)
                    {
                        float tempRemainingChestTime, checkWithRemainingTime;

                        if(savedData.QueueIndex == two)
                        {
                            if(savedData.TotalUnlockDuration <= TimeService.Instance.RemainingTime)
                            {
                                tempRemainingChestTime = zero;
                                checkWithRemainingTime = TimeService.Instance.RemainingTime - savedData.TotalUnlockDuration;
                            }
                            else
                            {
                                tempRemainingChestTime = MathF.Abs(savedData.TotalUnlockDuration - TimeService.Instance.RemainingTime);
                                checkWithRemainingTime = TimeService.Instance.RemainingTime - tempRemainingChestTime;
                            }
                        }
                        else
                        {
                            if (savedData.RemainingTime <= TimeService.Instance.RemainingTime)
                            {
                                tempRemainingChestTime = zero;
                                checkWithRemainingTime = TimeService.Instance.RemainingTime - savedData.RemainingTime;
                                if (savedData.QueueIndex == one)
                                {
                                    SlotService.Instance.SetFirstChestUnlocked();
                                }
                            }
                            else
                            {
                                tempRemainingChestTime = MathF.Abs(savedData.RemainingTime - TimeService.Instance.RemainingTime);
                                checkWithRemainingTime = TimeService.Instance.RemainingTime - tempRemainingChestTime;
                            }
                        }

                        chestController.SetRemainingTime(tempRemainingChestTime);

                        if (checkWithRemainingTime < TimeService.Instance.RemainingTime)
                        {
                            float timeServiceRemainingTime = TimeService.Instance.RemainingTime - checkWithRemainingTime;
                            TimeService.Instance.SetRemainingTime(timeServiceRemainingTime);
                        }
                        else
                        {
                            TimeService.Instance.SetRemainingTime(TimeService.Instance.RemainingTime);
                        }
                    }
                    else
                    {
                        chestController.SetRemainingTime(savedData.RemainingTime);
                    }

                    SlotSaveData slotData = new SlotSaveData();

                    slotData.IsSaved = false;

                    string _json = JsonUtility.ToJson(slotData, true);
                    File.WriteAllText(Application.persistentDataPath + saveLocationString, _json);

                    SlotService.Instance.SetSlotRemaining();
                    SlotIsTaken();
                }
            }

            SlotService.Instance.SlotLoaded = true;
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
                slotData.TotalUnlockDuration = chestController.GetTotalUnlockDuration();

                string json = JsonUtility.ToJson(slotData, true);
                File.WriteAllText(Application.persistentDataPath + saveLocationString, json);
            }
            else
            {
                SlotSaveData slotData = new SlotSaveData();

                slotData.IsSaved = false;

                string json = JsonUtility.ToJson(slotData, true);
                File.WriteAllText(Application.persistentDataPath + saveLocationString, json);
            }
            SlotService.Instance.SetSlotsSavedCount();
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
            slotData.TotalUnlockDuration = zero;

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