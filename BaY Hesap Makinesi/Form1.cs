/*
 * BaY Hesap Makinesi
 * Copyright © 2023 Berkay Bakacak
 * Bu programın kaynak kodu, Genel Kamu Lisansı'nın (GPL) koşulları altında lisanslanmıştır ve
 * kullanıcılar, bu koşullar çerçevesinde yeniden dağıtabilir, değiştirebilir veya kullanabilirler.
 * 
 * 2.0 sürümü güncelleme notları:
 * - Yayın tarihi: 18.04.2023
 * - Hafıza düğmeleri eklendi.
 * - Klavyeyle kontroldeki hatalar giderildi.
 * - Yüzde işlemi eklendi.
 * - "Sıfıra bölünemez" metninin işlem sonuç kutusundan taşması hatası giderildi.
 */

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaY_Hesap_Makinesi
{
    public partial class Form1 : Form
    {
        string programIsim = "BaY Hesap Makinesi";
        string programSurum = "2.0";
        string programYazar = "Berkay Bakacak";
        string programBildiri = "Copyright © 2023 Bu program GPL v3 ile lisanlanmıştır. Bu lisans koşulları çerçevesinde ücretsiz olarak kulanılabilir, dağıtılabilir ve düzenlenebilir.";

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int sol, int ust, int sag, int alt, int elipsG, int elipsY);

        double sayi1, sayi2, sonuc, hafizadakiDeger;
        bool silinecek = true, sayi1Atali = false, kilitli = false;
        string sonislem = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int radiusOran = 2;

            foreach (Control c in Controls)
                if (c is System.Windows.Forms.Button)
                    c.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, c.Width, c.Height, radiusOran, radiusOran));
        }

        private void sayidugme_MouseClick(object sender, MouseEventArgs e)
        {
            if (kilitli)
            {
                temizle_MouseClick(sender, null);
                return;
            }
            System.Windows.Forms.Button dugme = (System.Windows.Forms.Button)sender;

            if (silinecek)
            {
                islemSonuc.Text = "";
                silinecek = false;
            }

            islemSonuc.Text += dugme.Text;
        }

        private void teklisilme_MouseClick(object sender, MouseEventArgs e)
        {
            if (islemSonuc.Text.Length == 1)
            {
                islemSonuc.Text = "0";
                silinecek = true;
            }

            else
                islemSonuc.Text = islemSonuc.Text.Substring(0, islemSonuc.Text.Length - 1);
        }

        private void temizle_MouseClick(object sender, MouseEventArgs e)
        {
            label5.Text = "";
            islemSonuc.Text = "0";
            silinecek = true;
            sayi1Atali = false;
            sayi1 = sayi2 = sonuc = 0;
            sonislem = "";
            kilitAc();
        }


        private void girdiyitemizle_MouseClick(object sender, MouseEventArgs e)
        {
            if (!sayi1Atali)
                temizle_MouseClick(sender, null);

            else
            {
                islemSonuc.Text = "0";
                silinecek = true;
            }
        }

        private void virgul_MouseClick(object sender, MouseEventArgs e)
        {
            if (!islemSonuc.Text.Contains(","))
                islemSonuc.Text += ",";
        }

        private void islemDugme_MouseClick(object sender, MouseEventArgs e)
        {
            silinecek = true;
            System.Windows.Forms.Button islem = (System.Windows.Forms.Button)sender;

            if (!sayi1Atali)
            {
                sayi1 = Convert.ToDouble(islemSonuc.Text);
                sayi1Atali = true;
            }

            else
            {
                if (sonislem == "+")
                    sayi1 += Convert.ToDouble(islemSonuc.Text);

                else if (sonislem == "-")
                    sayi1 -= Convert.ToDouble(islemSonuc.Text);

                else if (sonislem == "x")
                    sayi1 *= Convert.ToDouble(islemSonuc.Text);

                else if (sonislem == "÷")
                {
                    if (Convert.ToDouble(islemSonuc.Text) == 0)
                    {
                        kilitle();
                        return;
                    }
                    sayi1 /= Convert.ToDouble(islemSonuc.Text);
                }

            }
            sonislem = islem.Text;


            islemSonuc.Text = sayi1.ToString();
            label5.Text = sayi1 + " " + islem.Text + " ";
        }

        private void birbolux_MouseClick(object sender, MouseEventArgs e)
        {
            double sayi = Convert.ToDouble(islemSonuc.Text);
            if (sayi == 0)
            {
                kilitle();
                return;
            }

            label5.Text = "1/(" + sayi + ")";
            islemSonuc.Text = (1 / sayi).ToString();
        }

        private void tersle_MouseClick(object sender, MouseEventArgs e)
        {
            double sayi = Convert.ToDouble(islemSonuc.Text);
            if (sayi != 0)
            {
                sayi = -1 * sayi;
                islemSonuc.Text = sayi.ToString();
            }
        }

        private void xkare_MouseClick(object sender, MouseEventArgs e)
        {
            double sayi = Convert.ToDouble(islemSonuc.Text);
            label5.Text = "sqr(" + sayi + ")";
            islemSonuc.Text = Math.Pow(sayi, 2).ToString();
        }

        private void karekok_MouseClick(object sender, MouseEventArgs e)
        {
            double sayi = Convert.ToDouble(islemSonuc.Text);
            label5.Text = "√(" + sayi + ")";
            islemSonuc.Text = Math.Sqrt(sayi).ToString();
        }

        private void esittir_MouseClick(object sender, MouseEventArgs e)
        {
            if (sonislem == "")
            {
                label5.Text = islemSonuc.Text + "=";
                return;
            }

            sayi2 = Convert.ToDouble(islemSonuc.Text);
            if (sonislem == "+")
                sonuc = sayi1 + sayi2;

            else if (sonislem == "-")
                sonuc = sayi1 - sayi2;

            else if (sonislem == "x")
                sonuc = sayi1 * sayi2;

            else if (sonislem == "÷")
            {
                if (sayi2 == 0)
                {
                    kilitle();
                    return;
                }
                sonuc = sayi1 / sayi2;
            }

            label5.Text = string.Format("{0} {1} {2} =", sayi1, sonislem, sayi2);
            sayi1Atali = false;
            silinecek = true;
            islemSonuc.Text = sonuc.ToString();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics grafik = e.Graphics;
            Pen kalem = new Pen(Color.FromArgb(192, 203, 223), 1);

            Rectangle bolge = new Rectangle(0, 0, this.Width, 110);
            LinearGradientBrush lgb = new LinearGradientBrush(bolge, Color.FromArgb(192, 203, 223), Color.FromArgb(243, 243, 243), LinearGradientMode.Vertical);

            grafik.FillRectangle(lgb, bolge);
            grafik.DrawRectangle(kalem, bolge);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!islemSonuc.Focused)
                islemSonuc_KeyUp(sender, e);
        }

        private void yuzde_MouseClick(object sender, MouseEventArgs e)
        {
            double sayi = Convert.ToDouble(islemSonuc.Text);

            label5.Text = sayi + "/100";
            islemSonuc.Text = (sayi / 100).ToString();
        }

        private void mcikar_MouseClick(object sender, MouseEventArgs e)
        {
            label5.Text = "";
            silinecek = true;
            hafizadakiDeger -= Convert.ToDouble(islemSonuc.Text);
        }

        private void mekle_MouseClick(object sender, MouseEventArgs e)
        {
            label5.Text = "";
            silinecek = true;
            hafizadakiDeger += Convert.ToDouble(islemSonuc.Text);
        }

        private void mcagir_MouseClick(object sender, MouseEventArgs e)
        {
            islemSonuc.Text = hafizadakiDeger.ToString();
        }

        private void msakla_MouseClick(object sender, MouseEventArgs e)
        {
            hafizadakiDeger = Convert.ToDouble(islemSonuc.Text);
            mtemizle.Enabled = true;
            mcagir.Enabled = true;
            silinecek = true;
        }

        private void mtemizle_MouseClick(object sender, MouseEventArgs e)
        {
            hafizadakiDeger = 0;
            mtemizle.Enabled = false;
            mcagir.Enabled = false;
        }

        private void kilitle()
        {
            kilitli = true;
            birbolux.Enabled = false;
            bolme.Enabled = false;
            carpi.Enabled = false;
            eksi.Enabled = false;
            arti.Enabled = false;
            esittir.Enabled = false;
            virgul.Enabled = false;
            tersle.Enabled = false;
            teklisilme.Enabled = false;
            xkare.Enabled = false;
            karekok.Enabled = false;
            mtemizle.Enabled = false;
            mcagir.Enabled = false;
            mekle.Enabled = false;
            mcikar.Enabled = false;
            msakla.Enabled = false;
            hafizadakiDeger = 0;
            label5.Text = "";
            islemSonuc.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            islemSonuc.Text = "Sıfıra bölünemez.";
        }

        private void kilitAc()
        {
            kilitli = false;
            birbolux.Enabled = true;
            bolme.Enabled = true;
            carpi.Enabled = true;
            eksi.Enabled = true;
            arti.Enabled = true;
            esittir.Enabled = true;
            virgul.Enabled = true;
            tersle.Enabled = true;
            teklisilme.Enabled = true;
            xkare.Enabled = true;
            karekok.Enabled = true;
            mekle.Enabled = true;
            mcikar.Enabled = true;
            msakla.Enabled = true;
            islemSonuc.Font = new Font("Segoe UI", 36, FontStyle.Bold);
        }


        private void islemSonuc_KeyUp(object sender, KeyEventArgs e)
        {
            if (kilitli)
            {
                temizle_MouseClick(sender, null);
                return;
            }

            if (e.KeyCode == Keys.Back)
            {
                teklisilme_MouseClick(sender, null);
                e.Handled = true;
                return;
            }

            else if (e.KeyCode == Keys.Oemcomma)
            {
                virgul_MouseClick(sender, null);
                e.Handled = true;
                return;
            }

            else if (e.KeyCode == Keys.D4 && e.Shift)
            {
                islemDugme_MouseClick(arti, null);
                e.Handled = true;
                return;
            }

            else if (e.KeyCode == Keys.OemMinus && !e.Shift)
            {
                islemDugme_MouseClick(eksi, null);
                e.Handled = true;
                return;
            }

            else if (e.KeyCode == Keys.Multiply || (e.KeyCode == Keys.Oem8 && !e.Shift))
            {
                islemDugme_MouseClick(carpi, null);
                e.Handled = true;
                return;
            }

            else if (e.KeyCode == Keys.Divide || (e.KeyCode == Keys.D7 && e.Shift))
            {
                islemDugme_MouseClick(bolme, null);
                e.Handled = true;
                return;
            }

            else if (e.KeyCode == Keys.Enter || (e.KeyCode == Keys.D0 && e.Shift))
            {
                esittir_MouseClick(sender, null);
                e.Handled = true;
                return;
            }

            else if (e.KeyCode == Keys.D5 && e.Shift)
            {
                yuzde_MouseClick(sender, null);
                e.Handled = true;
                return;
            }

            if (char.IsDigit((char)e.KeyValue))
            {
                if (silinecek)
                {
                    islemSonuc.Text = "";
                    silinecek = false;
                }

                islemSonuc.Text += (char)e.KeyValue;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string mesaj = string.Format("{0}\nSürüm: {1}\nYazar: {2}\n{3}", programIsim, programSurum, programYazar, programBildiri);
            MessageBox.Show(mesaj, programIsim, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
