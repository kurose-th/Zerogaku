﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace uranai
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Zero zerogaku = new Zero();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            zerogaku.printOut(args);
        }
    }
}
