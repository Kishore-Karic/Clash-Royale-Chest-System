using System.Collections.Generic;
using UnityEngine;

namespace ChestSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChestList", menuName = "ScriptableObject/NewList")]
    public class ChestScriptableObjectList : ScriptableObject
    {
        public List<ChestScriptableObject> ChestSO;

        public float TransparentValue;
        public int SixtySeconds;
        public int TenMinute;
        public int OneSecond;
        public int DelayTimeInMilliSeconds;
        public int SecondsForHour;
        public string HourString;
        public string MinuteString;
        public string SecondString;
        public int One;
        public int SecondsInDay;
        public string DayString;
    }
}