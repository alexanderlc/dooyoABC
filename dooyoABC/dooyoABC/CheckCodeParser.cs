using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Utils
{
    public class CheckCodeParser
    {
        public CheckCodeParser()
        {
            initBmpSamples();
        }
        Bitmap[] samples = new Bitmap[10];
        private void initBmpSamples()
        {
            string curr = Application.StartupPath + "\\bmp\\";
            int i = 0;
            foreach (string path in System.IO.Directory.GetFiles(curr, "*.bmp"))
            {
                Bitmap bmp = (Bitmap)Bitmap.FromFile(path);
                samples[i++] = bmp;
            }
        }

        public  String parse(System.Drawing.Image image)
        {
            String str = "";
            Bitmap[] bmps = GetSplitPics((Bitmap)image, 4, 1);

            for (int i = 0; i < bmps.Length; i++)
            {
                for (int j = 0; j < samples.Length; j++)
                {
                    if (ImageCompareArray(bmps[i], samples[j]))
                    {
                        str = str + j.ToString();                        
                        break;

                    }
                }

            }


            return str;
        }
        private  Bitmap[] GetSplitPics(Bitmap bmp, int ColNum, int RowNum)
        {
            if (RowNum == 0 || ColNum == 0)
                return null;

            int singW = (bmp.Width - 1) / ColNum;
            Console.WriteLine("width :{0}", singW);
            int singH = bmp.Height / RowNum;
            Console.WriteLine("height :{0}", singH);
            Bitmap[] PicArray = new Bitmap[RowNum * ColNum];

            Rectangle cloneRect;
            for (int i = 0; i < ColNum; i++)
            {
                cloneRect = new Rectangle(i * singW, 0, singW, singH);
                PicArray[i] = bmp.Clone(cloneRect, bmp.PixelFormat);//复制小块图
            }
            return PicArray;
        }

        public bool ImageCompareArray(Bitmap firstImage, Bitmap secondImage)
        {
            bool flag = true;
            string firstPixel;
            string secondPixel;

            if (firstImage.Width == secondImage.Width
                  && firstImage.Height == secondImage.Height)
            {
                for (int i = 0; i < firstImage.Width; i++)
                {
                    for (int j = 0; j < firstImage.Height; j++)
                    {
                        firstPixel = firstImage.GetPixel(i, j).ToString();
                        secondPixel = secondImage.GetPixel(i, j).ToString();
                        if (firstPixel != secondPixel)
                        {
                            flag = false;
                            break;
                        }
                    }
                }


                if (flag == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
