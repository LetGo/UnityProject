using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common {
    public class StringUtils {

        public static void SplitFilename( String qualifiedName, out String outBasename, out String outPath ) {
            String path = qualifiedName.Replace( '\\', '/' );
            int i = path.LastIndexOf( '/' );
            if ( i == -1 ) {
                outPath = String.Empty;
                outBasename = qualifiedName;
            } else {
                outBasename = path.Substring( i + 1, path.Length - i - 1 );
                outPath = path.Substring( 0, i + 1 );
            }
        }

        public static String StandardisePath( String init ) {
            String path = init.Replace( '\\', '/' );
            if ( path.Length > 0 && path[path.Length - 1] != '/' ) {
                path += '/';
            }
            return path;
        }

        public static void SplitBaseFilename( String fullName, out String outBasename, out String outExtention ) {
            int i = fullName.LastIndexOf( '.' );
            if ( i == -1 ) {
                outExtention = String.Empty;
                outBasename = fullName;
            } else {
                outExtention = fullName.Substring( i + 1 );
                outBasename = fullName.Substring( 0, i );
            }
        }

        public static int CountOf( String str, char what ) {
	        int count = 0;
            for ( int i = 0; i < str.Length; ++i ) {
                if ( str[i] == what ) {
                    ++count;
                }
            }
	        return count;
        }

        public static void SplitFullFilename( String qualifiedName, out String outBasename, out String outExtention, out String outPath ) {
            String fullName = String.Empty;
            SplitFilename( qualifiedName, out fullName, out outPath );
            SplitBaseFilename( fullName, out outBasename, out outExtention );
        }
    }
}
