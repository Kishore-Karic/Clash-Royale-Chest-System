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
        [SerializeField] private TextMeshProUGUI instantOpenGemText;
        [SerializeField] private GameObject instantOpenLayer;

        private ChestController chestController;

        public void SetChestController(ChestController _chestController, string _text)
        {
            chestController = _chestController;
            chestTypeText.text = _text;
            chestController.SetTimerText(chestTimeText);
        }

        public void UpdateImageAndText(Sprite sprite, string _statusText, string _timeText)
        {
            image.sprite = sprite;
            chestStatusText.text = _statusText;
            chestTimeText.text = _timeText;
        }

        public void EnteredUnlockingStage(float _value)
        {
            Color color = image.color;
            color.a = _value;
            image.color = color;
        }

        public TextMeshProUGUI GetInstantOpenText()
        {
            return instantOpenGemText;
        }

        public void SetInstantActiveLayer(bool _value)
        {
            instantOpenLayer.SetActive(_value);
        }
    }
}