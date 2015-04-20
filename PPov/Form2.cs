using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PPov
{
    public partial class Form2 : Form
    {
        Form3 form3 = new Form3();
        string ID, NamCod, CharCod, Nominol, Imya, Znach, obr, date;
        string[] arr = new string[6];
        double a, b, c, d, e, f, g, result;
        int k = 0;
        int koef = 0;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            //Готовим датагридвью
            this.dataGridView1.Rows.Clear(); //подготовили для нового заполнения 
            this.dataGridView1.ColumnCount = 6;
            this.dataGridView1.Columns[0].HeaderText = "ID";
            this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
            this.dataGridView1.Columns[1].HeaderText = "Номерной код";
            this.dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.Automatic;
            this.dataGridView1.Columns[2].HeaderText = "Буквенный код";
            this.dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.Automatic;
            this.dataGridView1.Columns[3].HeaderText = "Номинал";
            this.dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.Automatic;
            this.dataGridView1.Columns[4].HeaderText = "Название";
            this.dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.Automatic;
            this.dataGridView1.Columns[5].HeaderText = "Курс";
            this.dataGridView1.Columns[5].SortMode = DataGridViewColumnSortMode.Automatic;

            richTextBox1.Text = "";

            XmlTextReader reader = new XmlTextReader("http://pfsoft.com.ua/service/currency/");
            koef = 0;
            k = 0; //тонкая грань
               reader.ReadToFollowing("Notify");
               if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "Notify")
               {
                    label3.Visible = false;//кода, где
                    MessageBox.Show(reader.ReadString(), "Связь с сервером не установлена",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Question);
                    k++; //идёт проверка XML документа на доступность
                }

               reader = new XmlTextReader("http://pfsoft.com.ua/service/currency/");
               reader.ReadToFollowing("ValCurs");
               reader.MoveToFirstAttribute();
               date = reader.Value;
               date = date.Replace("/", ".");
               label3.Text = "Последнее обновление: " + date;
               

            reader = new XmlTextReader("http://pfsoft.com.ua/service/currency/");
            while (reader.Read() && k == 0)
            {
                label3.Visible = true;
                reader.ReadToFollowing("Valute");
                reader.MoveToFirstAttribute();
                ID = reader.Value;


                reader.ReadToFollowing("NumCode");
                if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "NumCode")
                {
                    NamCod = reader.ReadString();
                }

                reader.ReadToFollowing("CharCode");
                if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "CharCode")
                {
                    CharCod = reader.ReadString();
                }

                reader.ReadToFollowing("Nominal");
                if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "Nominal")
                {
                    Nominol = reader.ReadString();
                }

                reader.ReadToFollowing("Name");
                if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "Name")
                {
                    Imya = reader.ReadString();
                }

                reader.ReadToFollowing("Value");
                if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "Value")
                {
                    Znach = reader.ReadString();
                }

                if (koef == 0)
                {
                    richTextBox1.Text += "Последнее обновление: " + date + "\n";
                    koef++;
                }

                arr = new string[] { ID, NamCod, CharCod, Nominol, Imya, Znach };
                richTextBox1.Text += ("ID валюты: " + ID + "\n" + "Номерной код: " + NamCod + "\n" + "Буквенный код: " + CharCod + "\n" + "Номинал: " + Nominol + "\n" + "Название: " + Imya + "\n" + "Стоимость: " + Znach + "\n\n");
                this.dataGridView1.Rows.Add(arr);
            }
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {

                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridView1.Rows.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                comboBox1.Items.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                comboBox2.Items.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
            }
            comboBox1.Items.Add("UAH");
            comboBox2.Items.Add("UAH");
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "UAH" || comboBox2.Text == "UAH")
            {

                {
                    //смотрим на одинаковый текст
                    if (comboBox1.Text == comboBox2.Text)
                    {
                        comboBox1.SelectedIndex = -1;
                        MessageBox.Show("Укажите другую валюту", "Неверно указана валюта",
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Question);
                    }

                    //применяем поиск по колонке
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //важная строка для съедания дабла с точкой

                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    try
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Equals(comboBox1.Text))
                            {
                                c = double.Parse(row.Cells[5].Value.ToString());
                                b = double.Parse(row.Cells[3].Value.ToString());
                                break;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "UAH" || comboBox1.Text == "UAH")
            {
                if (comboBox2.Text == comboBox1.Text)
                {
                    comboBox2.SelectedIndex = -1;
                    MessageBox.Show("Укажите другую валюту", "Неверно указана валюта",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Question);
                }

                //применяем поиск по колонке
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //важная строка для съедания дабла с точкой

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[2].Value.ToString().Equals(comboBox2.Text))
                        {
                            f = double.Parse(row.Cells[5].Value.ToString());
                            d = double.Parse(row.Cells[3].Value.ToString());
                            break;
                        }
                    }
                }
                catch (Exception exc)
                {
                  //  MessageBox.Show(exc.Message);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //важная строка для съедания дабла с точкой
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //важная строка для съедания дабла с точкой
            if (textBox1.Text != "")
            {
                g = double.Parse(textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "UAH" && comboBox2.Text != "UAH")
            {
                result = ((c / b) * g) / (f / d);
                var strres = string.Format("{0:0.##}", result);
                MessageBox.Show("Обменяв " + g + " " + comboBox1.Text + " вы получите " + strres + " " + comboBox2.Text, "Обмен валют",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Question);
            }

            if (comboBox1.Text == "UAH" && comboBox2.Text != "UAH")
            {
                result = g/(f / d);
                var strres = string.Format("{0:0.##}", result);
                MessageBox.Show("Обменяв " + g + " " + comboBox1.Text + " вы получите " + strres + " " + comboBox2.Text, "Обмен валют",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Question);
            }

            if (comboBox1.Text != "UAH" && comboBox2.Text == "UAH")
            {
                result = g * (c / b);
                var strres = string.Format("{0:0.##}", result);
                MessageBox.Show("Обменяв " + g + " " + comboBox1.Text + " вы получите " + strres + " " + comboBox2.Text, "Обмен валют",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Question);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
                richTextBox1.SaveFile(date+".txt", RichTextBoxStreamType.PlainText);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            form3.Show();
            this.Hide();
            var fullPath = Application.StartupPath + @"\help.html";
            form3.webBrowser1.Navigate(fullPath);

        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}