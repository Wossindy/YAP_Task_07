using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace MyNameIsKURSACH
{
    public class Fields
    {
        public List<GameObj> F;
        public GameObj this[int i]
        {
            get { return F[i]; }
            set { F[i] = value; }
        }
        public IEnumerator<GameObj> GetEnumerator() { return F.GetEnumerator(); }
        //Поля для адресов тектстур 
        string grass;
        string desk;
        string deskSEL;
        string grassSEL;
        public string Grass
        { get { return grass; } set { grass = value; } }
        public string GrassSEL
        { get { return grassSEL; } set { grassSEL = value; } }
        public string Desk
        { get { return desk; } set { desk = value; } }
        public string DeskSEL
        { get { return deskSEL; } set { deskSEL = value; } }
        string chgrass;
        string chdesk;
        string chdeskSEL;
        string chgrassSEL;
        public string chGrass
        { get { return chgrass; } set { chgrass = value; } }
        public string chGrassSEL
        { get { return chgrassSEL; } set { chgrassSEL = value; } }
        public string chDesk
        { get { return chdesk; } set { chdesk = value; } }
        public string chDeskSEL
        { get { return chdeskSEL; } set { chdeskSEL = value; } }
        string Fgrass;
        string Fdesk;
        string FdeskSEL;
        string FgrassSEL;
        public string FGrass
        { get { return Fgrass; } set { Fgrass = value; } }
        public string FGrassSEL
        { get { return FgrassSEL; } set { FgrassSEL = value; } }
        public string FDesk
        { get { return Fdesk; } set { Fdesk = value; } }
        public string FDeskSEL
        { get { return FdeskSEL; } set { FdeskSEL = value; } }
        public Fields() { }
        public Fields(string DeskImg, string DeskSELImg, string GrassImg, string GrassSELImg, string chDeskImg, string chDeskSELImg, string chGrassImg, string chGrassSELImg, string FDeskImg, string FDeskSELImg, string FGrassImg, string FGrassSELImg/*Адреса текстур*/, System.Drawing.Size S/*Размеры полей*/, int StartX = 0, int StartY = 0/*Начальные координаты*/)
        {
            F = new List<GameObj>();//Выделяем память для хранения данных полей
            Grass = GrassImg;
            GrassSEL = GrassSELImg;
            Desk = DeskImg;
            //Добавляем куриц
            chGrass = chGrassImg;
            chGrassSEL = chGrassSELImg;
            chDesk = chDeskImg;
            chDeskSEL = chDeskSELImg;
            //Добавляем лис
            FGrass = FGrassImg;
            FGrassSEL = FGrassSELImg;
            FDesk = FDeskImg;
            FDeskSEL = FDeskSELImg;
            int i = 0;
            DeskSEL = DeskSELImg;//запоминает текстуры
            int X = StartX + S.Width*2;//Создаем точки для начала отрисовки (Y не используется, за ненадобностью)
            System.Drawing.Point L = new System.Drawing.Point(X, StartY);//Координата на форме
            System.Drawing.Point l = new System.Drawing.Point(2,0);//Игровая координата
            for (i = 0; i < 55; i++)//Задача парметров для полей игрового поля в соответствии с заданием курсовой работы
            {
                if (i < 6)
                {
                    if (i == 3) { L.Y += S.Height; L.X = X; l.Y += 1; l.X = 2;}
                    else if (i != 0)
                    {
                        L.X += S.Width;
                        l.X += 1;
                    }
                    F.Add(new Field(L, l, S, Desk,i));  
                }
                else if (i > 5 && i < 27)
                {
                    if (i == 6 || i == 13 || i == 20) { L.Y += S.Height; L.X = X-S.Width*2; l.Y += 1; l.X = 0; }
                    else
                    {
                        L.X += S.Width;
                        l.X += 1;
                    }
                    if(i > 7 && i <11) F.Add(new Field(L, l, S, Desk,i));
                    else F.Add(new Field(L, l, S, Grass,i));
                }
                else if(i > 26 && i < 33)
                {
                    if (i == 27 || i == 30) { L.Y += S.Height; L.X = X; l.Y += 1; l.X = 2; }
                    else
                    {
                        L.X += S.Width;
                        l.X += 1;
                    }
                    F.Add(new Field(L, l, S, Grass,i));
                }
                else if (i > 32 && i < 53){ F.Add(new Chicken(F[i-20], chGrass, i)); } 
                else { if (i == 53) F.Add(new Fox(F[8], FDesk,i));
                else F.Add(new Fox(F[10], FDesk,i));}
            }
        }      
    }
}
