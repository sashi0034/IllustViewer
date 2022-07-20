using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IllustViewer
{
    class UppingFlag
    {
        
        private bool isUpping = true;

        public UppingFlag()
        {}

        public bool IsUp => isUpping;

        public void MakeDown()
        {
            Debug.Assert(isUpping);
            isUpping = false;
        }
    }
}
