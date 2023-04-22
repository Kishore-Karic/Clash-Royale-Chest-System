using ChestSystem.Enum;
using ChestSystem.ScriptableObjects;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestModel
    {
        public ChestType ChestType { get; private set; }
        public string Name { get; private set; }
        public float UnlockDuration { get; private set; }
        public int MinCoin { get; private set; }
        public int MaxCoin { get; private set; }
        public int MinGem { get; private set; }
        public int MaxGem { get; private set; }
        public Sprite LockedImage { get; private set; }
        public Sprite UnlockedImage { get; private set; }
        public Sprite OpenedImage { get; private set; }
        public string UnlockAmount { get; private set; }

        public ChestModel(ChestScriptableObject chestScriptableObject)
        {
            ChestType = chestScriptableObject.ChestType;
            Name = chestScriptableObject.Name;
            UnlockDuration = chestScriptableObject.UnlockDuration;
            MinCoin = chestScriptableObject.MinCoin;
            MaxCoin = chestScriptableObject.MaxCoin;
            MinGem = chestScriptableObject.MinGem;
            MaxGem = chestScriptableObject.MaxGem;
            LockedImage = chestScriptableObject.LockedImage;
            UnlockedImage = chestScriptableObject.UnlockedImage;
            OpenedImage = chestScriptableObject.OpenedImage;
            UnlockAmount = chestScriptableObject.UnlockAmount;
        }
    }
}