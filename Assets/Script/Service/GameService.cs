using ChestSystem.GenericSingleton;
using ChestSystem.Resource;
using UnityEngine;

namespace ChestSystem.Service
{
    public class GameService : GenericSingleton<GameService>
    {
        [SerializeField] private InGameResource resource;
        [SerializeField] private int coins;
        [SerializeField] private int gems;

        private void Start()
        {
            resource.UpdateResource(coins, gems);
        }

        public bool IsRequireGemAvailable(int value)
        {
            if(resource.Gems < value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdatePurchaseCost(int cost)
        {
            resource.DeductPurchaseCost(cost);
        }

        public void AddRewards(int coins, int gems)
        {
            resource.UpdateResource(coins, gems);
        }
    }
}