using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MyNameIsKURSACH
{
    public class Chicken : GameObj
    {   
        
        public Chicken(GameObj F/*Привязка к полю*/, string Img/*Адрес текстуры*/, int i/*Порядковй номер*/)
        {
            SizeMode = PictureBoxSizeMode.StretchImage;
            Location = F.Location;
            Loc = F.Loc;
            Size = F.Size;
            Image = new System.Drawing.Bitmap(Img);
            ImageLocation = Img;
            Visible = true;
            ID = i;
            Sell = false;
        }
        //Выделение при наведении мышки
        public override void SelMy(Fields desk)
        {
            if (Sell == false) 
            {
                if (ImageLocation == desk.chDesk) { Image = new System.Drawing.Bitmap(desk.chDeskSEL); ImageLocation = desk.chDeskSEL; }
                else if (ImageLocation == desk.chGrass) { Image = new System.Drawing.Bitmap(desk.chGrassSEL); ImageLocation = desk.chGrassSEL; }
            }
        }
        //Снятие выделения при наведении мышки
        public override void NotSelMy(Fields desk)
        {
            if (Sell == false)
            {
                if (ImageLocation == desk.chDeskSEL) { Image = new System.Drawing.Bitmap(desk.chDesk); ImageLocation = desk.chDesk; }
                else if (ImageLocation == desk.chGrassSEL) { Image = new System.Drawing.Bitmap(desk.chGrass); ImageLocation = desk.chGrass; }
            }
        }
        public override void toClik(ref Fields desk, ref GameObj SelectedCh)//выделение курицы
        {
            if (this.ImageLocation == desk.chDeskSEL || this.ImageLocation == desk.chGrassSEL)
            {
                if (SelectedCh != null && SelectedCh.ID != ID) //если была выделенна другая
                {
                    SelectedCh.Sell = false;
                    SelectedCh.NotSelMy(desk);
                }
                if (Sell)//если выделенна была эта
                {
                    Sell = false;
                    SelectedCh = null;
                }
                else//если не была 
                {
                    Sell = true;
                    SelectedCh = this;
                }
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"Cud.wav");
                simpleSound.Play();
            }
        }
        public override int myPower(List<GameObj> desk) { return 0; }
        public override void Step(Fields desk,ref List<GameObj> F, ref bool CanMove, bool first) { }
    }
}
