using UnityEngine;

namespace ChestSystem.Game
{
    [CreateAssetMenu(fileName = "GameSave", menuName = "ScriptableObject/NewGameSave")]
    public class GameSaveScriptableObject : ScriptableObject
    {
        public int Coins;
        public int Gems;
    }
}