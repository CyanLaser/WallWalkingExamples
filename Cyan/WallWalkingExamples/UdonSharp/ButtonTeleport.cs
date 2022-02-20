
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Cyan.WallWalkingExamples
{
    public class ButtonTeleport : UdonSharpBehaviour
    {
        public Transform teleportDestination;
        public Transform room;
        public Origin origin;

        public override void Interact()
        {
            Networking.LocalPlayer.TeleportTo(teleportDestination.position, teleportDestination.rotation);
            origin._EnterRoom(room);
        }
    }
}