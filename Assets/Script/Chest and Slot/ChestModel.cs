using ChestSystem.Enum;
using ChestSystem.ScriptableObjects;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestModel
    {
        public ChestType ChestType { get; private set; }
        public ChestScriptableObject ChestScriptableObject { get; private set; }
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

        public float TransparentValue { get; private set; }
        public int SixtySeconds { get; private set; }
        public int TenMinute { get; private set; }
        public int OneSecond { get; private set; }
        public int DelayTimeInMilliSeconds { get; private set; }
        public int SecondsForHour { get; private set; }
        public string HourString { get; private set; }
        public string MinuteString { get; private set; }
        public string SecondString { get; private set; }
        public int One { get; private set; }

        public ChestModel(ChestScriptableObject chestSO, ChestScriptableObjectList chestSOList)
        {
            ChestType = chestSO.ChestType;
            ChestScriptableObject = chestSO;
            Name = chestSO.Name;
            UnlockDuration = chestSO.UnlockDuration;
            MinCoin = chestSO.MinCoin;
            MaxCoin = chestSO.MaxCoin;
            MinGem = chestSO.MinGem;
            MaxGem = chestSO.MaxGem;
            LockedImage = chestSO.LockedImage;
            UnlockedImage = chestSO.UnlockedImage;
            OpenedImage = chestSO.OpenedImage;
            UnlockAmount = chestSO.UnlockAmount;

            TransparentValue = chestSOList.TransparentValue;
            SixtySeconds = chestSOList.SixtySeconds;
            TenMinute = chestSOList.TenMinute;
            OneSecond = chestSOList.OneSecond;
            DelayTimeInMilliSeconds = chestSOList.DelayTimeInMilliSeconds;
            SecondsForHour = chestSOList.SecondsForHour;
            HourString = chestSOList.HourString;
            MinuteString = chestSOList.MinuteString;
            SecondString = chestSOList.SecondString;
            One = chestSOList.One;
        }
    }
}