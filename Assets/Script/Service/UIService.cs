using ChestSystem.Enum;
using ChestSystem.GenericSingleton;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Service
{
    public class UIService : GenericSingleton<UIService>
    {
        public Button generateChestButton;
        
        [SerializeField] private float coroutineDuration;
        [SerializeField] private GameObject sloteFullText;
        [SerializeField] private GameObject notEnoughGemText;
        [SerializeField] private GameObject instantOpenConfirmationLayer;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private GameObject chestSlotsUI;
        
        protected override void Awake()
        {
            base.Awake();
            generateChestButton.onClick.AddListener(CreateChestRequest);
            yesButton.onClick.AddListener(BuyWithGems);
            noButton.onClick.AddListener(CloseConfirmationLayer);
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
            GameObject gameObject = null;

            if (errorType == ErrorType.SlotFull)
            {
                gameObject = sloteFullText;
            }
            if(errorType == ErrorType.NotEnoughGem)
            {
                gameObject = notEnoughGemText;
            }

            gameObject.SetActive(true);
            StartCoroutine(CommonCoroutine(gameObject, coroutineDuration));
        }
        
        IEnumerator CommonCoroutine(GameObject _gameObject, float time)
        {
            yield return new WaitForSeconds(time);
            _gameObject.SetActive(false);
        }
    }
}