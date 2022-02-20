
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Cyan.WallWalkingExamples
{
    public class StationController : UdonSharpBehaviour
    {
        public VRCStation station;
        public Transform syncObj;
        public Transform stationPos;
        public Transform origin;

        private VRCPlayerApi _usingPlayer;

        public void _EnterStation()
        {
            station.PlayerMobility = VRCStation.Mobility.Mobile;
            VRCPlayerApi player = Networking.LocalPlayer;
            Networking.SetOwner(player, syncObj.gameObject);
            _UpdateLocal();
            station.UseStation(player);
        }

        public override void OnStationEntered(VRCPlayerApi player)
        {
            _usingPlayer = player;
        }

        public override void OnStationExited(VRCPlayerApi player)
        {
            _usingPlayer = null;
            station.PlayerMobility = VRCStation.Mobility.Immobilize;
            if (player.isLocal)
            {
                _UpdateLocal();
            }
        }

        private void Update()
        {
            if (!Utilities.IsValid(_usingPlayer))
            {
                return;
            }

            if (_usingPlayer.isLocal)
            {
                _UpdateLocal();
            }
            else
            {
                _UpdateRemote();
            }
        }

        private void _UpdateLocal()
        {
            // Update the station's position to be the player's position
            VRCPlayerApi player = Networking.LocalPlayer;
            Vector3 pos = player.GetPosition();
            Quaternion rot = player.GetRotation();
            stationPos.SetPositionAndRotation(pos, rot);

            // Get the player's position and rotation relative to the room origin. 
            var matrix = origin.worldToLocalMatrix;
            Vector3 systemPos = matrix.MultiplyPoint(pos);
            Quaternion systemRot = matrix.rotation * rot;
            syncObj.SetPositionAndRotation(systemPos, systemRot);
        }

        private void _UpdateRemote()
        {
            // Get the relative position of this player from the synced object.
            Vector3 pos = syncObj.position;
            Quaternion rot = syncObj.rotation;

            // Take the synced position and apply the room origin's transform to that position.
            var matrix = origin.localToWorldMatrix;
            Vector3 systemPos = matrix.MultiplyPoint(pos);
            Quaternion systemRot = matrix.rotation * rot;
            stationPos.SetPositionAndRotation(systemPos, systemRot);
        }
    }
}