using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace MyNameIsKURSACH 
{
    public class Field : GameObj
    {
        public Field(System.Drawing.Point L/*Координата на форме*/, System.Drawing.Point l/*Игровая координата*/, System.Drawing.Size S/*Размер*/, string I/*Путь к файлу изображения*/, int id)
        {
            SizeMode = PictureBoxSizeMode.StretchImage;
            Location = L;
            Loc = l;
            Size = S;
            Image = new System.Drawing.Bitmap(I);
            ImageLocation = I;
            Visible = true;
            ID = id;
        }
        //Выделение при наведении мышки
        public override void SelMy(Fields desk)
        {
            var se = from t in desk.F where t.Sell == true select t.Loc;
            if (se.Count() > 0)
            {
                if (((se.First().X - Loc.X == 1 || se.First().X - Loc.X == -1) && (se.First().Y == Loc.Y)) || ((se.First().X == Loc.X) && (se.First().Y - Loc.Y == 1)))
                {
                    if (ImageLocation == desk.Desk) { Image = new System.Drawing.Bitmap(desk.DeskSEL); ImageLocation = desk.DeskSEL; }
                    else if (ImageLocation == desk.Grass) { Image = new System.Drawing.Bitmap(desk.GrassSEL); ImageLocation = desk.GrassSEL; }
                }
            }
            else
            {
                if (ImageLocation == desk.Desk) { Image = new System.Drawing.Bitmap(desk.DeskSEL); ImageLocation = desk.DeskSEL; }
                else if (ImageLocation == desk.Grass) { Image = new System.Drawing.Bitmap(desk.GrassSEL); ImageLocation = desk.GrassSEL; }
            }  
        }
        //Снятие выделения при наведении мышки
        public override void NotSelMy(Fields desk)
        {
            if (ImageLocation == desk.DeskSEL) { Image = new System.Drawing.Bitmap(desk.Desk); ImageLocation = desk.Desk; }
            else if (ImageLocation == desk.GrassSEL) { Image = new System.Drawing.Bitmap(desk.Grass); ImageLocation = desk.Grass; }
        }
        public override void toClik(ref Fields desk, ref GameObj SelectedCh)
        {
            if (SelectedCh != null)//если курица выбранна
            {
                Point SelectedField = SelectedCh.Loc;//получаем её игровые координаты
                if (((Loc.X == SelectedField.X) && (Loc.Y - SelectedField.Y == -1)) || (Math.Abs(Loc.X - SelectedField.X) == 1 && Loc.Y == SelectedField.Y))
                { 
                    if (desk.F[desk.F.IndexOf(SelectedCh)].Sell == true)
                    {
                        desk.F[desk.F.IndexOf(SelectedCh)].Sell = false;
                        if (ImageLocation == desk.DeskSEL || ImageLocation == desk.Desk)
                        {
                            desk.F[desk.F.IndexOf(SelectedCh)].Image = new System.Drawing.Bitmap(desk.chDesk);
                            desk.F[desk.F.IndexOf(SelectedCh)].ImageLocation = desk.chDesk;
                        }
                        else
                        {
                            desk.F[desk.F.IndexOf(SelectedCh)].Image = new System.Drawing.Bitmap(desk.chGrass);
                            desk.F[desk.F.IndexOf(SelectedCh)].ImageLocation = desk.chGrass;
                        }
                        desk.F[desk.F.IndexOf(SelectedCh)].Loc = Loc;
                        desk.F[desk.F.IndexOf(SelectedCh)].Location = Location;
                        desk.F[desk.F.IndexOf(SelectedCh)].BringToFront();
                        SelectedCh = null;
                    }
                    #region ИИ лис
                    int BeforeKills = desk.F.Count;

                bool tomove = true; //флаг для нескольких поеданий подряд
                bool first = true; //флаг если поеданий еще не было 
                    //Вычисления силы позиций на данный момент
                    int P1 = desk.F[desk.F.Count - 2].myPower(desk.F);
                    int P2 = desk.F[desk.F.Count - 1].myPower(desk.F);
                if(P1 > P2) //оценка позиий лис
                {
                    while (tomove) //выполнение нескольких или одного поедания
                    {
                        desk.F[desk.F.Count - 2].Step(desk,ref desk.F, ref tomove, first);
                        first = false;
                    }
                }
                else if (P1 < P2)//оценка позиий лис
                    {

                    while (tomove) //выполнение нескольких или одного поедания
                        {
                        desk.F[desk.F.Count - 1].Step(desk, ref desk.F, ref tomove, first);
                        first = false;
                    }
                }
                else //если позиции равносильны
                {
                    Random rand = new Random();
                    int r = rand.Next(0,2);
                    if (r == 1)
                    {
                        while (tomove)
                        {
                            System.Drawing.Point Before = desk.F[desk.F.Count - 2].Loc;
                            desk.F[desk.F.Count - 2].Step(desk,ref desk.F, ref tomove, first);
                                //если выбранная лиса не изменила положения и не ела (то есть не ходила)
                                if (P1 == 0 && desk.F[desk.F.Count - 2].Loc == Before) desk.F[desk.F.Count - 1].Step(desk, ref desk.F, ref tomove, first);
                            first = false;
                        }
                    }
                    else
                    {
                        while (tomove)
                        {
                            System.Drawing.Point Before = desk.F[desk.F.Count - 1].Loc;
                            desk.F[desk.F.Count - 1].Step(desk, ref desk.F, ref tomove, first);
                                //если выбранная лиса не изменила положения и не ела (то есть не ходила)
                                if (P2 == 0 && desk.F[desk.F.Count - 1].Loc == Before) desk.F[desk.F.Count - 2].Step(desk, ref desk.F, ref tomove, first);
                            first = false;
                        }
                    }
                }
                    #region Звук
                    switch (BeforeKills - desk.F.Count)
                    {
                        case 0: break;
                        case 1:
                            {
                                if (BeforeKills == 55)
                                {
                                    System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"firstblood.wav");
                                    simpleSound.Play();
                                }
                                break;
                            }
                        case 2:
                            {
                                  System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"doublekill.wav");
                                   simpleSound.Play();
                                break;
                            }
                        case 3:
                            {
                                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"multikill.wav");
                                simpleSound.Play();
                                break;
                            }
                        case 4:
                            {
                                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"megakill.wav");
                                simpleSound.Play();
                                break;
                            }
                        case 5:
                            {
                                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"ultrakill.wav");
                                simpleSound.Play();
                                break;
                            }
                        case 6:
                            {
                                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"monsterkill.wav");
                                simpleSound.Play();
                                break;
                            }
                        default:
                            {
                                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"rampage.wav");
                                simpleSound.Play();
                                break;
                            }
                    }
                    #endregion
                }
            }
            #endregion
        }
        public override int myPower(List<GameObj> desk) { return 0; }
        public override void Step(Fields desk,ref List<GameObj> F, ref bool CanMove, bool first) { }
    }
}
