using GameFramework.Messaging;
using UnityEngine;

namespace InformalPenguins
{
    class ExtinguisherUsedMessage : BaseMessage
    {
        public readonly GameObject extinguisher;

        public ExtinguisherUsedMessage(GameObject extinguisher)
        {
            this.extinguisher = extinguisher;
        }
    }
}
