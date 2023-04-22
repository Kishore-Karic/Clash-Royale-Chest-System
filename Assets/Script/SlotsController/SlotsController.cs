using ChestSystem.Chest;
using ChestSystem.Service;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Slot
{
    public class SlotsController : MonoBehaviour
    {
        public List<SlotType> slotsList;
        private int slotsAvailable;

        public void CreateChestRequest()
        {
            if(slotsAvailable == slotsList.Count)
            {
                UIService.Instance.ShowErrorMessage(Enum.ErrorType.SlotFull);
            }
            else
            {
                for(int i = 0; i < slotsList.Count; i++)
                {
                    if (slotsList[i].isEmpty == true)
                    {
                        CreateChest(i);
                        break;
                    }
                }
            }
        }

        private void CreateChest(int i)
        {
            slotsList[i].isEmpty = false;
            ChestService.Instance.CreateChest(i, slotsList[i].slotObject, this);
            slotsList[i].slotObject.SetActive(true);
            slotsAvailable++;
        }

        public void SetChestController(ChestController _chestController, int i)
        {
            slotsList[i].ChestController = _chestController;
        }

        public void OnButtonClick(int i)
        {
            i--;
            slotsList[i].ChestController.ButtonClick();
        }

        public void EmptySlot(int i)
        {
            slotsList[i].isEmpty = true;
            slotsList[i].slotObject.SetActive(false);
            slotsAvailable--;
        }
    }
}