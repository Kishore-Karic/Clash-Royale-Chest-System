using System;
using System.Threading.Tasks;
using ChestSystem.Enum;
using ChestSystem.Service;
using TMPro;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        private ChestView chestView;
        private ChestModel chestModel;
        private ChestStatus currentStatus;
        private TextMeshProUGUI timerText;
        private float unlockTimeInSeconds;
        private int slotsControllersListIndex;
        private TextMeshProUGUI instantOpenGemText;
        private float gemsForTime;
        private bool isUnlocking;
        private bool isAddedToQueue;

        public ChestController(ChestModel _chestModel, ChestView _chestView, int i)
        {
            chestModel = _chestModel;
            chestView = _chestView;

            chestView.SetChestController(this, chestModel.Name);
            SetState(ChestStatus.Locked);
            unlockTimeInSeconds = chestModel.UnlockDuration;
            slotsControllersListIndex = i;
            isAddedToQueue = false;
            isUnlocking = false;
        }

        public void ButtonClick()
        {
            if (isUnlocking)
            {
                if (currentStatus == ChestStatus.Unlocking)
                {
                    UnlockInstantly();
                }
                else if (currentStatus == ChestStatus.Unlocked)
                {
                    OpenChest();
                }
            }

            if (!isAddedToQueue && ChestService.Instance.CanEnqueueChest())
            {
                ChestService.Instance.RequestForConfirmation(this, RequestType.AddToUnlockQueue);
            }
            else if (!isUnlocking && isAddedToQueue)
            {
                UIService.Instance.ShowErrorMessage(ErrorType.AddedToQueue);
            }
        }

        private void AddControllerToQueue()
        {
            if (ChestService.Instance.CanEnqueueChest())
            {
                ChestService.Instance.EnqueueChest(this);
            }
            else
            {
                UIService.Instance.ShowErrorMessage(ErrorType.QueueIsFull);
            }
        }

        public void StartUnlocking()
        {
            instantOpenGemText = chestView.GetInstantOpenText();
            SetState(ChestStatus.Unlocking);
            ChestService.Instance.SetIsChestUnlocking(true);
            StartTimer();
            isUnlocking = true;
        }

        private void UnlockInstantly()
        {
            if (ChestService.Instance.Resource.IsRequireGemAvailable((int)MathF.Ceiling(gemsForTime)))
            {
                ChestService.Instance.RequestForConfirmation(this, RequestType.OpenWithGems);
            }
            else
            {
                UIService.Instance.ShowErrorMessage(ErrorType.NotEnoughGem);
            }
        }

        public void RequestAccepted(RequestType requestType)
        {
            if (requestType == RequestType.OpenWithGems)
            {
                ChestService.Instance.Resource.UpdatePurchaseCost((int)MathF.Ceiling(gemsForTime));
                UIService.Instance.ShowConfirmationMessage(ConfirmationType.PurchaseDone);
                SetState(ChestStatus.Unlocked);
            }
            else if(requestType == RequestType.AddToUnlockQueue)
            {
                if (ChestService.Instance.IsAnyChestUnlocking() && currentStatus == ChestStatus.Locked)
                {
                    AddControllerToQueue();
                }
                else
                {
                    StartUnlocking();
                }
                isAddedToQueue = true;
                UIService.Instance.ShowConfirmationMessage(ConfirmationType.AddedToQueue);
            }
        }

        private void OpenChest()
        {
            ChestService.Instance.Resource.AddRewards(GetRandomValues(chestModel.MinCoin, chestModel.MaxCoin), GetRandomValues(chestModel.MinGem, chestModel.MaxGem));
            UIService.Instance.ShowConfirmationMessage(ConfirmationType.RewardsCollected);
            ChestService.Instance.ReturnSlot(slotsControllersListIndex);
        }

        private int GetRandomValues(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        private async void StartTimer()
        {
            float hours, minutes, seconds;

            while(unlockTimeInSeconds > -chestModel.OneSecond && currentStatus == ChestStatus.Unlocking)
            {
                hours = Mathf.FloorToInt(unlockTimeInSeconds / chestModel.SecondsForHour);
                minutes = Mathf.FloorToInt((unlockTimeInSeconds / chestModel.SixtySeconds) % chestModel.SixtySeconds);
                seconds = Mathf.FloorToInt(unlockTimeInSeconds % chestModel.SixtySeconds);

                timerText.text = hours + chestModel.HourString + minutes + chestModel.MinuteString + seconds + chestModel.SecondString;

                gemsForTime = ((unlockTimeInSeconds / chestModel.SixtySeconds) / chestModel.TenMinute);
                instantOpenGemText.text = "" + MathF.Ceiling(gemsForTime);

                await Task.Delay(chestModel.DelayTimeInMilliSeconds);
                unlockTimeInSeconds -= chestModel.OneSecond;
            }
            SetState(ChestStatus.Unlocked);
        }

        private void SetState(ChestStatus chestStatus)
        {
            if(chestStatus == ChestStatus.Locked)
            {
                currentStatus = ChestStatus.Locked;
                chestView.UpdateImageAndText(chestModel.LockedImage, ChestService.Instance.GetChestStatusText((int)ChestStatus.Locked), chestModel.UnlockAmount);
            }

            if(chestStatus == ChestStatus.Unlocking)
            {
                currentStatus = ChestStatus.Unlocking;
                chestView.UpdateImageAndText(chestModel.LockedImage, ChestService.Instance.GetChestStatusText((int)ChestStatus.Unlocking), timerText.text);
                chestView.EnteredUnlockingStage(chestModel.TransparentValue);
                chestView.SetInstantActiveLayer(true);
            }

            if(chestStatus == ChestStatus.Unlocked)
            {
                currentStatus = ChestStatus.Unlocked;
                chestView.UpdateImageAndText(chestModel.UnlockedImage, ChestService.Instance.GetChestStatusText((int)ChestStatus.Unlocked), null);
                chestView.SetInstantActiveLayer(false);

                ChestService.Instance.SetIsChestUnlocking(false);
                ChestService.Instance.UnlockNextChestInQueue();
            }
        }

        public void SetTimerText(TextMeshProUGUI _timerText)
        {
            timerText = _timerText;
        }
    }
}