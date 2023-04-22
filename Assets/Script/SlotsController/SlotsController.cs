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
                UIService.Instance.ShowSlotsFullMessage();
            }
            else
            {
                for(int i = 0; i < slotsList.Count; i++)
                {
                    if (slotsList[i].isEmpty == false)
                    {
                        slotsList[i].isEmpty = true;
                        ChestService.Instance.CreateChest(i, slotsList[i].slotObject);
                        slotsList[i].slotObject.SetActive(true);
                        slotsAvailable++;
                        break;
                    }
                }
            }
        }

        public void SetChestController(ChestController _chestController, int i)
        {
            slotsList[i].ChestController = _chestController;
        }

        public void OnButtonClick(int i)
        {
            i--;
            //slotsList[i].ChestController;
        }
    }
}