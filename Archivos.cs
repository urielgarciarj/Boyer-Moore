using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BoyerMoore_csharp
{
    public partial class Archivos : Form
    {
        public Archivos()
        {
            InitializeComponent();
            richTextBox.Hide();
            label.Hide();
            txtArchive.Hide();
            btnSave.Hide();
            btnSaveAs.Hide();
            label2.Hide();
            txtSaveAsArchive.Hide();
            label3.Hide();
            txtSearch.Hide();
            cbSensible.Hide();
        }

        public static class GlobalData
        {
            public static int pattern = 0; //Largo del patron
            public static int text = 0; //largo del texto
            public static string patternString = "";
            public static string textString = "";
            public static string archivoAbierto = "";
        }
        
        //LAS SIGUIENTE FUINCION TE PERMITE CREAR UN NUEVO ARCHIVO
        private void btnNew_Click(object sender, EventArgs e)
        {
            richTextBox.Show();
            btnSave.Show();
            btnSaveAs.Show();
        }

        //LA SIGUIENTE FUNCION GUARDA EL ARCHIVO CON UN NOMBRE ESTANDAR
        private void btnSave_Click(object sender, EventArgs e)
        {
            /*StreamWriter File = new StreamWriter("Archivo.txt");
            File.Write(richTextBox.Text);
            File.Close();
            richTextBox.Hide();*/
            StreamWriter File = new StreamWriter(GlobalData.archivoAbierto);
            File.Write(richTextBox.Text);
            File.Close();

        }

        //SIRVE PARA ABRIR ARCHIVOS
        private void btnOpen_Click(object sender, EventArgs e)
        {
            label.Show();
            txtArchive.Show();
            btnSave.Show();
        }

        //LA SIGUIENTE FUNCION ABRE EL ARCHIVO QUE SE SOLICITA
        private void txtArchive_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                /*StreamReader leer = new StreamReader(txtArchive.Text);
                string linea;
                try
                {
                    linea = leer.ReadLine();
                    while (linea != null)
                    {
                        if (leer.EndOfStream) 
                        richTextBox.AppendText(linea + "\n");
                        linea = leer.ReadLine();
                    }
                }
                catch
                {
                    MessageBox.Show("Error");
                }*/
                TextReader leer;
                leer = new StreamReader(txtArchive.Text);
                GlobalData.archivoAbierto = txtArchive.Text;
                richTextBox.Text = (leer.ReadLine());
                GlobalData.textString = richTextBox.Text;
                MessageBox.Show(GlobalData.textString);
                leer.Close();
                richTextBox.Show();
            }
        }
        
        //LA SIGUIENTE FUNCION TE ABRE EL TEXTBOX PARA PONER EL NOMBRE DE COMO QUIERAS GUARDAR EL ARCHIVO
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            label2.Show();
            txtSaveAsArchive.Show();
            btnSave.Hide();
            btnSaveAs.Hide();
        }

        //LA SIGUIENTE FUNCION TE PERMITE GUARDAR EL ARCHIVO CON EL NOMBRE QUE DESEES
        private void txtSaveAsArchive_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                StreamWriter File = new StreamWriter(txtSaveAsArchive.Text);
                File.Write(richTextBox.Text);
                File.Close();
                richTextBox.Hide();
                richTextBox.Hide();
                label2.Hide();
                txtSaveAsArchive.Hide();
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            richTextBox.Hide();
            btnSave.Hide();
            btnSaveAs.Hide();
        }

        /*public int BoyerMoore(string myString)
        {
            GlobalData.patternString = myString;
            GlobalData.pattern = myString.Length;
            GlobalData.text = GlobalData.textString.Length;

            int charcount = 0;
            char[] siguiente;
            char[] siguienteint;
            siguiente = myString.ToCharArray();
            for(int i = 1; i < siguiente.Length; i++)
            {
                charcount = i;
            }
            siguiente[charcount] = '0';
            myString = new string(siguiente);
            string a;
            int k = GlobalData.pattern-1;
            int j = GlobalData.pattern-1;
            while (k <= GlobalData.text && j >= 1)
            {
                if (GlobalData.textString[k - (GlobalData.pattern - j - 1)] == GlobalData.patternString[j])
                {
                    j--;
                }
                else
                {
                    a = GlobalData.textString[k - (GlobalData.pattern - j - 1)].ToString();
                    k = k + (GlobalData.pattern - 1 - myString.IndexOf(a));
                    j = GlobalData.pattern - 1;
                }
                if (j == 0)
                {
                    MessageBox.Show("La palabra se encuentra en el archivo");
                    MessageBox.Show(k.ToString());
                    if (k != GlobalData.textString.Length)
                    {
                        j = GlobalData.pattern - 1;
                    }
                }
                else
                {
                    MessageBox.Show("La palabra no se encuentra en el archivo");
                }
            }
            return k;
        }*/

        public static int[] BoyerMoore(string texto, string myString)
        {
            List<int> retVal = new List<int>();
            int m = myString.Length; //Tamaño de todo el texto del archivo
            int n = texto.Length; //Tamaño de la cadena
            int[] badChar = new int[256];
            TablaSiguiente(myString, m, ref badChar);

            int s = 0;
            while (s <= (n - m))
            {
                int j = m - 1;
                while (j >= 0 && myString[j] == texto[s + j])
                
                    --j;
                    if(j < 0)
                    {
                        remarcado(s, s+n);
                        retVal.Add(s);
                        s+= (s + m < n) ? m - badChar[texto[s + m]] : 1;
                    }
                    else
                    {
                        s += Math.Max(1, j - badChar[texto[s + j]]);
                    }
                
            }
            return retVal.ToArray();
        }

        private static void TablaSiguiente(string str, int size, ref int[] badChar)
        {
            int i;

            for (i = 0; i < 256; i++)
                badChar[i] = -1;

            for (i = 0; i < size; i++)
                badChar[(int)str[i]] = i;
        }

        public static void remarcado(int inicio, int tamanio)
        {
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Select(inicio, tamanio);
            richTextBox.SelectionColor = Color.Red;
            richTextBox.Refresh();
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                string myString = txtSearch.Text;
                BoyerMoore(GlobalData.textString , myString);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            label3.Show();
            txtSearch.Show();
            cbSensible.Show();
        }
    }
}
