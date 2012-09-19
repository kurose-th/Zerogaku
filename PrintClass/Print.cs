using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Fortune {
	/// <summary>
	/// ����ėp���\�b�h�N���X
	/// </summary>
	public class Print : IDisposable{
		
		//�����ϐ�
		private Font fFont;							//����t�H���g
		private Pen pPen = new Pen(Color.Black,1);	//�f�t�H���g�ŕ�1�̃y�����쐬
		private bool bIsLandscape = false;
		private PrintPageEventArgs pTarget;

		//�p���T�C�Y��A4�Œ�
//		private const PaperSize SiZE = PaperKind.A4;
		private PaperSize pSize = new PaperSize("A4",210,297);

		//���̒萔�͏c�u���̒l�ɂȂ��Ă���̂Ŏg�p����Ƃ��͒���
		private const int PAPER_WIDTH = 210;						//�p���̕�
		private const int PAPER_HEIGHT = 297;						//�p���̍���
		private const GraphicsUnit SCALE = GraphicsUnit.Millimeter;	//�~���P�ʂň���

#region �R���X�g���N�^
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="bIsLandscape"></param>
		public Print(PrintPageEventArgs target, bool bIsLandscape) {
			//����Ɋւ���ݒ�
			this.pTarget = target;
			this.bIsLandscape = bIsLandscape;

			//�y�[�W�̒P�ʂ�ݒ�
			this.pTarget.Graphics.PageUnit = SCALE;

			//�f�t�H���g�t�H���g�̍쐬
			fFont = new Font("�l�r �S�V�b�N",10);			//�f�t�H���g�̓S�V�b�N
		}
#endregion

#region Dispose
        public void Dispose(){
			//���\�[�X�̔p��
			pPen.Dispose();
			fFont.Dispose();
		}
#endregion

#region �v���p�e�B
        /// <summary>
        /// �y���̑���
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
		//���\�b�h
		//===============================================================================

        /// <summary>
        /// �����ʂ̃N���A
        /// </summary>
		public void Clear(){
			pTarget.Graphics.Clear(Color.White);	//���œh��Ԃ�
		}

        /// <summary>
        /// �t�H���g�ݒ�
        /// </summary>
        /// <param name="sFontName">�t�H���g�̖��O�i�S�p���p�𐳊m�ɁI�I�j</param>
        /// <param name="flSize">�����̃T�C�Y</param>
		public void SetFont(string sFontName, float flSize){
			fFont.Dispose();						//�p������
			fFont = new Font(sFontName,flSize);		//�t�H���g�ݒ�
		}

        /// <summary>
        /// �t�H���g�̃T�C�Y��ύX����
        /// </summary>
        /// <param name="flSize">�t�H���g�̃T�C�Y</param>
		public void SetFont(float flSize){
			string sFontName = fFont.Name;	//�O�̃t�H���g��ێ�
			fFont.Dispose();
			fFont = new Font(sFontName,flSize);
		}

		/// <summary>
		///		���C���̕`��
		/// </summary>
		/// <param name="x1">���C���̎n�_��X���W</param>
		/// <param name="y1">���C���̎n�_��Y���W</param>
		/// <param name="x2">���C���̏I�_��X���W</param>
		/// <param name="y2">���C���̏I�_��Y���W</param>
		public void DrawLine(int x1, int y1, int x2, int y2){
			//PointF ptPoint = new PointF(x,y);
			pTarget.Graphics.DrawLine(pPen,x1,y1,x2,y2);
		}

		/// <summary>
		/// �{�b�N�X�̕`��
		/// </summary>
		/// <param name="x">�n�_��X���W</param>
		/// <param name="y">�n�_��Y���W</param>
		/// <param name="width">�{�b�N�X�̕�</param>
		/// <param name="height">�{�b�N�X�̍���</param>
		public void DrawRectangle(int x, int y, int width, int height){
			pTarget.Graphics.DrawRectangle(pPen,x,y,width,height);
		}

		/// <summary>
		///	�摜�̕`��D�摜�͌��̑傫���̂܂ܕ`�悳��܂�
		/// </summary>
		/// <param name="imgImage">�`�悷��摜</param>
		/// <param name="x">�摜�̍���[��X���W</param>
		/// <param name="y">�摜�̍���[��Y���W</param>
		public void DrawImage(Image imgImage, int x, int y){
			pTarget.Graphics.DrawImageUnscaled(imgImage,x,y);
		}

		/// <summary>
		///	�摜���w��̑傫���ŕ`�悵�܂�
		/// </summary>
		/// <param name="imgImage">�`�悷��摜</param>
		/// <param name="x">�摜�̍���[��X���W</param>
		/// <param name="y">�摜�̍���[��Y���W</param>
		/// <param name="width">�\������摜�̕�</param>
		/// <param name="height">�\������摜�̍���</param>
		public void DrawImageResize(Image imgImage, int x, int y, int width, int height){
			pTarget.Graphics.DrawImage(imgImage,x,y,width,height);
		}

		/// <summary>
		///	�摜�𔼓����ɂ��ĕ`�悷��
		/// </summary>
		/// <param name="imgImage">�`�悷��摜</param>
		/// <param name="x">�摜�̍���[��X���W</param>
		/// <param name="y">�摜�̍���[��Y���W</param>
		public void DrawImageTransparent(Image imgImage, int x, int y){
			//�A���t�@�l�̕ύX
			System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();

			//ColorMatrix�̍s��̒l��ύX����
			cm.Matrix00 = 1;
			cm.Matrix11 = 1;
			cm.Matrix22 = 1;
			cm.Matrix33 = 0.5F;
			cm.Matrix44 = 1;

			//ImageAttributes�I�u�W�F�N�g�̍쐬
			System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
			ia.SetColorMatrix(cm);

			//�摜��`��
			try{
//				pTarget.Graphics.DrawImage(imgImage,0,0,210,297,SCALE,ia);
				pTarget.Graphics.DrawImage(imgImage,new Rectangle(0,0,imgImage.Width,
					imgImage.Height),x,y,imgImage.Width,imgImage.Height,SCALE,ia);
			}
			catch(Exception e){
				Console.WriteLine(e.Message);
			}
		}

		//A4�w�i�摜�̕`��
		/// <summary>
		///	�摜��w�i�ɕ`�悵�܂��D
		/// </summary>
		/// <param name="imgImage">�`�悷��摜</param>
		public void DrawBackGround(Image imgImage){
			//�c�u�������u�����𔻕�
			if (this.bIsLandscape){
				//���u���̏ꍇ

                //������Ƃ�����܂���


				//�摜���g��or�k�����ĕ`��
				this.DrawImageResize(imgImage, 0, 0, PAPER_HEIGHT, PAPER_WIDTH);
			}
			else{
				//�c�u���̏ꍇ
				//�摜���g��or�k�����ĕ`��
				this.DrawImageResize(imgImage, 0, 0, PAPER_WIDTH, PAPER_HEIGHT);
			}
		}

		//������`��
		/// <summary>
		///		�������`�悷��
		/// </summary>
		/// <param name="sString">�`�悷�镶����</param>
		/// <param name="x">�����̍���[X���W</param>
		/// <param name="y">�����̍���[Y���W</param>
		/// <param name="color">�����̐F</param>
		public void DrawString(string sString, int x, int y,Brush color){
			pTarget.Graphics.DrawString(sString,fFont,color,x,y);
		}
		public void DrawString(string sString, int x, int y){
			pTarget.Graphics.DrawString(sString,fFont,Brushes.Black,x,y);
		}

		//������{�b�N�X�`��
		/// <summary>
		///		��������w��̎l�p�`�̒��ɔ[�܂�悤�ɕ`�悵�܂�
		/// </summary>
		/// <param name="sString">�`�悷�镶����</param>
		/// <param name="x">�{�b�N�X�̍���[��X���W</param>
		/// <param name="y">�{�b�N�X�̍���[��Y���W</param>
		/// <param name="width">�{�b�N�X�̕�</param>
		/// <param name="height">�{�b�N�X�̍���</param>
		/// <param name="color">�����̐F</param>
		public void DrawStringBox(string sString, int x, int y, int width, int height, Brush color){
			//�{�b�N�X�̍쐬
			Rectangle rect = new Rectangle(x,y,width,height);

			//�{�b�N�X��`��
			//pTarget.Graphics.FillRectangle(Brushes.White,rect);	//�{�b�N�X����^�����Ɂi�K�v�Ȃ��H)

			//�������`��
			pTarget.Graphics.DrawString(sString,fFont,color,rect);
		}
		public void DrawStringBox(string sString, int x, int y, int width, int height){
			//�{�b�N�X�̍쐬
			Rectangle rect = new Rectangle(x,y,width,height);

			//�{�b�N�X��`��
			//pTarget.Graphics.FillRectangle(Brushes.White,rect);	//�{�b�N�X����^�����Ɂi�K�v�Ȃ��H)

			//�������`��
			pTarget.Graphics.DrawString(sString,fFont,Brushes.Black,rect);
		}

		//������Z���^�����O�`��
		/// <summary>
		///		��������Z���^�����O���ĕ`�悵�܂�
		/// </summary>
		/// <param name="sString">�`�悷�镶����</param>
		/// <param name="x">�����ʒu��X���W</param>
		/// <param name="y">�����ʒu��Y���W</param>
		/// <param name="color">�����̐F</param>
		public void DrawStringCentering(string sString, int x, int y, Brush color){
			//�`�悵�����̕�����̒������v��
			StringFormat sf = new StringFormat();	//�t�H�[�}�b�g�I�u�W�F�N�g�̍쐬
			SizeF size = pTarget.Graphics.MeasureString(sString,fFont,PAPER_WIDTH,sf);
			
			//��������V�t�g���钷�������߂�
			int x2 = Convert.ToInt32(x - size.Width / 2);
			this.DrawString(sString,x2,y,color);			//�`�悷��
		}

		public void DrawStringCentering(string sString, int x, int y){
			//�`�悵�����̕�����̒������v��
			StringFormat sf = new StringFormat();	//�t�H�[�}�b�g�I�u�W�F�N�g�̍쐬
			SizeF size = pTarget.Graphics.MeasureString(sString,fFont,PAPER_WIDTH,sf);
			
			//��������V�t�g���钷�������߂�
			int x2 = Convert.ToInt32(x - size.Width / 2);
			this.DrawString(sString,x2,y);			//�`�悷��
		}

		//������̔������`��
		/// <summary>
		///		������𔒔����ŕ`�悷��
		/// </summary>
		/// <param name="sString">�`�悷�镶����</param>
		/// <param name="x">������̍���[��X���W</param>
		/// <param name="y">������̍���[��Y���W</param>
		public void DrawStringWhiteBackground(string sString, int x, int y){
			System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
			gp.AddString(sString, new FontFamily(fFont.Name),0,fFont.Size,new Point(x,y),StringFormat.GenericDefault);
			
			//�������ɂ���
			pTarget.Graphics.FillPath(Brushes.White,gp);

			//�`��
			pTarget.Graphics.DrawPath(pPen,gp);
		}

		//������̉E�񂹕`��
		/// <summary>
		///		��������E�񂹂ŕ`�悷��
		/// </summary>
		/// <param name="sString">�`�悷�镶����</param>
		/// <param name="x">������̉E��[��X���W</param>
		/// <param name="y">������̉E��[��Y���W</param>
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

		//������{�b�N�X�Z���^�����O�`��
		public void DrawStringBoxCentering(string sString, int x, int y){

		}

		//������e���`��	�Ƃ肠������������Ȃ��H

		//������e���Z���^�����O�`��	�Ƃ肠������������Ȃ��H

		//�����񔒔����`��	�Ƃ肠������������Ȃ��H
	}
}
