using ChestSystem.Service;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        private ChestView chestView;
        private ChestModel chestModel;

        public ChestController(ChestModel _chestModel, ChestView _chestView, Transform slotTransform)
        {
            chestModel = _chestModel;
            chestView = Object.Instantiate(_chestView, slotTransform);

            chestView.SetChestController(this, chestModel.Name);
            chestView.UpdateImage(chestModel.LockedImage, ChestService.Instance.GetChestStatusText(0), chestModel.UnlockAmount);
        }
    }
}