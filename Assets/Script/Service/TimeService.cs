using ChestSystem.GenericSingleton;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace ChestSystem.Service
{
    public class TimeService : GenericSingleton<TimeService>
    {
        [SerializeField] private string URL;
        [SerializeField] private int hourSubStringValue;
        [SerializeField] private int minutSubStringValue;
        [SerializeField] private int secondSubStringValue;
        [SerializeField] private int removeStringCount;
        [SerializeField] private int removeSeconds;
        [SerializeField] private int removeMinutes;
        [SerializeField] private int sixtySeconds;
        [SerializeField] private int zero;
        [SerializeField] private string saveLocationString;
        [SerializeField] private int dateSubStringValue;
        [SerializeField] private int monthSubStringValue;
        [SerializeField] private int yearSubStringValue;
        [SerializeField] private int removeDate;
        [SerializeField] private int removeMonth;
        [SerializeField] private int one;
        [SerializeField] private int averageDateInMonth;
        [SerializeField] private int secondsInDay;
        [SerializeField] private int passedUnlockTimeInSeconds;

        public float CurrentTime { get; private set; }
        public float RemainingTime { get; private set; }
        public int CurrentDate { get; private set; }
        public int CurrentMonth { get; private set; }
        public int CurrentYear { get; private set; }

        private Coroutine coroutine;
        private bool firstTime;

        protected override void Awake()
        {
            base.Awake();
            firstTime = true;
            GetTime();
        }

        struct TimeDate
        {
            public string datetime;
        }

        public void GetTime()
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(GetTimeFromServer());
        }

        private IEnumerator GetTimeFromServer()
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(URL);

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                GetTime();
            }
            else
            {
                TimeDate timeDate = JsonUtility.FromJson<TimeDate>(webRequest.downloadHandler.text);
                ConvertTime(timeDate.datetime);
            }
        }

        private void ConvertTime(string datetime)
        {
            string time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;
            string fullDate = Regex.Match(datetime, @"\d{4}-\d{2}-\d{2}").Value;
            
            string dateString = fullDate.Substring(dateSubStringValue);
            fullDate = fullDate.Remove(removeDate, removeStringCount);

            string monthString = fullDate.Substring(monthSubStringValue);
            fullDate = fullDate.Remove(removeMonth, removeStringCount);

            string yearString = fullDate.Substring(yearSubStringValue);

            int dateValue, monthValue, yearValue;
            Int32.TryParse(dateString, out dateValue);
            Int32.TryParse(monthString, out monthValue);
            Int32.TryParse(yearString, out yearValue);

            CurrentDate = dateValue;
            CurrentMonth = monthValue;
            CurrentYear = yearValue;

            string second = time.Substring(secondSubStringValue);
            time = time.Remove(removeSeconds, removeStringCount);

            string minute = time.Substring(minutSubStringValue);
            time = time.Remove(removeMinutes, removeStringCount);

            string hour = time.Substring(hourSubStringValue);

            int timeHoure, timeMinute, timeSecond;
            Int32.TryParse(hour, out timeHoure);
            Int32.TryParse(minute, out timeMinute);
            Int32.TryParse(second, out timeSecond);

            CurrentTime = timeHoure * (sixtySeconds * sixtySeconds) + timeMinute * sixtySeconds + timeSecond;
            
            if (firstTime)
            {
                firstTime = false;
                SetRemainingTime();
            }
        }

        private void SetRemainingTime()
        {
            if (RemainingTime < zero)
            {
                RemainingTime = zero;
            }

            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                GameSaveData savedData = JsonUtility.FromJson<GameSaveData>(json);

                if (savedData.LastDate < CurrentDate || savedData.LastDate > CurrentDate && (CurrentDate == one || CurrentDate > one))
                {
                    int leftedDates = zero;
                    if(CurrentDate == one || CurrentDate > one)
                    {
                        leftedDates = (averageDateInMonth - savedData.LastDate) + CurrentDate; 
                    }
                    float tempCurrentTime = (secondsInDay * leftedDates) - savedData.LastSavedTimeInSeconds;
                    RemainingTime = CurrentTime + tempCurrentTime;
                }
                else
                {
                    if (CurrentMonth != savedData.LastMonth || CurrentYear != savedData.LastYear)
                    {
                        RemainingTime = passedUnlockTimeInSeconds;
                    }
                    else
                    {
                        RemainingTime = CurrentTime - savedData.LastSavedTimeInSeconds;
                    }
                }
            }

            SlotService.Instance.LoadSlots();
        }

        public void SetRemainingTime(float time)
        {
            RemainingTime -= time;
            if(RemainingTime < zero)
            {
                RemainingTime = zero;
            }
        }
    }
}