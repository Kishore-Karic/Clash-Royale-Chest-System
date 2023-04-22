using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem.Chest
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI chestStatusText;
        [SerializeField] private TextMeshProUGUI chestTypeText;
        [SerializeField] private TextMeshProUGUI chestTimeText;

        private ChestController chestController;

        public void SetChestController(ChestController _chestController, string _text)
        {
            chestController = _chestController;
            chestTypeText.text = _text;
        }

        public void UpdateImage(Sprite sprite, string _statusText, string _timeText)
        {
            image.sprite = sprite;
            chestStatusText.text = _statusText;
            chestTimeText.text = _timeText;
        }
    }
}