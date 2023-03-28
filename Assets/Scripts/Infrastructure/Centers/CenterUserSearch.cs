using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.CommunityCenter;
using Scripts.Infrastructure.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterUserSearch : CommunityCenterFriends
    {
        [Title("Input Field")] 
        [SerializeField] private TMP_InputField inputSearch;

        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        
        private void Awake()
        {
            base.Awake();
            
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
            inputSearch.onValueChanged.AddListener(UpdateUserlistInput);
        }

        public void OnEnable()
        {
            inputSearch.text = inputSearch.text;
        }

        public void UpdateUserlistInput(string nickName)
        {
            UpdateUserlist(nickName);
        }

        private string _lastWord;
        public override async UniTask UpdateUserlist()
        {
            await UpdateUserlist(_lastWord);
        }

        private bool _loading;
        private async UniTask UpdateUserlist(string nickName)
        {
            if (!ValidationUtils.StringNicknameValid(nickName))
            {
                DeleteAllSlots();
                return;
            }

            await UniTask.WaitUntil(() => !_loading);
            
            _loading = true;
            
            var users = await _communityCenterTypesOfRequest.GET_Users(nickName);

            _lastWord = nickName;
            
            _loading = false;
            
            DeleteAllSlots();

            for (var i = 0; i < users.Count; i++)
            {
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out CommunityCenterFriendsSlot slot))
                {
                    slot.SetInfo(users[i], i + 1);
                    slot.SetEmblem(GetUserSprite(users[i]), GetEmblemSprite(users[i]));
                    slot.SetFriendList(this);

                    CreateDisplayListener(slot);
                    
                    PlayerSlots.Add(slot);
                }

                AddButtonListener(slotInstance);
            }
        }

        protected virtual void AddButtonListener(GameObject slotInstance) { }

    }
}