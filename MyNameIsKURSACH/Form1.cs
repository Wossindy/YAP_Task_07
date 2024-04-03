using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNameIsKURSACH
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        #region Константы с адрессами изображений
        //Выглядит не красиво, нужно проверить нет ли привязки к абсолютным путям
        const string Idesk = @"desk.png";
        const string IdeskSEL = @"deskSEL.png";
        const string Igrass = @"grass.png";
        const string IgrassSEL = @"grassSEL.png";
        const string ICHdesk = @"CHdesk.png";
        const string ICHdeskSEL = @"CHdeskSEL.png";
        const string ICHgrass = @"CHgrass.png";
        const string ICHgrassSEL = @"CHgrassSEL.png";
        const string IFdesk = @"Foxdesk.png";
        const string IFdeskSEL = @"FoxdeskSEL.png";
        const string IFgrass = @"Foxgrass.png";
        const string IFgrassSEL = @"FoxgrassSEL.png";
        #endregion    
        int ChiCount = 20;
        public Fields desk;//Коллекция игровых объектов
        public GameObj SelectedCh = null;
        public int ID;
        public Size scale= new System.Drawing.Size(50, 50);// 25 25 - стандартные размеры, скалирование допускается, но нужно ли?
        public Form1()
        {
             InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e) //Генерация игровых объектов
        {
            ChiCount = 20;
            this.label1.Text = "Куриц осталось: " + ChiCount;
            this.label1.ForeColor = Color.Black;
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.menuStrip1.BackgroundImage = new System.Drawing.Bitmap(Igrass);
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.помощьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 32);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripTextBox1.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripTextBox1.ForeColor = System.Drawing.Color.Black;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripTextBox1.Size = new System.Drawing.Size(126, 28);
            this.toolStripTextBox1.Text = "Новая игра";
            this.toolStripTextBox1.Click += new System.EventHandler(this.toolStripTextBox1_Click);
            this.toolStripTextBox1.MouseLeave += new System.EventHandler(this.toolStripTextBox1_MouseLeave);
            this.toolStripTextBox1.MouseHover += new System.EventHandler(this.toolStripTextBox1_MouseHover);
            menuStrip1.Items.Add(toolStripTextBox1);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 15F);
            this.помощьToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(97, 28);
            this.помощьToolStripMenuItem.Text = "Помощь";
            this.помощьToolStripMenuItem.Click += new System.EventHandler(this.помощьToolStripMenuItem_Click);
            this.помощьToolStripMenuItem.MouseLeave += new System.EventHandler(this.toolStripTextBox1_MouseLeave);
            this.помощьToolStripMenuItem.MouseHover += new System.EventHandler(this.toolStripTextBox1_MouseHover);
            menuStrip1.Items.Add(помощьToolStripMenuItem);
            //


            this.Controls.Add(menuStrip1);

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = "2ве лисы и 20 кур";
            //создние игровых полей, кур, лис 
            desk = new Fields(Idesk, IdeskSEL, Igrass, IgrassSEL, ICHdesk, ICHdeskSEL, ICHgrass, ICHgrassSEL, IFdesk, IFdeskSEL, IFgrass, IFgrassSEL, scale, 75, 50);
            Controls.AddRange(desk.F.ToArray());
            for (int i = 0; i < 33; i++)
            {
                desk[i].SendToBack();
                desk[i].MouseHover += new System.EventHandler(this.MousHover);
                desk[i].MouseLeave += new System.EventHandler(this.NotMousHover);
                desk[i].Click += new System.EventHandler(this.FiClick);
            }
            for (int i = 33; i < 55; i++)
            {
                
                desk[i].BringToFront();
                desk[i].MouseHover += new System.EventHandler(this.MousHover);
                desk[i].MouseLeave += new System.EventHandler(this.NotMousHover);
                desk[i].Click += new System.EventHandler(this.FiClick);
            }
        }
        #region выделения при наведении мышкой
        private void MousHover(object sender, EventArgs e)//Мышь наведена
        {
             ((GameObj)sender).SelMy(desk);
        }
        private void NotMousHover(object sender, EventArgs e)//Мышь отведена
        {
            ((GameObj)sender).NotSelMy(desk);
        }
        #endregion
        private void FiClick(object sender, EventArgs e)//Обработка нажатия на клетку
        {
            ((GameObj)sender).toClik(ref desk, ref SelectedCh);
            //перерисовка при удалении куриц
            GameObj[] A = new GameObj[Controls.Count-2];
            if (desk.F.Count != Controls.Count - 2)
            {
                int j = 0;
                for (int i = 0; i < Controls.Count; i++)
                {
                    try
                    {
                        A[j] = (GameObj)Controls[i];
                        j++;
                    }
                    catch { }
                }
            List<GameObj> B = A.ToList();
            
                foreach (GameObj D in desk.F)
                {
                    B.Remove(D);
                }
                foreach (GameObj D in B)
                {
                    Controls.Remove(D);
                }
                ChiCount = ChiCount - B.Count;
                this.label1.Text = "Куриц осталось: " + ChiCount;
                if(ChiCount < 13)this.label1.ForeColor = Color.Red;
            }
            //проверка условий победы/поражения
            var TryFail = from win in desk.F where (win.ID > 32 && win.ID < 53) select win;
            var TryWin = from win in desk.F where (win.Loc.Y < 3 && win.Loc.X > 1 && win.Loc.X < 5 && (win.ID > 32 && win.ID < 53)) select win;
            if (TryWin.Count() == 9)
            {
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"You_have_won_the_match.wav");
                simpleSound.Play();
                if (desk.F.Count == 55)
                {
                    simpleSound = new System.Media.SoundPlayer(@"Flawless_victory.wav");
                    simpleSound.Play();
                }
                DialogResult status = MessageBox.Show("Вы победили!", "Победа!", MessageBoxButtons.OK);
                simpleSound = new System.Media.SoundPlayer(@"NewRoundIn.wav");
                simpleSound.Play();
                EventArgs ee = new EventArgs();
                desk = new Fields();
                Controls.Clear();
                this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                this.toolStripTextBox1 = new System.Windows.Forms.ToolStripMenuItem();
                this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.Form1_Load(null, ee);
                menuStrip1.Items.Add(помощьToolStripMenuItem);
                
                Controls.Add(label1);
            }
            else if (TryFail.Count() < 9)
            {
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"You_have_lost_the_match.wav");
                simpleSound.Play();
                DialogResult status = MessageBox.Show("Вы проиграли!", "Поражение!", MessageBoxButtons.OK);
                simpleSound = new System.Media.SoundPlayer(@"NewRoundIn.wav");
                simpleSound.Play();
                EventArgs ee = new EventArgs();
                desk = new Fields();
                Controls.Clear();
                this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                this.toolStripTextBox1 = new System.Windows.Forms.ToolStripMenuItem();
                this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.Form1_Load(null, ee);
                menuStrip1.Items.Add(помощьToolStripMenuItem);
                Controls.Add(label1);
            }
        }
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            DialogResult status = MessageBox.Show("Вы уверены?", "Новая игра", MessageBoxButtons.YesNo);
            if (status == DialogResult.Yes)
            {
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"NewRoundIn.wav");
                simpleSound.Play();
                EventArgs ee = new EventArgs();
                desk = new Fields();
                Controls.Clear();
                this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                this.toolStripTextBox1 = new System.Windows.Forms.ToolStripMenuItem();
                this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.Form1_Load(null, ee);
                menuStrip1.Items.Add(помощьToolStripMenuItem);
                Controls.Add(label1);
            }
        }
        private void toolStripTextBox1_MouseHover(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = Color.Black;
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult status = MessageBox.Show("||----------УСЛОВИЯ ПОБЕДЫ И ПОРАЖЕНИЯ:\n||->Для победы необходимо провести 9 куриц в курятник (клетки с досками).\n||->Вы проиграете если у вас останется меньше 9ти куриц.\n||----------ПЕРЕМЕЩЕНИЕ:\n||->Курицы ходят на одну клетку вверх, вправо и влево.\n||->Лисы ходят на одну клетку вверх, вниз, вправо или влево.\n||----------КАК ЕДЯТ КУРИЦ:\n||->Лиса может съесть курицу если она находится к ней в плотную, \n||и за курицей есть пустая клетка.\n||->При поедении курицы, лиса переходит на клетку за ней.\n||->Лиса может выполнить цепь поеданий подряд.\n||->Лиса обязана есть, если может.\n||----------------------------------------------------\n||->Разработал Юрий Жомов, ТюмГУ 2017г", "ПРАВИЛА ИГРЫ", MessageBoxButtons.OK); 
        }

        private void toolStripTextBox1_MouseLeave(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).ForeColor = Color.Black;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult status = MessageBox.Show("Вы уверены?", "Выход", MessageBoxButtons.YesNo);
            if(status == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
