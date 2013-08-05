using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Utils
{
    class UnCodebase
    {
        public Bitmap bmpobj;
        public void Save(String fileName)
        {
            bmpobj.Save(fileName);

        }
        public UnCodebase(Bitmap pic)
        {
            //       if (pic.PixelFormat == PixelFormat.Format8bppIndexed)
            bmpobj = new Bitmap(pic);    //ת��ΪFormat32bppRgb
        }
        public void clear1()
        {
            List<Point> del = new List<Point>();
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    if (!hasMoreNB(j, i))
                    {
                        del.Add(new Point(j, i));
                    }
                }
            }
            for (int t = 0; t < del.Count;t++ )
            {
                bmpobj.SetPixel(del[t].X, del[t].Y, Color.White);
            }
        }
        private Boolean hasMoreNB(int j,int i){

            if (j - 1 < 0 || j + 1 >= bmpobj.Width
                || i - 1 < 0 || i + 1 >= bmpobj.Height)
                return false;
            int n = 0;
            Color c = Color.White;
                c = bmpobj.GetPixel(j-1, i);
            if (c != Color.White) n = n + 1;
                c = bmpobj.GetPixel(j + 1, i);
            if (c != Color.White) n = n + 1;
                c = bmpobj.GetPixel(j , i-1);
            if (c != Color.White) n = n + 1;
              c = bmpobj.GetPixel(j, i+1);
            if (c != Color.White) n = n + 1;
                c = bmpobj.GetPixel(j + 1, i+1);
            if (c != Color.White) n = n + 1;
                c = bmpobj.GetPixel(j - 1, i-1);
            if (c != Color.White) n = n + 1;
            if (n >=3) return true;
            return false;
        }
        public void clearColor()
        {
            List<Color> border = new List<Color>();
            for (int t = 0; t < bmpobj.Height;t++ )
            {

                Color c = bmpobj.GetPixel(bmpobj.Width-3, t);
                border.Add(c);
                c = bmpobj.GetPixel(bmpobj.Width - 5, t);
                border.Add(c);
                c = bmpobj.GetPixel(bmpobj.Width -7, t);
                border.Add(c);
            }
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    Color c=bmpobj.GetPixel(j, i);
                    int offset=8;
                    for (int t = 0; t < border.Count; t++)
                    {
                        Color ct = border[t];
                        if (Math.Abs(ct.R - c.R) < offset
                            && Math.Abs(ct.R - c.R) < offset
                            && Math.Abs(ct.R - c.R) < offset)
                        {
                            bmpobj.SetPixel(j, i, Color.White);
                            t = 9999;
                        }
                    }//end t                    
                    //bmpobj.SetPixel(j, i, Color.FromArgb(tmpValue, tmpValue, tmpValue));
                }
            }
        }

        /// <summary>
        /// ����RGB������Ҷ�ֵ
        /// </summary>
        /// <param name="posClr">Colorֵ</param>
        /// <returns>�Ҷ�ֵ������</returns>
        private int GetGrayNumColor(System.Drawing.Color posClr)
        {
            return (posClr.R * 19595 + posClr.G * 38469 + posClr.B * 7472) >> 16;
        }

        /// <summary>
        /// �Ҷ�ת��,��㷽ʽ
        /// </summary>
        public void GrayByPixels()
        {
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int tmpValue = GetGrayNumColor(bmpobj.GetPixel(j, i));
                    bmpobj.SetPixel(j, i, Color.FromArgb(tmpValue, tmpValue, tmpValue));
                }
            }
        }

        /// <summary>
        /// ȥͼ�α߿�
        /// </summary>
        /// <param name="borderWidth"></param>
        public void ClearPicBorder(int borderWidth)
        {
            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    if (i < borderWidth || j < borderWidth || j > bmpobj.Width - 1 - borderWidth || i > bmpobj.Height - 1 - borderWidth)
                        bmpobj.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }
            }
        }

        /// <summary>
        /// �Ҷ�ת��,���з�ʽ
        /// </summary>
        public void GrayByLine()
        {
            Rectangle rec = new Rectangle(0, 0, bmpobj.Width, bmpobj.Height);
            BitmapData bmpData = bmpobj.LockBits(rec, ImageLockMode.ReadWrite, bmpobj.PixelFormat);// PixelFormat.Format32bppPArgb);
            //    bmpData.PixelFormat = PixelFormat.Format24bppRgb;
            IntPtr scan0 = bmpData.Scan0;
            int len = bmpobj.Width * bmpobj.Height;
            int[] pixels = new int[len];
            Marshal.Copy(scan0, pixels, 0, len);

            //��ͼƬ���д���
            int GrayValue = 0;
            for (int i = 0; i < len; i++)
            {
                GrayValue = GetGrayNumColor(Color.FromArgb(pixels[i]));
                pixels[i] = (byte)(Color.FromArgb(GrayValue, GrayValue, GrayValue)).ToArgb();      //Colorתbyte
            }

            bmpobj.UnlockBits(bmpData);

            ////���
            //GCHandle gch = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            //bmpOutput = new Bitmap(bmpobj.Width, bmpobj.Height, bmpData.Stride, bmpData.PixelFormat, gch.AddrOfPinnedObject());
            //gch.Free();
        }

        /// <summary>
        /// �õ���Чͼ�β�����Ϊ��ƽ���ָ�Ĵ�С
        /// </summary>
        /// <param name="dgGrayValue">�Ҷȱ����ֽ�ֵ</param>
        /// <param name="CharsCount">��Ч�ַ���</param>
        /// <returns></returns>
        public void GetPicValidByValue(int dgGrayValue, int CharsCount)
        {
            int posx1 = bmpobj.Width; int posy1 = bmpobj.Height;
            int posx2 = 0; int posy2 = 0;
            for (int i = 0; i < bmpobj.Height; i++)      //����Ч��
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int pixelValue = bmpobj.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //���ݻҶ�ֵ
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    };
                };
            };
            // ȷ��������
            int Span = CharsCount - (posx2 - posx1 + 1) % CharsCount;   //�������Ĳ����
            if (Span < CharsCount)
            {
                int leftSpan = Span / 2;    //���䵽��ߵĿ��� ����spanΪ����,���ұ߱���ߴ�1
                if (posx1 > leftSpan)
                    posx1 = posx1 - leftSpan;
                if (posx2 + Span - leftSpan < bmpobj.Width)
                    posx2 = posx2 + Span - leftSpan;
            }
            //������ͼ
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            bmpobj = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);
        }

        /// <summary>
        /// �õ���Чͼ��,ͼ��Ϊ�����
        /// </summary>
        /// <param name="dgGrayValue">�Ҷȱ����ֽ�ֵ</param>
        /// <param name="CharsCount">��Ч�ַ���</param>
        /// <returns></returns>
        public void GetPicValidByValue(int dgGrayValue)
        {
            int posx1 = bmpobj.Width; int posy1 = bmpobj.Height;
            int posx2 = 0; int posy2 = 0;
            for (int i = 0; i < bmpobj.Height; i++)      //����Ч��
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    int pixelValue = bmpobj.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //���ݻҶ�ֵ
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    };
                };
            };
            //������ͼ
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            bmpobj = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);
        }

        /// <summary>
        /// �õ���Чͼ��,ͼ�������洫��
        /// </summary>
        /// <param name="dgGrayValue">�Ҷȱ����ֽ�ֵ</param>
        /// <param name="CharsCount">��Ч�ַ���</param>
        /// <returns></returns>
        public Bitmap GetPicValidByValue(Bitmap singlepic, int dgGrayValue)
        {
            int posx1 = singlepic.Width; int posy1 = singlepic.Height;
            int posx2 = 0; int posy2 = 0;
            for (int i = 0; i < singlepic.Height; i++)      //����Ч��
            {
                for (int j = 0; j < singlepic.Width; j++)
                {
                    int pixelValue = singlepic.GetPixel(j, i).R;
                    if (pixelValue < dgGrayValue)     //���ݻҶ�ֵ
                    {
                        if (posx1 > j) posx1 = j;
                        if (posy1 > i) posy1 = i;

                        if (posx2 < j) posx2 = j;
                        if (posy2 < i) posy2 = i;
                    };
                };
            };
            //������ͼ
            Rectangle cloneRect = new Rectangle(posx1, posy1, posx2 - posx1 + 1, posy2 - posy1 + 1);
            return singlepic.Clone(cloneRect, singlepic.PixelFormat);
        }

        /// <summary>
        /// ƽ���ָ�ͼƬ
        /// </summary>
        /// <param name="RowNum">ˮƽ�Ϸָ���</param>
        /// <param name="ColNum">��ֱ�Ϸָ���</param>
        /// <returns>�ָ�õ�ͼƬ����</returns>
        public Bitmap[] GetSplitPics(int RowNum, int ColNum)
        {
            if (RowNum == 0 || ColNum == 0)
                return null;
            int singW = bmpobj.Width / RowNum;
            int singH = bmpobj.Height / ColNum;
            Bitmap[] PicArray = new Bitmap[RowNum * ColNum];

            Rectangle cloneRect;
            for (int i = 0; i < ColNum; i++)      //����Ч��
            {
                for (int j = 0; j < RowNum; j++)
                {
                    cloneRect = new Rectangle(j * singW, i * singH, singW, singH);
                    PicArray[i * RowNum + j] = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);//����С��ͼ
                }
            }
            return PicArray;
        }

        /// <summary>
        /// ���ػҶ�ͼƬ�ĵ��������ִ���1��ʾ�ҵ㣬0��ʾ����
        /// </summary>
        /// <param name="singlepic">�Ҷ�ͼ</param>
        /// <param name="dgGrayValue">��ǰ����ɫ����</param>
        /// <returns></returns>
        public string GetSingleBmpCode(Bitmap singlepic, int dgGrayValue)
        {
            Color piexl;
            string code = "";
            for (int posy = 0; posy < singlepic.Height; posy++)
                for (int posx = 0; posx < singlepic.Width; posx++)
                {
                    piexl = singlepic.GetPixel(posx, posy);
                    if (piexl.R < dgGrayValue)    // Color.Black )
                        code = code + "1";
                    else
                        code = code + "0";
                }
            return code;
        }

        /// <summary>
        /// �õ��Ҷ�ͼ��ǰ���������ٽ�ֵ �����䷽���yuanbao,2007.08
        /// </summary>
        /// <returns>ǰ���������ٽ�ֵ</returns>
        public int GetDgGrayValue()
        {
            int[] pixelNum = new int[256];           //ͼ��ֱ��ͼ����256����
            int n, n1, n2;
            int total;                              //totalΪ�ܺͣ��ۼ�ֵ
            double m1, m2, sum, csum, fmax, sb;     //sbΪ��䷽�fmax�洢��󷽲�ֵ
            int k, t, q;
            int threshValue = 1;                      // ��ֵ
            int step = 1;
            //����ֱ��ͼ
            for (int i = 0; i < bmpobj.Width; i++)
            {
                for (int j = 0; j < bmpobj.Height; j++)
                {
                    //���ظ��������ɫ����RGB��ʾ
                    pixelNum[bmpobj.GetPixel(i, j).R]++;            //��Ӧ��ֱ��ͼ��1
                }
            }
            //ֱ��ͼƽ����
            for (k = 0; k <= 255; k++)
            {
                total = 0;
                for (t = -2; t <= 2; t++)              //�븽��2���Ҷ���ƽ������tֵӦȡ��С��ֵ
                {
                    q = k + t;
                    if (q < 0)                     //Խ�紦��
                        q = 0;
                    if (q > 255)
                        q = 255;
                    total = total + pixelNum[q];    //totalΪ�ܺͣ��ۼ�ֵ
                }
                pixelNum[k] = (int)((float)total / 5.0 + 0.5);    //ƽ���������2��+�м�1��+�ұ�2���Ҷȣ���5���������ܺͳ���5�������0.5��������ֵ
            }
            //����ֵ
            sum = csum = 0.0;
            n = 0;
            //�����ܵ�ͼ��ĵ����������أ�Ϊ����ļ�����׼��
            for (k = 0; k <= 255; k++)
            {
                sum += (double)k * (double)pixelNum[k];     //x*f(x)�����أ�Ҳ����ÿ���Ҷȵ�ֵ�������������һ����Ϊ���ʣ���sumΪ���ܺ�
                n += pixelNum[k];                       //nΪͼ���ܵĵ�������һ��������ۻ�����
            }

            fmax = -1.0;                          //��䷽��sb������Ϊ��������fmax��ʼֵΪ-1��Ӱ�����Ľ���
            n1 = 0;
            for (k = 0; k < 256; k++)                  //��ÿ���Ҷȣ���0��255������һ�ηָ�����䷽��sb
            {
                n1 += pixelNum[k];                //n1Ϊ�ڵ�ǰ��ֵ��ǰ��ͼ��ĵ���
                if (n1 == 0) { continue; }            //û�зֳ�ǰ����
                n2 = n - n1;                        //n2Ϊ����ͼ��ĵ���
                if (n2 == 0) { break; }               //n2Ϊ0��ʾȫ�����Ǻ�ͼ����n1=0������ƣ�֮��ı���������ʹǰ���������ӣ����Դ�ʱ�����˳�ѭ��
                csum += (double)k * pixelNum[k];    //ǰ���ġ��Ҷȵ�ֵ*����������ܺ�
                m1 = csum / n1;                     //m1Ϊǰ����ƽ���Ҷ�
                m2 = (sum - csum) / n2;               //m2Ϊ������ƽ���Ҷ�
                sb = (double)n1 * (double)n2 * (m1 - m2) * (m1 - m2);   //sbΪ��䷽��
                if (sb > fmax)                  //����������䷽�����ǰһ���������䷽��
                {
                    fmax = sb;                    //fmaxʼ��Ϊ�����䷽�otsu��
                    threshValue = k;              //ȡ�����䷽��ʱ��Ӧ�ĻҶȵ�k���������ֵ
                }
            }
            return threshValue;
        }
        /// <summary>
        /// 3��3��ֵ�˲����ӣ�yuanbao,2007.10
        /// </summary>
        /// <param name="dgGrayValue"></param>
        public void ClearNoise(int dgGrayValue)
        {
            int x, y;
            byte[] p = new byte[9]; //��С������3*3
            byte s;
            //byte[] lpTemp=new BYTE[nByteWidth*nHeight];
            int i, j;

            //--!!!!!!!!!!!!!!���濪ʼ����Ϊ3��3��ֵ�˲�!!!!!!!!!!!!!!!!
            for (y = 1; y < bmpobj.Height - 1; y++) //--��һ�к����һ���޷�ȡ����
            {
                for (x = 1; x < bmpobj.Width - 1; x++)
                {
                    //ȡ9�����ֵ
                    p[0] = bmpobj.GetPixel(x - 1, y - 1).R;
                    p[1] = bmpobj.GetPixel(x, y - 1).R;
                    p[2] = bmpobj.GetPixel(x + 1, y - 1).R;
                    p[3] = bmpobj.GetPixel(x - 1, y).R;
                    p[4] = bmpobj.GetPixel(x, y).R;
                    p[5] = bmpobj.GetPixel(x + 1, y).R;
                    p[6] = bmpobj.GetPixel(x - 1, y + 1).R;
                    p[7] = bmpobj.GetPixel(x, y + 1).R;
                    p[8] = bmpobj.GetPixel(x + 1, y + 1).R;
                    //������ֵ
                    for (j = 0; j < 5; j++)
                    {
                        for (i = j + 1; i < 9; i++)
                        {
                            if (p[j] > p[i])
                            {
                                s = p[j];
                                p[j] = p[i];
                                p[i] = s;
                            }
                        }
                    }
                    //      if (bmpobj.GetPixel(x, y).R < dgGrayValue)
                    bmpobj.SetPixel(x, y, Color.FromArgb(p[4], p[4], p[4]));    //����Чֵ����ֵ
                }
            }
        }
        /// <summary>
        ///  ȥ���ӵ㣨�ʺ��ӵ�/���ߴ�Ϊ1��
        /// </summary>
        /// <param name="dgGrayValue">��ǰ����ɫ����</param>
        /// <returns></returns>
        public void ClearNoise(int dgGrayValue, int MaxNearPoints)
        {
            Color piexl;
            int nearDots = 0;
            int XSpan, YSpan, tmpX, tmpY;
            //����ж�
            for (int i = 0; i < bmpobj.Width; i++)
                for (int j = 0; j < bmpobj.Height; j++)
                {
                    piexl = bmpobj.GetPixel(i, j);
                    if (piexl.R < dgGrayValue)
                    {
                        nearDots = 0;
                        //�ж���Χ8�����Ƿ�ȫΪ��
                        if (i == 0 || i == bmpobj.Width - 1 || j == 0 || j == bmpobj.Height - 1)  //�߿�ȫȥ��
                        {
                            bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            if (bmpobj.GetPixel(i - 1, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j - 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i - 1, j).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i - 1, j + 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i, j + 1).R < dgGrayValue) nearDots++;
                            if (bmpobj.GetPixel(i + 1, j + 1).R < dgGrayValue) nearDots++;
                        }

                        if (nearDots < MaxNearPoints)
                            bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));   //ȥ������ && ��ϸС3�ڱߵ�
                    }
                    else  //����
                        bmpobj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
        }



    }
}
