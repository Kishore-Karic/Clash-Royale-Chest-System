using ChestSystem.Enum;
using ChestSystem.ScriptableObjects;
using UnityEngine;

namespace ChestSystem.Slot
{
    [CreateAssetMenu(fileName = "SlotSave", menuName = "ScriptableObject/NewSlotSave")]
    public class SlotSaveScriptableObject : ScriptableObject
    {
        public bool IsSaved;
        public ChestScriptableObject ChestScriptableObject;
        public ChestStatus ChestStatus;
        public float RemainingTime;
        public int SlotIndex;
        public bool IsAddedToQueue;
        public bool IsUnlocking;
        public int QueueIndex;
    }
}