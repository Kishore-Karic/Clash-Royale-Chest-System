using ChestSystem.GenericSingleton;
using ChestSystem.Slot;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Service
{
    public class SlotService : GenericSingleton<SlotService>
    {
        [SerializeField] private List<SlotsController> slotsControllersList;
        private int slotsRemaining;

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
            ChestService.Instance.CreateChest(i, slotsControllersList[i].ChestView, slotsControllersList[i]);
            slotsControllersList[i].SlotIsTaken();
            slotsRemaining++;
        }

        public void EmptySlot(int i)
        {
            slotsControllersList[i].EmptySlot();
            slotsRemaining--;
        }
    }
}