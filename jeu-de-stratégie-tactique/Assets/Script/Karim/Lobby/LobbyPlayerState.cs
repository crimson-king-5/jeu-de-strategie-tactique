using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;

namespace TEAM2
{
    public struct LobbyPlayerState : INetworkSerializable, IEquatable<LobbyPlayerState>
    {
        public ulong ClientId;
        public ForceNetworkSerializeByMemcpy<FixedString32Bytes> PlayerName;
        public bool IsReady;

        public LobbyPlayerState(ulong clientId, ForceNetworkSerializeByMemcpy<FixedString32Bytes> playerName, bool isReady)
        {
            ClientId = clientId;
            PlayerName = playerName;
            IsReady = isReady;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref PlayerName);
            serializer.SerializeValue(ref IsReady);
        }

        public bool Equals(LobbyPlayerState other)
        {
            return ClientId == other.ClientId && PlayerName.Equals(other.PlayerName);
        }

    }

}