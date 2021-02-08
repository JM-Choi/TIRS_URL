#region Imports
#endregion

#region Program
namespace TechFloor.Components
{
    public class ReelTowerMessage
    {
        #region Fields
        public readonly int Tick;

        public ReelTowerCommands Command = ReelTowerCommands.REPLY_LINK_TEST;

        public string Data = string.Empty;
        #endregion

        #region Constructors
        public ReelTowerMessage(ReelTowerCommands command, string data = null)
        {
            Command = command;
            Data = data;
            Tick = App.TickCount;
        }
        #endregion
    }
}
#endregion