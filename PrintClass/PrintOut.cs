using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Fortune {
	/// <summary>
	/// ����o�̓N���X
	/// </summary>
	public class PrintOut {
#region	�����ϐ� 
        //===============================================================================
		private PrintDocument pd;				//����̃h�L�������g
		private bool bIsLandscape = false;		//�p�������C���ɂ��邩�ǂ���
		private bool bIsPreview = true;			//�v���r���[���s�����ǂ���
        //===============================================================================
#endregion
        
#region �C�x���g
		//===============================================================================
		public delegate void dlgEventHandler(object sender, PrintOutEventArgs e);
		public event dlgEventHandler evPrint;			//�ڑ��v������C�x���g
        //===============================================================================
#endregion

        //�R���X�g���N�^
		public PrintOut() {
			pd = new PrintDocument();
            pd.PrintPage += delegate(object sender, PrintPageEventArgs e) {
                PrintOutEventArgs poea = new PrintOutEventArgs(e, this.bIsLandscape);
                evPrint(this, poea);							//�C�x���g���N����
            };
            pd.QueryPageSettings += delegate(object sender, QueryPageSettingsEventArgs e) {
                e.PageSettings.Landscape = bIsLandscape;	//��������̐ݒ�
            };

			//�]�����Ȃ��ɂ���
			pd.DefaultPageSettings.Margins.Top = 0;
			pd.DefaultPageSettings.Margins.Bottom = 0;
			pd.DefaultPageSettings.Margins.Left = 0;
			pd.DefaultPageSettings.Margins.Right = 0;
		}

		//===============================================================================
		//�v���p�e�B
		//===============================================================================
		
        /// <summary>
        /// �p������
        /// true:������ false:�c����
        /// </summary>
        public bool LandScape { set { this.bIsLandscape = value; } }

        /// <summary>
        /// ������Ƀv���r���[��ʂ�\�����邩
        /// true:�v���r���[��\������ false:�v���r���[��\�����Ȃ�
        /// </summary>
        public bool IsPreview { set { this.bIsPreview = value; } }

		//===============================================================================
		//���\�b�h
		//===============================================================================
		/// <summary>
		///	������o�͂���
		/// </summary>
		public void PrintPage(){
			if (bIsPreview){
				PrintPreviewDialog ppd = new PrintPreviewDialog();
				ppd.Document = pd;		//�h�L�������g�̐ݒ�
				ppd.ShowDialog();		//�v���r���[��ʂ��o��
			}
			else{
				pd.Print();
			}
		}
	}

    /// <summary>
    /// �C�x���g�����̃N���X
    /// </summary>
	public class PrintOutEventArgs : EventArgs{
        /// <summary>
        /// ���G�ȏ����͂�����g�p����.Net�̃��\�b�h���ڂ������Ă�
        /// </summary>
		public PrintPageEventArgs ppe;

        /// <summary>
        /// �y�������l�ɂ����߁A�������Ĉ����`�悷��Ƃ��낢��y
        /// </summary>
		public Print print;

		public PrintOutEventArgs(PrintPageEventArgs e,bool landscape){
			this.ppe = e;						//�����̐ݒ�
			this.print = new Print(e,landscape);			//���\�b�h�N���X�̐���
		}
	}
}
