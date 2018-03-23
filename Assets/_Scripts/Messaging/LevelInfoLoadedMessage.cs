using GameFramework.Messaging;

namespace InformalPenguins
{
    class LevelInfoLoadedMessage : BaseMessage
    {
        public readonly LevelInfo levelInfo;

        public LevelInfoLoadedMessage(LevelInfo levelInfo)
        {
            this.levelInfo = levelInfo;
        }
    }
}
