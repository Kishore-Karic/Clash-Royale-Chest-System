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
        private Queue<ChestController> chestUnlockQueue;
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

        public void CreateChest(int i, ChestView _chestView, SlotsController slotsController)
        {
            chestController = new ChestController(new ChestModel(GetRandomChest(), chestSOList), _chestView, i);
            slotsController.SetChestController(chestController);
        }

        public void AddChestToQueue(ChestController _chestController)
        {
            chestUnlockQueue.Enqueue(_chestController);
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