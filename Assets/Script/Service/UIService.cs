using ChestSystem.GenericSingleton;
using ChestSystem.Slot;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Service
{
    public class UIService : GenericSingleton<UIService>
    {
        public Button generateChestButton;

        [SerializeField] private SlotsController slotsController;
        [SerializeField] private GameObject sloteFullText;

        protected override void Awake()
        {
            base.Awake();
            generateChestButton.onClick.AddListener(CreateChestRequest);
        }

        private void CreateChestRequest()
        {
            slotsController.CreateChestRequest();
        }

        public void ShowSlotsFullMessage()
        {
            sloteFullText.SetActive(true);
            StartCoroutine(CommonCoroutine(sloteFullText, 1f));
        }

        IEnumerator CommonCoroutine(GameObject _gameObject, float time)
        {
            yield return new WaitForSeconds(time);
            _gameObject.SetActive(false);
        }
    }
}