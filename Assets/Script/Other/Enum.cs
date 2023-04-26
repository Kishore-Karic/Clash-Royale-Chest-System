namespace ChestSystem.Enum
{
    public enum ChestType
    {
        None,
        Common,
        Rare,
        Epic,
        Legendary
    }

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
        AddToUnlockQueue
    }
}