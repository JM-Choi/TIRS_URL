#region Imports
using System.Text;
#endregion

#region Program
namespace TechFloor.Components
{
    public class ReelTowerObject
    {
        #region Fields
        public int Id = 0;

        public AsyncSocketClient AsyncSocket = null;
        #endregion

        #region Constructors
        public ReelTowerObject(AsyncSocketClient sock, int clientid)
        {
            AsyncSocket = sock;
            Id = clientid;
        }
        #endregion

        #region Public methods
        public void Send(string message)
        {
            AsyncSocket.Send(Encoding.Default.GetBytes(message));
        }
        #endregion
    }
}
#endregion