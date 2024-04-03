using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNameIsKURSACH
{
    public class Fox : GameObj
    {
        public Fox(GameObj F/*Привязка к полю*/, string Img/*Адрес текстуры*/,int id)
        {
            SizeMode = PictureBoxSizeMode.StretchImage;
            Location = F.Location;
            Loc = F.Loc;
            Size = F.Size;
            Image = new System.Drawing.Bitmap(Img);
            ImageLocation = Img;
            Visible = true;
            ID = id; 
        }
        public override void toClik(ref Fields desk, ref GameObj SelectedCh)//выделение курицы
        { }
        //Выделение при наведении мышки
        public override void SelMy(Fields desk)
        {
            if (ImageLocation == desk.FDesk) { Image = new System.Drawing.Bitmap(desk.FDeskSEL); ImageLocation = desk.FDeskSEL; }
            else if (ImageLocation == desk.FGrass) { Image = new System.Drawing.Bitmap(desk.FGrassSEL); ImageLocation = desk.FGrassSEL; }
        }
        //Снятие выделения при наведении мышки
        public override void NotSelMy(Fields desk)
        {
            if (ImageLocation == desk.FDeskSEL) { Image = new System.Drawing.Bitmap(desk.FDesk); ImageLocation = desk.FDesk; }
            else if(ImageLocation == desk.FGrassSEL)  { Image = new System.Drawing.Bitmap(desk.FGrass); ImageLocation = desk.FGrass; }
        }
        //Метод оценки хода
        public int HowManyToKill(List<GameObj> G/*Игральная доска*/, System.Drawing.Point SLoc/*Коодинаты клетки для оценки*/, GameObj FirstBlood/*Курица которую придется съесть*/, int Kill = 0/*Кооличеств ранее "убитых" куриц при проведении оценки*/)
        {
            List<GameObj> F = new List<GameObj>();
            F.AddRange(G);
            if (FirstBlood != null) F.Remove(FirstBlood);//Убираем "съеденную" курицу на данном ходу
            Kill++;
            List <int> Kills = new List<int>();//создаем массив из потенциальных "поеданий" для потенциальных ходов
            var ChickensAround = from t in F
                                 where
                ((((t.Loc.X - SLoc.X == 1 || t.Loc.X - SLoc.X == -1) && (t.Loc.Y == SLoc.Y) && (t.ID > 32 && t.ID < 53))) ||
                (((t.Loc.Y - SLoc.Y == 1 || t.Loc.Y - SLoc.Y == -1) && (t.Loc.X == SLoc.X) && (t.ID > 32 && t.ID < 53))))
                                 select t; //Ищем куриц вокруг данной клетки в радиусе 1
            List<GameObj> FieldsNearChickens = new List<GameObj>();
            var FieldsNearChickenss = from t in F
                               where
              ((((t.Loc.X - SLoc.X == 2 || t.Loc.X - SLoc.X == -2) && (t.Loc.Y == SLoc.Y))) ||
              (((t.Loc.Y - SLoc.Y == 2 || t.Loc.Y - SLoc.Y == -2) && (t.Loc.X == SLoc.X)))) && (t.ID < 33)
                               select t; //Ищем пустые клетки вокруг данной в радиусе 2
            FieldsNearChickens.AddRange(FieldsNearChickenss.ToArray());
            var ChickensAround2 = from t in F
                                  where
                 ((((t.Loc.X - SLoc.X == 2 || t.Loc.X - SLoc.X == -2) && (t.Loc.Y == SLoc.Y) && (t.ID > 32))) ||
                 (((t.Loc.Y - SLoc.Y == 2 || t.Loc.Y - SLoc.Y == -2) && (t.Loc.X == SLoc.X) && (t.ID > 32))))
                                  select t; //Ищем куриц и лис вокруг данной клетки в радиусе 2
            for (int i = 0; i < ChickensAround2.Count(); i++) //убираем занятые клетки в радиусе 2
            {
                var bedField = from t in FieldsNearChickens
                                 where (t.Loc == ChickensAround2.ToArray()[i].Loc)
                                 select t;
                if (bedField.Count() > 0) FieldsNearChickens.Remove(bedField.First());
            }
            for (int i = 0; i < ChickensAround.Count(); i++) 
            {
                var AbleToMove = from t in FieldsNearChickens
                                 where ((t.Loc.X - ChickensAround.ToArray()[i].Loc.X == 1 || t.Loc.X - ChickensAround.ToArray()[i].Loc.X == -1) && (t.Loc.Y == ChickensAround.ToArray()[i].Loc.Y)) ||
                                 ((t.Loc.Y - ChickensAround.ToArray()[i].Loc.Y == 1 || t.Loc.Y - ChickensAround.ToArray()[i].Loc.Y == -1) && (t.Loc.X == ChickensAround.ToArray()[i].Loc.X))
                                 select t; //Выбираем для данной курицы пустую клетку за ней из пустых клеток выбранных до этого
                if (AbleToMove.Count() > 0) Kills.Add(HowManyToKill(F, AbleToMove.First().Loc, ChickensAround.ToArray()[i], Kill)+1); //если у данной курицы есть пустая клетка для хода, то вызываем оценучную функцию для нее
            }//повторяем для каждой курицы вокруг данной клетки
            if (Kills.Count > 0) return Kills.Max();//возвращаем наибольшее число вохможных убийств (оценка окончена для данной клетки)
            else return 0;
        }
        //Один шаг (с поеданием или нет)
        public override void Step(Fields desk,ref List<GameObj> F/*Игральная доска*/, ref bool CanMove/*Флаг цепочки поеданий*/, bool first = true/*Первый ли это шаг*/)
        { 
            var ChickensAround = from t in F
                                 where
                ((((t.Loc.X - Loc.X == 1 || t.Loc.X - Loc.X == -1) && (t.Loc.Y == Loc.Y) && (t.ID > 32 && t.ID < 53))) ||
                (((t.Loc.Y - Loc.Y == 1 || t.Loc.Y - Loc.Y == -1) && (t.Loc.X == Loc.X) && (t.ID > 32 && t.ID < 53))))
                                 select t; //Ищем куриц вокруг данной клетки в радиусе 1

            List<GameObj> FieldsNearChickens = new List<GameObj>();
            var FieldsNearChickenss = from t in F
                                      where
                     ((((t.Loc.X - Loc.X == 2 || t.Loc.X - Loc.X == -2) && (t.Loc.Y == Loc.Y))) ||
                     (((t.Loc.Y - Loc.Y == 2 || t.Loc.Y - Loc.Y == -2) && (t.Loc.X == Loc.X)))) && (t.ID < 33)
                                      select t; //Ищем пустые клетки вокруг данной в радиусе 2
            FieldsNearChickens.AddRange(FieldsNearChickenss.ToArray());
            var ChickensAround2 = from t in F
                                  where
                 ((((t.Loc.X - Loc.X == 2 || t.Loc.X - Loc.X == -2) && (t.Loc.Y == Loc.Y) && (t.ID > 32))) ||
                 (((t.Loc.Y - Loc.Y == 2 || t.Loc.Y - Loc.Y == -2) && (t.Loc.X == Loc.X) && (t.ID > 32))))
                                  select t; //Ищем куриц и лис вокруг данной клетки в радиусе 2
            for (int i = 0; i < ChickensAround2.Count(); i++) //убираем занятые клетки в радиусе 2
            {
                var bedField = from t in FieldsNearChickens
                               where (t.Loc == ChickensAround2.ToArray()[i].Loc)
                               select t;
                if (bedField.Count() > 0) FieldsNearChickens.Remove(bedField.First());
            }
                List<int> Kills = new List<int>();//создаем массив из потенциальных "поеданий" для потенциальных ходов
                List<GameObj> MustDie = new List<GameObj>();//массив из куриц которые могут быть съеденны на данном шаге
                List<GameObj> ToMove = new List<GameObj>(); //массив из клеток за курицами куда переметится лиса после поедания
                foreach (GameObj Chicken in ChickensAround)
                {
                    var AbleToMove = from t in FieldsNearChickens
                                     where ((t.Loc.X - Chicken.Loc.X == 1 || t.Loc.X - Chicken.Loc.X == -1) && (t.Loc.Y == Chicken.Loc.Y)) ||
                                     ((t.Loc.Y - Chicken.Loc.Y == 1 || t.Loc.Y - Chicken.Loc.Y == -1) && (t.Loc.X == Chicken.Loc.X))
                                     select t; //Выбираем для данной курицы пустую клетку за ней из пустых клеток выбранных до этого
                    if (AbleToMove.Count() > 0 &&(Chicken.Loc.X != 2 || Chicken.Loc.Y != 0))//если за данной курицей есть пустая клетка вносим данные в листы
                    {
                    MustDie.Add(Chicken);//курицу в лист к возможным для поедания
                    ToMove.Add(AbleToMove.First());//клетку за курицей к возможным клеткам для перемещния лисы
                    Kills.Add(HowManyToKill(F, AbleToMove.First().Loc, Chicken)); // для этого потенцильного шага вызываем функцию оценки 
                    }
                }//повторяем для каждой курицы вокруг данной клетки
            if (Kills.Count() > 0) //если есть шаги с поеданиями
            {
                //выбираме максимальные по силе вариант хода 
                this.Loc = ToMove[Kills.IndexOf(Kills.Max())].Loc; //смена игровых координат
                this.Location = ToMove[Kills.IndexOf(Kills.Max())].Location;//смена реальных
                //смена текстур если необходима 
                if (ToMove[Kills.IndexOf(Kills.Max())].ImageLocation == desk.Desk) { Image = new System.Drawing.Bitmap(desk.FDesk); ImageLocation = desk.FDesk; }
                else if (ToMove[Kills.IndexOf(Kills.Max())].ImageLocation == desk.Grass) { Image = new System.Drawing.Bitmap(desk.FGrass); ImageLocation = desk.FGrass; }
                F.Remove(MustDie[Kills.IndexOf(Kills.Max())]);//удалиние убитой курицы
            }
            else if (ChickensAround.Count() != 4 && first == true) // если лиса не окружена и может ходить, но не может есть, и до этого не было поеданий
            {
                var ChickensAndFoxAround = from t in F
                                     where
                    ((((t.Loc.X - Loc.X == 1 || t.Loc.X - Loc.X == -1) && (t.Loc.Y == Loc.Y) && (t.ID > 32))) ||
                    (((t.Loc.Y - Loc.Y == 1 || t.Loc.Y - Loc.Y == -1) && (t.Loc.X == Loc.X) && (t.ID > 32))))
                                     select t;//ищем куриц и лис вокруг
                var FieldsAroundd = from t in F
                                   where
                  ((((t.Loc.X - Loc.X == 1 || t.Loc.X - Loc.X == -1) && (t.Loc.Y == Loc.Y) && (t.ID > -1 && t.ID < 33))) ||
                  (((t.Loc.Y - Loc.Y == 1 || t.Loc.Y - Loc.Y == -1) && (t.Loc.X == Loc.X) && (t.ID > -1 && t.ID < 33))))
                                   select t; //Ищем пустые клетки
                List <GameObj> FieldsAround= new List<GameObj>();
                FieldsAround.AddRange(FieldsAroundd.ToArray());
                for (int i = 0; i < ChickensAndFoxAround.Count(); i++)
                {
                    var bedField = from t in FieldsAround
                                   where (t.Loc == ChickensAndFoxAround.ToArray()[i].Loc)
                                   select t;
                    if (bedField.Count() > 0) FieldsAround.Remove(bedField.First());
                }//убираем занятые клетки
                if (FieldsAround.Count > 0) //если есть не занятые клетки - совершаем ход
                {
                    Random rand = new Random();
                    int r = rand.Next(0,FieldsAround.Count());
                    Loc = FieldsAround.ToArray()[r].Loc;
                    Location = FieldsAround.ToArray()[r].Location;
                    if (FieldsAround.ToArray()[r].ImageLocation == desk.Desk) { Image = new System.Drawing.Bitmap(desk.FDesk); ImageLocation = desk.FDesk; }
                    else if (FieldsAround.ToArray()[r].ImageLocation == desk.Grass) { Image = new System.Drawing.Bitmap(desk.FGrass); ImageLocation = desk.FGrass; }
                }
                CanMove = false;//ход окончен
            }
            //если лиса полностью окружена и не может сходить
            else CanMove = false;//ход окончен
        }
        public override int myPower(List<GameObj> desk)//вызов функции оценки для текущей позиции/
        {
            return HowManyToKill(desk, Loc, null);//вызов функции оценки для текущей позиции
        }
    }
}
