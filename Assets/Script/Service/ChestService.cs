using ChestSystem.Chest;
using ChestSystem.GenericSingleton;
using ChestSystem.ScriptableObjects;
using ChestSystem.Slot;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Service
{
    public class ChestService : GenericSingleton<ChestService>
    {
        private ChestController chestController;
        private ChestController requestedChestController;

        [SerializeField] private ChestScriptableObjectList chestSOList;
        
        [SerializeField] private List<string> statusText;

        public void CreateChest(int i, ChestView _chestView, SlotsController slotsController)
        {
            chestController = new ChestController(new ChestModel(GetRandomChest(), chestSOList), _chestView, i);
            slotsController.SetChestController(chestController);
        }

        public void ReturnSlot(int i)
        {
            SlotService.Instance.EmptySlot(i);
        }

        public void RequestForBuyWithGems(ChestController _chestController)
        {
            requestedChestController = _chestController;
            UIService.Instance.InstantlyOpenChest();
        }

        public void RequestAccepted()
        {
            requestedChestController.RequestAccepted();
        }

        private ChestScriptableObject GetRandomChest()
        {
            int index = Random.Range(0, chestSOList.ChestSO.Count);

            return chestSOList.ChestSO[index];
        }

        public string GetChestStatusText(int i)
        {
            return statusText[i];
        }
    }
}