using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Utils
{
    public class DoubleBufferListView : ListView
    {
        public DoubleBufferListView()
        {
            //双缓冲，可以改善ListView的显示闪烁问题
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
