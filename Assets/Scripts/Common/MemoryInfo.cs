using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Common {

    public class MemoryInfoPlugin {
        [DllImport( "__Internal" )]
        private static extern UInt32 MemoryStat_GetFreeMemory();

        public static String FormatMemoryNumber( UInt32 memSize ) {
            if ( memSize < 1024 ) {
                return memSize.ToString();
            }
            if ( memSize < 1024 * 1024 ) {
                float memSizeKB = ( (float)memSize ) / 1024.0f;
                return memSizeKB.ToString( "0.00" ) + "KB";
            }
            float memSizeMB = ( (float)memSize ) / 1024.0f / 1024.0f;
            return memSizeMB.ToString( "0.00" ) + "MB";
        }

        public static String GetFreeMemoryString() {
            return FormatMemoryNumber( GetFreeMemory() );
        }

        public static UInt32 GetFreeMemory() {
#if UNITY_EDITOR
            return 0;
#else
            return MemoryStat_GetFreeMemory();
#endif
        }
    }
}
