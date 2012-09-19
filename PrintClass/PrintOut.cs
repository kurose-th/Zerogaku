using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Fortune {
	/// <summary>
	/// 印刷出力クラス
	/// </summary>
	public class PrintOut {
#region	内部変数 
        //===============================================================================
		private PrintDocument pd;				//印刷のドキュメント
		private bool bIsLandscape = false;		//用紙方向，横にするかどうか
		private bool bIsPreview = true;			//プレビューを行うかどうか
        //===============================================================================
#endregion
        
#region イベント
		//===============================================================================
		public delegate void dlgEventHandler(object sender, PrintOutEventArgs e);
		public event dlgEventHandler evPrint;			//接続要求ありイベント
        //===============================================================================
#endregion

        //コンストラクタ
		public PrintOut() {
			pd = new PrintDocument();
            pd.PrintPage += delegate(object sender, PrintPageEventArgs e) {
                PrintOutEventArgs poea = new PrintOutEventArgs(e, this.bIsLandscape);
                evPrint(this, poea);							//イベントを起こす
            };
            pd.QueryPageSettings += delegate(object sender, QueryPageSettingsEventArgs e) {
                e.PageSettings.Landscape = bIsLandscape;	//印刷方向の設定
            };

			//余白をなしにする
			pd.DefaultPageSettings.Margins.Top = 0;
			pd.DefaultPageSettings.Margins.Bottom = 0;
			pd.DefaultPageSettings.Margins.Left = 0;
			pd.DefaultPageSettings.Margins.Right = 0;
		}

		//===============================================================================
		//プロパティ
		//===============================================================================
		
        /// <summary>
        /// 用紙方向
        /// true:横方向 false:縦方向
        /// </summary>
        public bool LandScape { set { this.bIsLandscape = value; } }

        /// <summary>
        /// 印刷時にプレビュー画面を表示するか
        /// true:プレビューを表示する false:プレビューを表示しない
        /// </summary>
        public bool IsPreview { set { this.bIsPreview = value; } }

		//===============================================================================
		//メソッド
		//===============================================================================
		/// <summary>
		///	印刷を出力する
		/// </summary>
		public void PrintPage(){
			if (bIsPreview){
				PrintPreviewDialog ppd = new PrintPreviewDialog();
				ppd.Document = pd;		//ドキュメントの設定
				ppd.ShowDialog();		//プレビュー画面を出す
			}
			else{
				pd.Print();
			}
		}
	}

    /// <summary>
    /// イベント引数のクラス
    /// </summary>
	public class PrintOutEventArgs : EventArgs{
        /// <summary>
        /// 複雑な処理はこれを使用して.Netのメソッド直接たたいてね
        /// </summary>
		public PrintPageEventArgs ppe;

        /// <summary>
        /// 楽したい人にお勧め、これを介して印刷を描画するといろいろ楽
        /// </summary>
		public Print print;

		public PrintOutEventArgs(PrintPageEventArgs e,bool landscape){
			this.ppe = e;						//引数の設定
			this.print = new Print(e,landscape);			//メソッドクラスの生成
		}
	}
}
