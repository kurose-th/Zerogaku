using System;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Fortune;


namespace uranai
{
    class Zero
    {


        public int calcjulius(int year, int month, int day){
            int julius, y, m, a, b;
            if(month < 2){
                y = year;
                m = month;
            }else{
                y = year - 1;
                m = month + 12;
            }
            a = (int)Math.Floor( (double)(y) / 100.0 );
            b = 2 - a + a/4;
            julius = (int)Math.Floor(Math.Floor(365.25*y) + Math.Floor(30.60*(m + 1)) + day + 1720994.5 + b + 0.5);
            return julius;
        }

        public int calcshihai(int year, int julius)
        {
            int star = julius % 60;
            int n;
            int number = 0;

            if (year % 2 == 0)
            {
                n = 0;
            }
            else
            {
                n = 1;
            }

            if (0 < star && star <= 10)
            {
                number = 0 + n;
            }
            else if (star >= 11 && star <= 20)
            {
                number = 2 + n;
            }
            else if (star >= 21 && star <= 30)
            {
                number = 4 + n;
            }
            else if (star >= 31 && star <= 40)
            {
                number = 6 + n;
            }
            else if (star >= 41 && star <= 50)
            {
                number = 8 + n;
            }
            else if ((star >= 51 && star <= 59) || star == 0)
            {
                number = 10 + n;
            }
            return number;
        }

        public void printOut(string[] args){
            PrintOut printer = new PrintOut();
            printer.LandScape = false;
            printer.IsPreview = true;

            int year = int.Parse(args[2]);
            int month = int.Parse(args[3]);
            int day = int.Parse(args[4]);
            string name = args[0] + " " + args[1];
            int x_center = 105;
            int y = 5;
            int julius = calcjulius(year, month, day);
            int number = calcshihai(year, julius);
            string openfilename = "./text/";
            string openimgname = "./img/";
            string[] openfilelist = {"suisei.txt", "hyouousei.txt", "mokusei.txt", "kaiousei.txt", "getsusei.txt",
                  "gyoousei.txt", "kasei.txt", "meiousei.txt", "kinsei.txt", "shouousei.txt", "dosei.txt", "tenousei.txt"};
            string[] openimglist = {"kettei.png", "seicho.png", "kaitaku.png", "seisan.png", "0chiten.png",
                  "haishin.png", "jujitsu.png", "keizai.png", "saikai.png", "uwaki.png", "ninki.png", "kenko.png"};
            string[] openimglist2 = {"1.jpg", "0.jpg", "11.jpg", "10.jpg", "9.jpg",
                  "8.jpg", "7.jpg", "6.jpg", "5.jpg", "4.jpg", "3.jpg", "2.jpg"};
            string[] shihai = {"水星", "氷王星", "木星", "海王星", "月星", "魚王星", "火星", "冥王星", "金星", "小王星", "土星", "天王星"};


            printer.evPrint += delegate(object sender, PrintOutEventArgs e)
            {
                Print print = e.print;
                print.SetFont("MS ゴシック", 25);
                print.DrawImageResize(new Bitmap(openimgname + "title.png"), 65, 0, 80, 25);
                print.DrawImageResize(new Bitmap(openimgname + "shihai/" + openimglist2[number]), 165, -3, 40, 40);
                print.SetFont("MS ゴシック", 18);
                print.DrawStringCentering(name + "さんの支配星は、" + shihai[number] + "です", x_center, y + 20);
                print.DrawLine( 0, y + 30, 220, y + 30);
                print.SetFont("MS ゴシック", 13);
                System.IO.StreamReader sr = new System.IO.StreamReader(
                    openfilename + "feature/" + openfilelist[number],
                    System.Text.Encoding.GetEncoding("utf-8"));
                int i = 0;
                while (sr.Peek() > -1)
                {
                    print.DrawStringCentering(sr.ReadLine(), x_center, y + 33 + i*6);
                    i++;
                }
                sr.Close();
                print.DrawLine(0, y + 107, 220, y + 107);
                print.SetFont("MS ゴシック", 18);
                print.DrawStringCentering(name + "さんの運命グラフ", x_center, y + 110);
                print.DrawImageResize(new Bitmap(openimgname + "graph/" + openimglist[number]), 45, y + 120, 120, 75);
                print.SetFont("MS ゴシック", 13);
                System.IO.StreamReader sr2 = new System.IO.StreamReader(
                    openfilename + "nextyear/" + openfilelist[number],
                    System.Text.Encoding.GetEncoding("utf-8"));
                int j = 0;
                while (sr2.Peek() > -1)
                {
                    print.DrawStringCentering(sr2.ReadLine(), x_center, y + 196 + j * 6);
                    j++;
                }
                sr2.Close();

            };
            printer.PrintPage();
        }
    }
}
