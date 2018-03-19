using GameFramework.Messaging;
using UnityEngine;

namespace InformalPenguins
{
    class RabbitAddedMessage : BaseMessage
    {
        public readonly GameObject rabbit;

        public RabbitAddedMessage(GameObject rabbit)
        {
            this.rabbit = rabbit;
        }
    }
}
