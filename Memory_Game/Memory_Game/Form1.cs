using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class Form1 : Form
    {
        private List<Image> images = new List<Image>();
        private List<PictureBox> pictureBoxes = new List<PictureBox>();
        private Random rnd = new Random();
        private int oyuncu1Puani = 0;
        private int oyuncu2Puani = 0;
        private bool oyuncu1Sirasi = true;
        private PictureBox ilkSecilenResim = null;
        private int acilanResimSayisi = 0;
        private Timer tiklamaZamani = new Timer();
        private Timer ikinciResimSecimSuresi = new Timer();
        public Form1()
        {
            InitializeComponent();

            pictureBoxes.AddRange(new PictureBox[] {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16,
                pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24,
                pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30, pictureBox31, pictureBox32,
                pictureBox33, pictureBox34, pictureBox35, pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40
            });

            KartlariGetirVeKaristir();
            TumResimleriGosterVeKapat();

            tiklamaZamani.Interval = 5000; // 5 saniyelik bekleme süresi
            tiklamaZamani.Tick += ClickTimerTick;


            ikinciResimSecimSuresi.Interval = 5000; 
            ikinciResimSecimSuresi.Tick += IkinciResimSecimSuresi_Tick;
        }

        private void KartlariGetirVeKaristir()
        {
            images = new List<Image>
            {
                Properties.Resources.image0, Properties.Resources.image1, Properties.Resources.image2, Properties.Resources.image3,
                Properties.Resources.image4, Properties.Resources.image5, Properties.Resources.image6, Properties.Resources.image7,
                Properties.Resources.image8, Properties.Resources.image9, Properties.Resources.image10, Properties.Resources.image11,
                Properties.Resources.image12, Properties.Resources.image13, Properties.Resources.image14, Properties.Resources.image15,
                Properties.Resources.image16, Properties.Resources.image17, Properties.Resources.image18, Properties.Resources.image19
            };

            images = images.Concat(images).OrderBy(x => rnd.Next()).ToList();

            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                pictureBoxes[i].Tag = images[i];
                pictureBoxes[i].Image = null;
                pictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxes[i].Click += PictureBox_Click;
            }
        }

        private void TumResimleriGosterVeKapat()
        {
            foreach (var pictureBox in pictureBoxes)
            {
                pictureBox.Image = (Image)pictureBox.Tag;
            }

            tiklamaZamani.Start();
        }

        private void ClickTimerTick(object sender, EventArgs e)
        {
            foreach (var pictureBox in pictureBoxes)
            {
                pictureBox.Image = null;
            }

            oyuncu1Sirasi = true;
            UpdateLabelColors();
            tiklamaZamani.Stop();
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox secilenResim = sender as PictureBox;

            if (secilenResim == null || secilenResim.Image != null || acilanResimSayisi == 2)
                return;

            secilenResim.Image = (Image)secilenResim.Tag;

            if (acilanResimSayisi == 0)
            {
                ilkSecilenResim = secilenResim;
                acilanResimSayisi++;
                ikinciResimSecimSuresi.Start();
            }
            else if (acilanResimSayisi == 1)
            {
                acilanResimSayisi++;
                ikinciResimSecimSuresi.Stop();

                if (ilkSecilenResim.Tag == secilenResim.Tag)
                {
                    if (oyuncu1Sirasi)
                    {
                        oyuncu1Puani++;
                        lblOyuncu1Puan.Text = oyuncu1Puani.ToString();
                    }
                    else
                    {
                        oyuncu2Puani++;
                        lblOyuncu2Puan.Text = oyuncu2Puani.ToString();
                    }

                    ilkSecilenResim = null;
                    acilanResimSayisi = 0;
                    OyunBitisKontrol();
                }
                else
                {
                    Timer timer = new Timer
                    {
                        Interval = 500
                    };
                    
                    timer.Tick += (s, ev) =>
                    {
                        ilkSecilenResim.Image = null;
                        secilenResim.Image = null;
                        ilkSecilenResim = null;
                        acilanResimSayisi = 0;
                        oyuncu1Sirasi = !oyuncu1Sirasi;
                        UpdateLabelColors();
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
        }

            private void IkinciResimSecimSuresi_Tick(object sender, EventArgs e)
            {
                
                if (ilkSecilenResim != null)
                {
                    ilkSecilenResim.Image = null;
                    ilkSecilenResim = null;
                    acilanResimSayisi = 0;
                    oyuncu1Sirasi = !oyuncu1Sirasi;
                    UpdateLabelColors();
                }
                ikinciResimSecimSuresi.Stop();
            }




        

        private void UpdateLabelColors()
        {
            if (oyuncu1Sirasi)
            {
                lblOyuncu1.BackColor = Color.Red;
                lblOyuncu2.BackColor = Color.Transparent;
            }
            else
            {
                lblOyuncu2.BackColor = Color.Red;
                lblOyuncu1.BackColor = Color.Transparent;
            }
        }

        private void OyunBitisKontrol()
        {
            if (oyuncu1Puani >= 11 || oyuncu2Puani >= 11 || oyuncu1Puani + oyuncu2Puani == 20)
            {
                string kazanan = oyuncu1Puani > oyuncu2Puani ? "Oyuncu 1 kazandı!" : "Oyuncu 2 kazandı!";
                MessageBox.Show(kazanan);
                Application.Exit();
            }
        }
    }
}
