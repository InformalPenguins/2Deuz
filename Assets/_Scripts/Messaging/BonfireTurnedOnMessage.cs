using GameFramework.Messaging;
using UnityEngine;

namespace InformalPenguins
{
    class BonfireChangedMessage : BaseMessage
    {
        public readonly bool IsTurnedOn;

        public BonfireChangedMessage(bool isTurnedOn)
        {
            this.IsTurnedOn = isTurnedOn;
        }
    }
}
