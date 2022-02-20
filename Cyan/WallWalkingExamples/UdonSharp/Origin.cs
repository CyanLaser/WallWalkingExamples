
using Cyan.PlayerObjectPool;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Cyan.WallWalkingExamples
{
    public class Origin : UdonSharpBehaviour
    {
        public CyanPlayerObjectAssigner pool;
        [HideInInspector]
        public UdonBehaviour playerAssignedPoolObject;

        private StationController _playerStation;
        
        public void _EnterRoom(Transform room)
        {
            transform.SetPositionAndRotation(room.position, room.rotation);
            _playerStation._EnterStation();
        }

        [PublicAPI]
        public void _OnLocalPlayerAssigned()
        {
            _playerStation = (StationController)pool._GetPlayerPooledUdon(Networking.LocalPlayer);
        }

        [PublicAPI]
        public void _OnPlayerAssigned()
        {
            ((StationController)(Component)playerAssignedPoolObject).origin = transform;
        }
    }
}