using System.Collections.Generic;
using LazySoccer.SceneLoading.Buildings.OfficeEmblem;
using LazySoccer.SceneLoading.PlayerData.Enum;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace Scripts.Infrastructure.ScriptableStore
{
    [CreateAssetMenu(fileName = "StatusSprites", menuName = "Building/StatusSprites", order = 1)]
    public class StatusSprites : ScriptableObject
    {
        [SerializeField]
        private List<StatusSprite> statusSpriteList;

        [System.Serializable]
        public class StatusSprite
        {
            public TeamPlayerStatus Status;
            public Sprite statusSprite;
        }

        public Sprite Get(TeamPlayerStatus playerStatus)
        {
            return statusSpriteList.Find(icon => icon.Status == playerStatus).statusSprite;
        }
    }
}