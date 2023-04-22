using ChestSystem.Chest;
using UnityEngine;

namespace ChestSystem.Slot
{
    [System.Serializable]
    public class SlotType
    {
        public GameObject slotObject;
        public bool isEmpty;
        public ChestController ChestController;
    }
}