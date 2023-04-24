using ChestSystem.Enum;
using ChestSystem.GenericSingleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Service
{
    public class UIService : GenericSingleton<UIService>
    {
        private Coroutine coroutine;
        private Queue<string> errorTextQueue;

        [SerializeField] private Button generateChestButton;
        [SerializeField] private float coroutineDuration;
        [SerializeField] private float coroutineDelay;
        [SerializeField] private TextMeshProUGUI errorMessageText;
        [SerializeField] private List<string> errorTextList;
        [SerializeField] private GameObject instantOpenConfirmationLayer;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private GameObject chestSlotsUI;
        
        protected override void Awake()
        {
            base.Awake();
            errorTextQueue = new Queue<string>();

            generateChestButton.onClick.AddListener(CreateChestRequest);
            yesButton.onClick.AddListener(BuyWithGems);
            noButton.onClick.AddListener(CloseConfirmationLayer);

            errorMessageText.text = null;
        }

        private void CreateChestRequest()
        {
            SlotService.Instance.CreateChestRequest();
        }

        public void InstantlyOpenChest()
        {
            instantOpenConfirmationLayer.SetActive(true);
            chestSlotsUI.SetActive(false);
        }

        private void BuyWithGems()
        {
            ChestService.Instance.RequestAccepted();
            CloseConfirmationLayer();
        }

        private void CloseConfirmationLayer()
        {
            chestSlotsUI.SetActive(true);
            instantOpenConfirmationLayer.SetActive(false);
        }

        public void ShowErrorMessage(ErrorType errorType)
        {
            if (errorType == ErrorType.SlotFull)
            {
                errorTextQueue.Enqueue(errorTextList[0]);
            }
            if(errorType == ErrorType.NotEnoughGem)
            {
                errorTextQueue.Enqueue(errorTextList[1]);
            }

            if (coroutine == null)
            {
                StartCoroutine(CommonCoroutine());
            }
        }
        
        IEnumerator CommonCoroutine()
        {
            while (errorTextQueue.Count > 0)
            {
                errorMessageText.text = errorTextQueue.Dequeue();
                yield return new WaitForSeconds(coroutineDuration);

                errorMessageText.text = null;
                yield return new WaitForSeconds(coroutineDelay);
            }
            coroutine = null;
        }
    }
}