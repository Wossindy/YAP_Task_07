using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyNameIsKURSACH
{
    public abstract class GameObj : PictureBox
    {
        //Игровые координаты
        private System.Drawing.Point loca;
        public System.Drawing.Point Loc
        {
            get { return loca; }
            set { loca = value; }
        }
        //Парамет указывающий выбрана ли, упрощает поиск выбранной (для избегания перебора)
        private bool selected = false;
        public bool Sell
        {
            get { return selected; }
            set { selected = value; }
        }
        //Порядковый номер 
        int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        //Методы для игровых объектов
        public abstract void SelMy(Fields desk);
        public abstract void NotSelMy(Fields desk);
        public abstract void toClik(ref Fields desk,ref GameObj SelectedCh);
        public abstract int myPower(List<GameObj> desk);
        public abstract void Step(Fields desk, ref List<GameObj> F, ref bool CanMove, bool first = true);
    }
}
