using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Fortune {
	/// <summary>
	/// 印刷汎用メソッドクラス
	/// </summary>
	public class Print : IDisposable{
		
		//内部変数
		private Font fFont;							//印刷フォント
		private Pen pPen = new Pen(Color.Black,1);	//デフォルトで幅1のペンを作成
		private bool bIsLandscape = false;
		private PrintPageEventArgs pTarget;

		//用紙サイズはA4固定
//		private const PaperSize SiZE = PaperKind.A4;
		private PaperSize pSize = new PaperSize("A4",210,297);

		//この定数は縦置きの値になっているので使用するときは注意
		private const int PAPER_WIDTH = 210;						//用紙の幅
		private const int PAPER_HEIGHT = 297;						//用紙の高さ
		private const GraphicsUnit SCALE = GraphicsUnit.Millimeter;	//ミリ単位で扱う

#region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="bIsLandscape"></param>
		public Print(PrintPageEventArgs target, bool bIsLandscape) {
			//印刷に関する設定
			this.pTarget = target;
			this.bIsLandscape = bIsLandscape;

			//ページの単位を設定
			this.pTarget.Graphics.PageUnit = SCALE;

			//デフォルトフォントの作成
			fFont = new Font("ＭＳ ゴシック",10);			//デフォルトはゴシック
		}
#endregion

#region Dispose
        public void Dispose(){
			//リソースの廃棄
			pPen.Dispose();
			fFont.Dispose();
		}
#endregion

#region プロパティ
        /// <summary>
        /// ペンの太さ
        /// </summary>
        /// ===============================================================================
		public int PenBold{
			set {
				pPen.Dispose();
				pPen = new Pen(Color.Black,value);
			}
        }
#endregion

        //===============================================================================
		//メソッド
		//===============================================================================

        /// <summary>
        /// 印刷画面のクリア
        /// </summary>
		public void Clear(){
			pTarget.Graphics.Clear(Color.White);	//白で塗りつぶす
		}

        /// <summary>
        /// フォント設定
        /// </summary>
        /// <param name="sFontName">フォントの名前（全角半角を正確に！！）</param>
        /// <param name="flSize">文字のサイズ</param>
		public void SetFont(string sFontName, float flSize){
			fFont.Dispose();						//廃棄して
			fFont = new Font(sFontName,flSize);		//フォント設定
		}

        /// <summary>
        /// フォントのサイズを変更する
        /// </summary>
        /// <param name="flSize">フォントのサイズ</param>
		public void SetFont(float flSize){
			string sFontName = fFont.Name;	//前のフォントを保持
			fFont.Dispose();
			fFont = new Font(sFontName,flSize);
		}

		/// <summary>
		///		ラインの描画
		/// </summary>
		/// <param name="x1">ラインの始点のX座標</param>
		/// <param name="y1">ラインの始点のY座標</param>
		/// <param name="x2">ラインの終点のX座標</param>
		/// <param name="y2">ラインの終点のY座標</param>
		public void DrawLine(int x1, int y1, int x2, int y2){
			//PointF ptPoint = new PointF(x,y);
			pTarget.Graphics.DrawLine(pPen,x1,y1,x2,y2);
		}

		/// <summary>
		/// ボックスの描画
		/// </summary>
		/// <param name="x">始点のX座標</param>
		/// <param name="y">始点のY座標</param>
		/// <param name="width">ボックスの幅</param>
		/// <param name="height">ボックスの高さ</param>
		public void DrawRectangle(int x, int y, int width, int height){
			pTarget.Graphics.DrawRectangle(pPen,x,y,width,height);
		}

		/// <summary>
		///	画像の描画．画像は元の大きさのまま描画されます
		/// </summary>
		/// <param name="imgImage">描画する画像</param>
		/// <param name="x">画像の左上端のX座標</param>
		/// <param name="y">画像の左上端のY座標</param>
		public void DrawImage(Image imgImage, int x, int y){
			pTarget.Graphics.DrawImageUnscaled(imgImage,x,y);
		}

		/// <summary>
		///	画像を指定の大きさで描画します
		/// </summary>
		/// <param name="imgImage">描画する画像</param>
		/// <param name="x">画像の左上端のX座標</param>
		/// <param name="y">画像の左上端のY座標</param>
		/// <param name="width">表示する画像の幅</param>
		/// <param name="height">表示する画像の高さ</param>
		public void DrawImageResize(Image imgImage, int x, int y, int width, int height){
			pTarget.Graphics.DrawImage(imgImage,x,y,width,height);
		}

		/// <summary>
		///	画像を半透明にして描画する
		/// </summary>
		/// <param name="imgImage">描画する画像</param>
		/// <param name="x">画像の左上端のX座標</param>
		/// <param name="y">画像の左上端のY座標</param>
		public void DrawImageTransparent(Image imgImage, int x, int y){
			//アルファ値の変更
			System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();

			//ColorMatrixの行列の値を変更する
			cm.Matrix00 = 1;
			cm.Matrix11 = 1;
			cm.Matrix22 = 1;
			cm.Matrix33 = 0.5F;
			cm.Matrix44 = 1;

			//ImageAttributesオブジェクトの作成
			System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
			ia.SetColorMatrix(cm);

			//画像を描画
			try{
//				pTarget.Graphics.DrawImage(imgImage,0,0,210,297,SCALE,ia);
				pTarget.Graphics.DrawImage(imgImage,new Rectangle(0,0,imgImage.Width,
					imgImage.Height),x,y,imgImage.Width,imgImage.Height,SCALE,ia);
			}
			catch(Exception e){
				Console.WriteLine(e.Message);
			}
		}

		//A4背景画像の描画
		/// <summary>
		///	画像を背景に描画します．
		/// </summary>
		/// <param name="imgImage">描画する画像</param>
		public void DrawBackGround(Image imgImage){
			//縦置きか横置きかを判別
			if (this.bIsLandscape){
				//横置きの場合

                //ちょっといじりますよ


				//画像を拡大or縮小して描画
				this.DrawImageResize(imgImage, 0, 0, PAPER_HEIGHT, PAPER_WIDTH);
			}
			else{
				//縦置きの場合
				//画像を拡大or縮小して描画
				this.DrawImageResize(imgImage, 0, 0, PAPER_WIDTH, PAPER_HEIGHT);
			}
		}

		//文字列描画
		/// <summary>
		///		文字列を描画する
		/// </summary>
		/// <param name="sString">描画する文字列</param>
		/// <param name="x">文字の左上端X座標</param>
		/// <param name="y">文字の左上端Y座標</param>
		/// <param name="color">文字の色</param>
		public void DrawString(string sString, int x, int y,Brush color){
			pTarget.Graphics.DrawString(sString,fFont,color,x,y);
		}
		public void DrawString(string sString, int x, int y){
			pTarget.Graphics.DrawString(sString,fFont,Brushes.Black,x,y);
		}

		//文字列ボックス描画
		/// <summary>
		///		文字列を指定の四角形の中に納まるように描画します
		/// </summary>
		/// <param name="sString">描画する文字列</param>
		/// <param name="x">ボックスの左上端のX座標</param>
		/// <param name="y">ボックスの左上端のY座標</param>
		/// <param name="width">ボックスの幅</param>
		/// <param name="height">ボックスの高さ</param>
		/// <param name="color">文字の色</param>
		public void DrawStringBox(string sString, int x, int y, int width, int height, Brush color){
			//ボックスの作成
			Rectangle rect = new Rectangle(x,y,width,height);

			//ボックスを描画
			//pTarget.Graphics.FillRectangle(Brushes.White,rect);	//ボックス内を真っ白に（必要ない？)

			//文字列を描画
			pTarget.Graphics.DrawString(sString,fFont,color,rect);
		}
		public void DrawStringBox(string sString, int x, int y, int width, int height){
			//ボックスの作成
			Rectangle rect = new Rectangle(x,y,width,height);

			//ボックスを描画
			//pTarget.Graphics.FillRectangle(Brushes.White,rect);	//ボックス内を真っ白に（必要ない？)

			//文字列を描画
			pTarget.Graphics.DrawString(sString,fFont,Brushes.Black,rect);
		}

		//文字列センタリング描画
		/// <summary>
		///		文字列をセンタリングして描画します
		/// </summary>
		/// <param name="sString">描画する文字列</param>
		/// <param name="x">中央位置のX座標</param>
		/// <param name="y">中央位置のY座標</param>
		/// <param name="color">文字の色</param>
		public void DrawStringCentering(string sString, int x, int y, Brush color){
			//描画した時の文字列の長さを計測
			StringFormat sf = new StringFormat();	//フォーマットオブジェクトの作成
			SizeF size = pTarget.Graphics.MeasureString(sString,fFont,PAPER_WIDTH,sf);
			
			//中央からシフトする長さを求める
			int x2 = Convert.ToInt32(x - size.Width / 2);
			this.DrawString(sString,x2,y,color);			//描画する
		}

		public void DrawStringCentering(string sString, int x, int y){
			//描画した時の文字列の長さを計測
			StringFormat sf = new StringFormat();	//フォーマットオブジェクトの作成
			SizeF size = pTarget.Graphics.MeasureString(sString,fFont,PAPER_WIDTH,sf);
			
			//中央からシフトする長さを求める
			int x2 = Convert.ToInt32(x - size.Width / 2);
			this.DrawString(sString,x2,y);			//描画する
		}

		//文字列の白抜き描画
		/// <summary>
		///		文字列を白抜きで描画する
		/// </summary>
		/// <param name="sString">描画する文字列</param>
		/// <param name="x">文字列の左上端のX座標</param>
		/// <param name="y">文字列の左上端のY座標</param>
		public void DrawStringWhiteBackground(string sString, int x, int y){
			System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
			gp.AddString(sString, new FontFamily(fFont.Name),0,fFont.Size,new Point(x,y),StringFormat.GenericDefault);
			
			//白抜きにする
			pTarget.Graphics.FillPath(Brushes.White,gp);

			//描画
			pTarget.Graphics.DrawPath(pPen,gp);
		}

		//文字列の右寄せ描画
		/// <summary>
		///		文字列を右寄せで描画する
		/// </summary>
		/// <param name="sString">描画する文字列</param>
		/// <param name="x">文字列の右上端のX座標</param>
		/// <param name="y">文字列の右上端のY座標</param>
		public void DrawStringRightAlign(string sString, int x, int y,Brush color){
			SizeF size = pTarget.Graphics.MeasureString(sString,fFont);
			float x2 = float.Parse(x.ToString()) - size.Width;
			pTarget.Graphics.DrawString(sString,fFont,color,x2,y);
		}
		public void DrawStringRightAlign(string sString, int x, int y){
			SizeF size = pTarget.Graphics.MeasureString(sString,fFont);
			float x2 = float.Parse(x.ToString()) - size.Width;
			pTarget.Graphics.DrawString(sString,fFont,Brushes.Black,x2,y);
		}

		//文字列ボックスセンタリング描画
		public void DrawStringBoxCentering(string sString, int x, int y){

		}

		//文字列影つき描画	とりあえず実装いらない？

		//文字列影つきセンタリング描画	とりあえず実装いらない？

		//文字列白抜き描画	とりあえず実装いらない？
	}
}
