using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using ComizoaSDK;
using Motion.Comizoa;

namespace IO.Comizoa
{
    public class ComiD_IOFunc
    {      
        public static bool  DeviceOpen()
        {
            return true;
        }

        public static bool DeviceClose()
        {
            return true;
        }

        public static bool GetInputState(int nChannel)
        {
            if (IOMain.MAX_INPUT <= nChannel) return false;

            int nState = (int)Defines._TCmBool.cmFALSE;

            if (CMDLL.cmmDiGetOne(nChannel, ref nState) != Defines.cmERR_NONE) return false;

            if (nState == (int)Defines._TCmBool.cmFALSE)
                return false;            
	
	        return true;
        }

        public static bool GetOutputState(int nChannel)
        {
            if (IOMain.MAX_OUTPUT <= nChannel) return false;

            int nState = (int)Defines._TCmBool.cmFALSE;

            if (CMDLL.cmmDoGetOne(nChannel, ref nState) != Defines.cmERR_NONE) return false;

            if (nState == (int)Defines._TCmBool.cmFALSE)
                return false;

            return true;
        }

        public static bool Output(int nChannel, int nState)
        {
            if (IOMain.MAX_OUTPUT <= nChannel) return false;

            if (CMDLL.cmmDoPutOne(nChannel, nState) != Defines.cmERR_NONE) return false;
            
            return true;
        }
    }
}
