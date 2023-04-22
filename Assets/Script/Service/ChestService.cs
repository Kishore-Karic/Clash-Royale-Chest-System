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
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private List<ChestScriptableObject> chestScriptableObjectList;

        [SerializeField] private SlotsController slotController;
        [SerializeField] private List<string> statusText;

        public void CreateChest(int i, GameObject chestSlot)
        {
            chestController = new ChestController(new ChestModel(GetRandomChest()), chestPrefab, chestSlot.transform);
            slotController.SetChestController(chestController, i);
        }

        private ChestScriptableObject GetRandomChest()
        {
            int index = Random.Range(0, chestScriptableObjectList.Count);

            return chestScriptableObjectList[index];
        }

        public string GetChestStatusText(int i)
        {
            return statusText[i];
        }
    }
}