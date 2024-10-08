using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ECAR.Framework.PDF
{
    public static class PDFConstants
    {
        public enum OrientationPDF { VERTICAL, HORIZONTAL };
        //public enum BorderCellPDF { NO_BORDER = 0, BOTTOM_BORDER = 1, TOP_BORDER = 2, LEFT_BORDER = 3, RIGHT_BORDER = 4,
        //                            ALL_BORDER = 5, BOTTOM_LEFT_BORDER = 6, BOTTOM_RIGHT_BORDER = 7, TOP_LEFT_BORDER = 8,
        //                            TOP_RIGHT_BORDER = 9 }


        public enum BorderCellPDF
        {
            NO_BORDER = 0,
            TOP_BORDER = 1,
            BOTTOM_BORDER = 2,
            LEFT_BORDER = 4,
            RIGHT_BORDER = 8,
        }

    }
}
