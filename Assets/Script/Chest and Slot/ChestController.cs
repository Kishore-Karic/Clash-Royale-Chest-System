using System;
using System.Threading;
using System.Threading.Tasks;
using ChestSystem.Enum;
using ChestSystem.ScriptableObjects;
using ChestSystem.Service;
using TMPro;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestController
    {
        public ChestStatus CurrentStatus { get; private set; }
        public float UnlockTimeInSeconds { get; private set; }
        public int SlotsControllersListIndex { get; private set; }
        public bool IsAddedToQueue { get; private set; }
        public bool IsUnlocking { get; private set; }
        public int QueueIndex { get; private set; }

        private ChestView chestView;
        private ChestModel chestModel;
        private TextMeshProUGUI timerText;
        private TextMeshProUGUI instantOpenGemText;
        private float gemsForTime;
        private CancellationTokenSource tokenSource;

        public ChestController(ChestModel _chestModel, ChestView _chestView, int _slotIndex, bool isNewChest, ChestStatus chestStatus, bool _isAddedToQueue, bool _isUnlocking, int _queueIndex)
        {
            chestModel = _chestModel;
            chestView = _chestView;

            tokenSource = new CancellationTokenSource();
            chestView.SetChestController(this, chestModel.Name);
            if (isNewChest)
            {
                SetState(ChestStatus.Locked);
                UnlockTimeInSeconds = chestModel.UnlockDuration;
            }
            else
            {
                SetState(chestStatus);
                QueueIndex = _queueIndex;
                if(chestStatus != ChestStatus.Unlocked)
                {
                    if (chestStatus == ChestStatus.Unlocking)
                    {
                        instantOpenGemText = chestView.GetInstantOpenText();
                        ChestService.Instance.SetIsChestUnlocking(true);
                        StartTimer(tokenSource.Token);
                    }

                    if (_isAddedToQueue == true)
                    {
                        ChestService.Instance.AddChestToQueue(this);
                    }
                }
            }

            SlotsControllersListIndex = _slotIndex;
            IsAddedToQueue = _isAddedToQueue;
            IsUnlocking = _isUnlocking;
        }

        public void ButtonClick()
        {
            if (IsUnlocking)
            {
                if (CurrentStatus == ChestStatus.Unlocking)
                {
                    UnlockInstantly();
                }
                else if (CurrentStatus == ChestStatus.Unlocked)
                {
                    OpenChest();
                }
            }

            if(!IsAddedToQueue && !ChestService.Instance.IsAnyChestUnlocking())
            {
                ChestService.Instance.RequestForConfirmation(this, RequestType.AddToQueueStartUnlock);
            }
            else if (!IsAddedToQueue && ChestService.Instance.CanEnqueueChest())
            {
                ChestService.Instance.RequestForConfirmation(this, RequestType.AddToUnlockQueue);
            }
            else if (!IsUnlocking && IsAddedToQueue)
            {
                UIService.Instance.ShowErrorMessage(ErrorType.AddedToQueue);
            }
            else if(!IsUnlocking && !IsAddedToQueue)
            {
                UIService.Instance.ShowErrorMessage(ErrorType.QueueIsFull);
            }
        }

        public void StartUnlocking()
        {
            instantOpenGemText = chestView.GetInstantOpenText();
            SetState(ChestStatus.Unlocking);
            ChestService.Instance.SetIsChestUnlocking(true);
            StartTimer(tokenSource.Token);
            IsUnlocking = true;
            if(QueueIndex != chestModel.One)
            {
                QueueIndex = chestModel.One;
            }
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
                ChestService.Instance.SetIsChestUnlocking(false);
                ChestService.Instance.FinishedUnlockingChest();
                QueueIndex--;
            }
            else if(requestType == RequestType.AddToQueueStartUnlock)
            {
                QueueIndex = ChestService.Instance.AddChestToQueue(this);
                UIService.Instance.ShowConfirmationMessage(ConfirmationType.AddedToQueue);
                IsAddedToQueue = true;
                StartUnlocking();
                ChestService.Instance.SetIsChestUnlocking(true);
            }
            else if(requestType == RequestType.AddToUnlockQueue)
            {
                QueueIndex = ChestService.Instance.AddChestToQueue(this);
                IsAddedToQueue = true;
                UIService.Instance.ShowConfirmationMessage(ConfirmationType.AddedToQueue);
            }
        }

        private void OpenChest()
        {
            ChestService.Instance.Resource.AddRewards(GetRandomValues(chestModel.MinCoin, chestModel.MaxCoin), GetRandomValues(chestModel.MinGem, chestModel.MaxGem));
            UIService.Instance.ShowConfirmationMessage(ConfirmationType.RewardsCollected);
            ChestService.Instance.ReturnSlot(SlotsControllersListIndex);
        }

        private int GetRandomValues(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public void StopTimer()
        {
            chestView.SetInstantActiveLayer(false);
            ChestService.Instance.ResetChestQueue();
            tokenSource.Cancel();
        }

        private async void StartTimer(CancellationToken token)
        {
            float hours, minutes, seconds;

            while(UnlockTimeInSeconds > -chestModel.OneSecond && CurrentStatus == ChestStatus.Unlocking)
            {
                hours = Mathf.FloorToInt(UnlockTimeInSeconds / chestModel.SecondsForHour);
                minutes = Mathf.FloorToInt((UnlockTimeInSeconds / chestModel.SixtySeconds) % chestModel.SixtySeconds);
                seconds = Mathf.FloorToInt(UnlockTimeInSeconds % chestModel.SixtySeconds);

                timerText.text = hours + chestModel.HourString + minutes + chestModel.MinuteString + seconds + chestModel.SecondString;

                gemsForTime = ((UnlockTimeInSeconds / chestModel.SixtySeconds) / chestModel.TenMinute);
                instantOpenGemText.text = "" + MathF.Ceiling(gemsForTime);

                try
                {
                    await Task.Delay(chestModel.DelayTimeInMilliSeconds, token);
                }
                catch(OperationCanceledException) when (token.IsCancellationRequested)
                {
                    return;
                }

                UnlockTimeInSeconds -= chestModel.OneSecond;
            }

            if (CurrentStatus != ChestStatus.Unlocked)
            {
                SetState(ChestStatus.Unlocked);
                ChestService.Instance.SetIsChestUnlocking(false);
                ChestService.Instance.FinishedUnlockingChest();
                QueueIndex--;
            }
        }

        private void SetState(ChestStatus chestStatus)
        {
            if(chestStatus == ChestStatus.Locked)
            {
                CurrentStatus = ChestStatus.Locked;
                chestView.UpdateImageAndText(chestModel.LockedImage, ChestService.Instance.GetChestStatusText((int)ChestStatus.Locked), chestModel.UnlockAmount);
            }

            if(chestStatus == ChestStatus.Unlocking)
            {
                CurrentStatus = ChestStatus.Unlocking;
                chestView.UpdateImageAndText(chestModel.LockedImage, ChestService.Instance.GetChestStatusText((int)ChestStatus.Unlocking), timerText.text);
                chestView.EnteredUnlockingStage(chestModel.TransparentValue);
                chestView.SetInstantActiveLayer(true);
            }

            if(chestStatus == ChestStatus.Unlocked)
            {
                CurrentStatus = ChestStatus.Unlocked;
                chestView.UpdateImageAndText(chestModel.UnlockedImage, ChestService.Instance.GetChestStatusText((int)ChestStatus.Unlocked), null);
                chestView.SetInstantActiveLayer(false);
            }
        }

        public void SetTimerText(TextMeshProUGUI _timerText)
        {
            timerText = _timerText;
        }

        public void SetRemainingTime(float _value)
        {
            UnlockTimeInSeconds = _value;
        }

        public ChestScriptableObject GetChestScriptableObject() => chestModel.ChestScriptableObject;
    }
}