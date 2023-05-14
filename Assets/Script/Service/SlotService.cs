using ChestSystem.GenericSingleton;
using ChestSystem.Slot;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.Service
{
    public class SlotService : GenericSingleton<SlotService>
    {
        [SerializeField] private List<SlotsController> slotsControllersList;
        [SerializeField] private int queueIndex;
        [SerializeField] private int zero;

        public bool FirstChestUnlocked { get; private set; }
        public bool SlotLoaded;

        private int slotsRemaining;
        private event Action OnReset;
        private Coroutine coroutine;

        protected override void Awake()
        {
            base.Awake();
            SlotLoaded = false;
            FirstChestUnlocked = false;
        }
        private void Start()
        {
            for(int i = 0; i < slotsControllersList.Count; i++)
            {
                OnReset += slotsControllersList[i].ResetChestDetails;
            }
        }

        public void LoadSlots()
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(ExecuteSlotController());
        }

        private IEnumerator ExecuteSlotController()
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

                    yield return new WaitUntil(() => SlotLoaded == true);
                    SlotLoaded = false;
                }
            }
        }

        public void SetFirstChestUnlocked() => FirstChestUnlocked = true;

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