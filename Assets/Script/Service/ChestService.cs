using ChestSystem.Chest;
using ChestSystem.Enum;
using ChestSystem.GenericSingleton;
using ChestSystem.Resource;
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
        public Queue<ChestController> chestUnlockQueue { get; private set; }
        private bool isUnlocking;

        [SerializeField] private int chestUnlockLimit;
        [SerializeField] private ChestScriptableObjectList chestSOList;
        [SerializeField] private List<string> statusText;
        [field: SerializeField] public InGameResource Resource { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            chestUnlockQueue = new Queue<ChestController>();
        }

        public void CreateChest(bool newChest, ChestScriptableObject chestScriptableObject, ChestView chestView, int slotIndex, SlotsController slotsController, ChestStatus chestStatus, bool isAddedToQueue, bool isUnlocking, int queueIndex)
        {
            if (newChest)
            {
                chestController = new ChestController(new ChestModel(GetRandomChest(), chestSOList), chestView, slotIndex, newChest, chestStatus, isAddedToQueue, isUnlocking, queueIndex);
            }
            else
            {
                chestController = new ChestController(new ChestModel(chestScriptableObject, chestSOList), chestView, slotIndex, newChest, chestStatus, isAddedToQueue, isUnlocking, queueIndex);
            }
            slotsController.SetChestController(chestController);
        }

        public int AddChestToQueue(ChestController _chestController)
        {
            chestUnlockQueue.Enqueue(_chestController);
            return chestUnlockQueue.Count;
        }

        public void FinishedUnlockingChest()
        {
            chestUnlockQueue.Dequeue();
            UnlockNextChestInQueue();
        }

        public bool IsAnyChestUnlocking() => isUnlocking;

        public void SetIsChestUnlocking(bool _value)
        {
            isUnlocking = _value;
        }

        private void UnlockNextChestInQueue()
        {
            if (chestUnlockQueue.Count != 0)
            {
                chestUnlockQueue.Peek().StartUnlocking();
                SetIsChestUnlocking(true);
            }
        }

        public bool CanEnqueueChest() => chestUnlockQueue.Count < chestUnlockLimit;

        public void ResetChestQueue()
        {
            chestUnlockQueue.Clear();
            isUnlocking = false;
        }

        public void ReturnSlot(int i)
        {
            SlotService.Instance.EmptySlot(i);
        }

        public void RequestForConfirmation(ChestController _chestController, RequestType requestType)
        {
            requestedChestController = _chestController;
            UIService.Instance.DisplayConfirmationLayer(requestType);
        }

        public void RequestAccepted(RequestType requestType)
        {
            requestedChestController.RequestAccepted(requestType);
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