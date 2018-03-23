using GameFramework.Messaging;
using UnityEngine;

namespace InformalPenguins
{
    class ArcherDefeatedMessage : BaseMessage
    {
        public readonly GameObject archer;

        public ArcherDefeatedMessage(GameObject archer)
        {
            this.archer = archer;
        }
    }
}
