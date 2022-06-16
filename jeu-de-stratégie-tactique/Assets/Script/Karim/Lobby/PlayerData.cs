using Unity.Collections;
using Unity.Netcode;

namespace DapperDino.UMT.Lobby.Networking
{
    public struct PlayerData
    {
        public /*ForceNetworkSerializeByMemcpy<FixedString32Bytes>*/string PlayerName { get; private set; }
        public ulong ClientId { get; private set; }

        public PlayerData(/*ForceNetworkSerializeByMemcpy<FixedString32Bytes>*/string playerName, ulong clientId)
        {
            PlayerName = playerName;
            ClientId = clientId;
        }
    }
}
