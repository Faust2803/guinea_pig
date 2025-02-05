using Cysharp.Threading.Tasks;
using DataModels.PlayerData;
using Zenject;

namespace Managers
{
    public class PlayerManager
    {
        [Inject] private INetworkManager _networkManager;
        [Inject] private UiManager _uiManager;

        public string PlayerEmail  { get; private set; }
        public string PlayerName   { get; private set; }
        

        // public async UniTask SetUpAllPlayerData(PlayerDataModel playerDataModel)
        // {
        //     PlayerEmail = playerDataModel.email;
        //     PlayerName = playerDataModel.name;
        // }
        
        // public async UniTask UpdatePlayerData()
        // {
        //
        // }
        

        // public async UniTask<bool> ResetUser()
        // {
        //     var result = false;
        //     //result = await _networkManager.ResetUser();
        //     return result;
        // }
    }
}