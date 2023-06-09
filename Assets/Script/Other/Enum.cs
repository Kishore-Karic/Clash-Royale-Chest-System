namespace ChestSystem.Enum
{
    public enum ChestStatus
    {
        Locked,
        Unlocking,
        Unlocked
    }

    public enum ErrorType
    {
        SlotFull,
        NotEnoughGem,
        QueueIsFull,
        AddedToQueue
    }

    public enum ConfirmationType
    {
        PurchaseDone,
        RewardsCollected,
        AddedToQueue
    }

    public enum RequestType
    {
        OpenWithGems,
        AddToQueueStartUnlock,
        AddToUnlockQueue
    }

    public enum SaveType
    {
        SlotsSave,
        GameSave
    }
}