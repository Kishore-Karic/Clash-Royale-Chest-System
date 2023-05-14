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
        [SerializeField] private int startingCount_1;
        [SerializeField] private int startingCount_2;
        [SerializeField] private int sixtySeconds;
        [SerializeField] private int zero;
        [SerializeField] private string saveLocationString;

        public float CurrentTime { get; private set; }
        public float RemainingTime { get; private set; }
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

            string second = time.Substring(secondSubStringValue);
            time = time.Remove(startingCount_1, removeStringCount);

            string minute = time.Substring(minutSubStringValue);
            time = time.Remove(startingCount_2, removeStringCount);

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

                RemainingTime = CurrentTime - savedData.LastSavedTimeInSeconds;
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