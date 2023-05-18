using ChestSystem.Enum;
using UnityEngine;

namespace ChestSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChestType", menuName = "ScriptableObject/NewChest")]
    public class ChestScriptableObject : ScriptableObject
    {
        public string Name;
        public float UnlockDuration;
        public int MinCoin;
        public int MaxCoin;
        public int MinGem;
        public int MaxGem;
        public Sprite LockedImage;
        public Sprite UnlockedImage;
        public Sprite OpenedImage;
        public string UnlockAmount;
    }
}