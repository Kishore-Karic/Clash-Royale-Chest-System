using ChestSystem.Chest;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Slot
{
    public class SlotsController : MonoBehaviour
    {
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private bool isEmpty;
        [SerializeField] private ChestController chestController;
        [SerializeField] private Button button;
        public ChestView ChestView { get; private set; }

        private void Awake()
        {
            if (button.IsInteractable())
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }

        private void Start()
        {
            button.interactable = false;

            ChestView = Instantiate(chestPrefab, transform);
            ChestView.gameObject.SetActive(false);
        }
        
        public bool GetIsEmpty() => isEmpty;

        public void SlotIsTaken()
        {
            isEmpty = false;
            ChestView.gameObject.SetActive(true);
            button.interactable = true;
        }

        public void SetChestController(ChestController _chestController)
        {
            chestController = _chestController;
        }

        public void OnButtonClick()
        {
            chestController.ButtonClick();
        }

        public void EmptySlot()
        {
            isEmpty = true;
            chestController = null;
            ChestView.gameObject.SetActive(false);
            button.interactable = false;
        }
    }
}