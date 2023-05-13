using ChestSystem.GenericSingleton;
using ChestSystem.Slot;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Service
{
    public class SlotService : GenericSingleton<SlotService>
    {
        [SerializeField] private List<SlotsController> slotsControllersList;
        [SerializeField] private int queueIndex;
        [SerializeField] private int zero;

        private int slotsRemaining;
        private event Action OnSave;
        private event Action OnReset;

        private void Start()
        {
            for(int i = 0; i < slotsControllersList.Count; i++)
            {
                OnSave += slotsControllersList[i].StoreChestDetails;
                OnReset += slotsControllersList[i].ResetChestDetails;
            }

            ExecuteSlotController();
        }

        private void ExecuteSlotController()
        {
            int count = zero;

            while (count != slotsControllersList.Count)
            {
                for (int i = 0; i < slotsControllersList.Count; i++)
                {
                    if (slotsControllersList[i].QueueIndex == queueIndex)
                    {
                        slotsControllersList[i].LoadSlotController();
                        count++;
                        queueIndex++;
                    }
                    else if (slotsControllersList[i].QueueIndex == zero)
                    {
                        slotsControllersList[i].LoadSlotController();
                        count++;
                    }
                }
            }
        }

        public void CreateChestRequest()
        {
            if (slotsRemaining == slotsControllersList.Count)
            {
                UIService.Instance.ShowErrorMessage(Enum.ErrorType.SlotFull);
            }
            else
            {
                for (int i = 0; i < slotsControllersList.Count; i++)
                {
                    if (slotsControllersList[i].GetIsEmpty() == true)
                    {
                        CreateChest(i);
                        break;
                    }
                }
            }
        }

        private void CreateChest(int i)
        {
            ChestService.Instance.CreateChest(true, null, slotsControllersList[i].ChestView, i, slotsControllersList[i], Enum.ChestStatus.Locked, false, false, zero);
            slotsControllersList[i].SlotIsTaken();
            slotsRemaining++;
        }

        public void EmptySlot(int i)
        {
            slotsControllersList[i].EmptySlot();
            slotsRemaining--;
        }

        public void SaveGame()
        {
            OnSave?.Invoke();
        }

        public void ResetGame()
        {
            OnReset?.Invoke();
            slotsRemaining = zero;
        }

        public void SetSlotRemaining()
        {
            slotsRemaining++;
        }
    }
}