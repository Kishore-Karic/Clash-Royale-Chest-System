using ChestSystem.Enum;
using ChestSystem.GenericSingleton;
using ChestSystem.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Service
{
    public class UIService : GenericSingleton<UIService>
    {
        private Queue<string> displayTextQueue;
        private RequestType requestType;
        private Coroutine endCoroutine;
        private bool slotsSave;
        private bool gameSave;
        private bool saveCompleted;

        [SerializeField] private Button generateChestButton;
        [SerializeField] private float coroutineDuration;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private GameObject chestSlotsUI;
        [SerializeField] private TextMeshProUGUI displayMessageText;
        [SerializeField] private List<string> errorTextList;
        [SerializeField] private List<string> confirmationTextList;
        [SerializeField] private GameObject requestConfirmationLayer;
        [SerializeField] private TextMeshProUGUI requestConfirmationText;
        [SerializeField] private List<string> requestTextList;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private InGameResource inGameResource;
        [SerializeField] private string saveInfoText;
        
        protected override void Awake()
        {
            base.Awake();
            displayTextQueue = new Queue<string>();

            generateChestButton.onClick.AddListener(CreateChestRequest);
            yesButton.onClick.AddListener(RequestPositive);
            noButton.onClick.AddListener(RequestNegetive);
            exitButton.onClick.AddListener(ExitGame);
            resetButton.onClick.AddListener(ResetGame);

            displayMessageText.text = null;
            requestConfirmationText.text = null;
            slotsSave = false;
            gameSave = false;
            saveCompleted = false;

            displayTextQueue.Enqueue(saveInfoText);
            StartCoroutine(CommonCoroutine(Color.yellow));
        }
        
        private void CreateChestRequest()
        {
            SlotService.Instance.CreateChestRequest();
        }

        public void DisplayConfirmationLayer(RequestType _requestType)
        {
            requestType = _requestType;
         
            if (requestType == RequestType.OpenWithGems)
            {
                requestConfirmationText.text = requestTextList[0];
            }
            else if(requestType == RequestType.AddToQueueStartUnlock)
            {
                requestConfirmationText.text = requestTextList[1];
            }
            else if(requestType == RequestType.AddToUnlockQueue)
            {
                requestConfirmationText.text = requestTextList[2];
            }

            requestConfirmationLayer.SetActive(true);
            chestSlotsUI.SetActive(false);
            exitButton.gameObject.SetActive(false);
            resetButton.gameObject.SetActive(false);
        }

        private void RequestPositive()
        {
            ChestService.Instance.RequestAccepted(requestType);
            RequestNegetive();
        }

        private void RequestNegetive()
        {
            chestSlotsUI.SetActive(true);
            exitButton.gameObject.SetActive(true);
            resetButton.gameObject.SetActive(true);
            requestConfirmationLayer.SetActive(false);
        }

        public void ShowErrorMessage(ErrorType errorType)
        {
            if (errorType == ErrorType.SlotFull)
            {
                displayTextQueue.Enqueue(errorTextList[0]);
            }
            else if(errorType == ErrorType.NotEnoughGem)
            {
                displayTextQueue.Enqueue(errorTextList[1]);
            }
            else if(errorType == ErrorType.QueueIsFull)
            {
                displayTextQueue.Enqueue(errorTextList[2]);
            }
            else if(errorType == ErrorType.AddedToQueue)
            {
                displayTextQueue.Enqueue(errorTextList[3]);
            }

            StartCoroutine(CommonCoroutine(Color.red));
        }
        
        public void ShowConfirmationMessage(ConfirmationType confirmationType)
        {
            if(confirmationType == ConfirmationType.PurchaseDone)
            {
                displayTextQueue.Enqueue(confirmationTextList[0]);
            }
            else if(confirmationType == ConfirmationType.RewardsCollected)
            {
                displayTextQueue.Enqueue(confirmationTextList[1]);
            }
            else if(confirmationType == ConfirmationType.AddedToQueue)
            {
                displayTextQueue.Enqueue(confirmationTextList[2]);
            }

            StartCoroutine(CommonCoroutine(Color.green));
        }

        IEnumerator CommonCoroutine(Color color)
        {
            displayMessageText.color = color;
            displayMessageText.text = displayTextQueue.Dequeue();
            yield return new WaitForSeconds(coroutineDuration);

            displayMessageText.text = null;
        }

        public void GameSaved(SaveType saveType)
        {
            if(saveType == SaveType.SlotsSave)
            {
                slotsSave = true;
            }
            if(saveType == SaveType.GameSave)
            {
                gameSave = true;
            }
            if(slotsSave && gameSave)
            {
                saveCompleted = true;
            }
        }

        private void SaveGame()
        {
            TimeService.Instance.GetTime();
            SlotService.Instance.SaveSlots();
            inGameResource.SaveGame();
        }

        private void ResetGame()
        {
            SlotService.Instance.ResetSlots();
            inGameResource.ResetGame();
        }

        private void ExitGame()
        {
            SaveGame();
            if (endCoroutine != null)
            {
                StopCoroutine(endCoroutine);
            }
            endCoroutine = StartCoroutine(QuitApplication());
        }

        private IEnumerator QuitApplication()
        {
            yield return new WaitUntil(() => saveCompleted == true);
            Application.Quit();
        }
    }
}