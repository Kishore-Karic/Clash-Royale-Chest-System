using ChestSystem.Enum;
using ChestSystem.ScriptableObjects;

public class SlotSaveData
{
    public bool IsSaved;
    public ChestScriptableObject ChestScriptableObject;
    public ChestStatus ChestStatus;
    public float RemainingTime;
    public int SlotIndex;
    public bool IsAddedToQueue;
    public bool IsUnlocking;
    public int QueueIndex;
    public float TotalUnlockDuration;
}
