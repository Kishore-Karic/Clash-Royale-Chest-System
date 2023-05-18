using ChestSystem.GenericSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private int averageDateInYear;
        [SerializeField] private int secondsInDay;
        [SerializeField] private List<int> datesFromMonths;
        [SerializeField] private int four;
        [SerializeField] private int febraury;

        public float CurrentTime { get; private set; }
        public float RemainingTime { get; private set; }
        public int TotalDate { get; private set; }
        public int CurrentYear { get; private set; }
        public int LeapYear { get; private set; }

        private Coroutine coroutine;
        private bool firstTime;
        private GameSaveData savedData;
        private int currentMonth;

        protected override void Awake()
        {
            base.Awake();
            firstTime = true;
            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                string json = File.ReadAllText(Application.persistentDataPath + saveLocationString);
                savedData = JsonUtility.FromJson<GameSaveData>(json);
            }
            
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
            
            currentMonth = monthValue;
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
                if(File.Exists(Application.persistentDataPath + saveLocationString))
                {
                    LeapYear = savedData.LastLeapYear;
                }
                else
                {
                    LeapYear = FindLeapYear();
                }
                CalculateTotalDate(dateValue, monthValue);
                FindRemainingTime();
            }
        }

        private int FindLeapYear()
        {
            bool foundLeapYear = false;
            int year = CurrentYear;

            while (foundLeapYear == false)
            {
                float floatValue = (float)year / four;
                int intValue = (int)floatValue;
                
                if (intValue == floatValue)
                {
                    foundLeapYear = true;
                }
                else
                {
                    year -= one;
                }
            }

            return year;
        }

        private int FindLeftedLeapDates()
        {
            int leftedLeapDate = zero;
            int tempLeapYear = FindLeapYear();
            LeapYear = tempLeapYear;

            if(tempLeapYear > savedData.LastSavedYear)
            {
                if (tempLeapYear == CurrentYear)
                {
                    tempLeapYear -= four;
                }
                while (tempLeapYear > savedData.LastSavedYear)
                {
                    leftedLeapDate += one;
                    tempLeapYear -= four;
                }
            }
            
            return leftedLeapDate;
        }

        private void CalculateTotalDate(int todayDate, int thisMonth)
        {
            TotalDate = zero;

            for (int i = 0; i < thisMonth; i++)
            {
                TotalDate += datesFromMonths[i];
            }
            TotalDate += todayDate;

            if (CurrentYear == LeapYear)
            {
                if(currentMonth > febraury)
                {
                    TotalDate += one;
                }
            }
            else if ((LeapYear + four) == CurrentYear)
            {
                LeapYear += four;
                if (currentMonth > febraury)
                {
                    TotalDate += one;
                }
            }
        }

        private void FindRemainingTime()
        {
            RemainingTime = zero;

            if (File.Exists(Application.persistentDataPath + saveLocationString))
            {
                float totalTime = zero;
                if (savedData.LastSavedTotalDate < TotalDate || savedData.LastSavedTotalDate > TotalDate || CurrentYear != savedData.LastSavedYear)
                {
                    int leftedDates = zero;

                    if (CurrentYear != savedData.LastSavedYear)
                    {
                        if ((CurrentYear - savedData.LastSavedYear) > one)
                        {
                            leftedDates += ((CurrentYear - one) - savedData.LastSavedYear) * averageDateInYear;
                        }
                        leftedDates += FindLeftedLeapDates();
                        leftedDates += (averageDateInYear - savedData.LastSavedTotalDate) + TotalDate;
                    }
                    else if((TotalDate - savedData.LastSavedTotalDate) > one)
                    {
                        leftedDates += (TotalDate - one) - savedData.LastSavedTotalDate;
                    }

                    totalTime += (secondsInDay - savedData.LastSavedTimeInSeconds) + CurrentTime;
                    totalTime += leftedDates * secondsInDay;
                }
                else
                {
                    totalTime = CurrentTime - savedData.LastSavedTimeInSeconds;
                }

                RemainingTime = totalTime;
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