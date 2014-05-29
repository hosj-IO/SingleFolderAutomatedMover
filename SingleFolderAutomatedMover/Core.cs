using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleFolderAutomatedMover
{
    public static class Core
    {
        public static string Salt()
        {
            return "dtvFdrtkvc74NDVT040xSYiae401tKAfeT9t1XSQwuwtQbITnLRXDnE5iiUhJHL3krA4vWhVN3vLRbYIrEbLmMMIxJ8XigXDRcanIVj8Q8A8626zda3YeY10dOpUvQfoyaefwVSc7tIs7YWI2ix1CwQbhHA0xuQWHSQPohdzEwLZgLJWYsT9dAFhzXA1mjc3uZftgWv4";
        }

        public static bool IsSubfolder(string parentPath, string childPath)
        {
            var parentUri = new Uri(parentPath);

            var childUri = new DirectoryInfo(childPath).Parent;

            while (childUri != null)
            {
                if (new Uri(childUri.FullName) == parentUri)
                {
                    return true;
                }

                childUri = childUri.Parent;
            }

            return false;
        }

    }
}
