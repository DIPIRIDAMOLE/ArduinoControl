using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace ArduinoCtrlApplication_x64
{
    public partial class Form1 : Form
    {
        Math3D.Cube mainCube;
        Point drawOrigin;
        int tx = 0;
        int ty = 0;
        int tz = 0;

        public Form1()
        {
            InitializeComponent();
            SetDefaultButton();
            tabControl1.SelectedIndex = 0;
        }

        private void SetDefaultButton()
        {
            button1.Text = "Программа";
            button2.Text = "";
            button3.Text = "";
            button4.Text = "";
            button5.Text = "Настройки";
            button6.Text = "Выход";
            button7.Text = "";
            button8.Text = "";
            button9.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.Selectable, false);
            switch (button6.Text)
            {
                case "Выход":
                    button1.Text = "Ок";
                    button2.Text = "";
                    button3.Text = "";
                    button4.Text = "";
                    button5.Text = "Отмена";
                    button6.Text = "";
                    button7.Text = "";
                    button8.Text = "";
                    button9.Text = "";
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.Selectable, false);
            switch (button1.Text)
            {
                case "Ок":
                    Environment.Exit(0);
                    break;
                case "Отправить":
                    if (!comboBox1.Enabled)
                    {
                        serialPort1.WriteLine(textBox2.Text);
                        textBox2.Text = "";
                    }
                    else
                    {
                        listBox1.Items.Add(DateTime.Now.ToString() + "\t\tСИСТЕМА: Нет подключения");
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                    break;
                case "Программа":
                    tabControl1.SelectedIndex = 3;
                    button1.Text = "";
                    button2.Text = "";
                    button3.Text = "";
                    button4.Text = "";
                    button5.Text = "Назад";
                    button6.Text = "";
                    button7.Text = "";
                    button8.Text = "";
                    button9.Text = "";

                    mainCube = new Math3D.Cube(100, 200, 75);
                    drawOrigin = new Point(pictureBox2.Width / 2, pictureBox2.Height / 2);
                    tx = 0;
                    ty = 0;
                    tz = 0;
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.Selectable, false);
            switch (button5.Text)
            {
                case "Отмена":
                    SetDefaultButton();
                    break;
                case "Настройки":
                    tabControl1.SelectedIndex = 1;
                    button1.Text = "Сохранить";
                    button2.Text = "";
                    button3.Text = "Подключиться";
                    button4.Text = "Консоль";
                    button5.Text = "Назад";
                    button6.Text = "";
                    button7.Text = "";
                    button8.Text = "";
                    button9.Text = "";
                    break;
                case "Назад":
                    tabControl1.SelectedIndex = 0;
                    SetDefaultButton();
                    break;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            comboBox1.Text = "";
            comboBox1.Items.Clear();

            if (ports.Length != 0)
            {
                comboBox1.Items.AddRange(ports);
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (button3.Text)
            {
                case "Подключиться":
                    try
                    {
                        serialPort1.PortName = comboBox1.Text;
                        serialPort1.BaudRate = Int32.Parse(textBox1.Text);
                        serialPort1.Open();
                        comboBox1.Enabled = false;
                        button3.Text = "Отключиться";
                        label3.Text = "COM-порт: " + comboBox1.Text + "\r\nСкорость: " + textBox1.Text + "\r\nСтатус: подключено";
                    }
                    catch
                    {
                        listBox1.Items.Add(DateTime.Now.ToString() + "\t\tСИСТЕМА: Не удалось подключиться");
                        textBox3.Text = "СИСТЕМА: Не удалось подключиться";
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                    break;
                case "Отключиться":
                    serialPort1.Close();
                    comboBox1.Enabled = true;
                    button1.Text = "Подключиться";
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch (button4.Text)
            {
                case "Консоль":
                    tabControl1.SelectedIndex = 2;
                    button1.Text = "Отправить";
                    button2.Text = "";
                    button3.Text = "";
                    button4.Text = "";
                    button5.Text = "Назад";
                    button6.Text = "";
                    button7.Text = "";
                    button8.Text = "";
                    button9.Text = "";
                    break;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (InvokeRequired)
                this.Invoke(new Action(() =>
                {
                    String read = serialPort1.ReadLine();
                    listBox1.Items.Add(DateTime.Now.ToString() + "\t" + read);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }));
            else
            {
                String read = serialPort1.ReadLine();
                listBox1.Items.Add(DateTime.Now.ToString() + "\t" + read);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void Render()
        {
            //mainCube.RotateX = (float)tX.Value;
            //mainCube.RotateY = (float)tY.Value;
            //mainCube.RotateZ = (float)tZ.Value;

            pictureBox2.Image = mainCube.DrawCube(drawOrigin);
        }


        Point lastPoint;
        Point movePoint;

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {  
            if (e.Button == MouseButtons.Left)
            {
                int VecX = (Cursor.Position.X - lastPoint.X);
                int VecY = (Cursor.Position.Y - lastPoint.Y);

                if (Cursor.Position.X - lastPoint.X > 0)
                {
                    movePoint.X = drawOrigin.X - (Cursor.Position.X - lastPoint.X)/15;
                }
                else
                {
                    movePoint.X = drawOrigin.X + (lastPoint.X - Cursor.Position.X)/15;
                }


                if (Cursor.Position.Y - lastPoint.Y > 0)
                {
                    movePoint.Y = drawOrigin.Y - (Cursor.Position.Y - lastPoint.Y)/15;
                }
                else
                {
                    movePoint.Y = drawOrigin.Y + (lastPoint.Y - Cursor.Position.Y)/15;
                }

                drawOrigin = movePoint;
                Render();
            }
            else if (e.Button == MouseButtons.Right)
            {
                int VecX = (Cursor.Position.X - lastPoint.X);
                int VecY = (Cursor.Position.Y - lastPoint.Y);

                if (Cursor.Position.X - lastPoint.X > 0)
                {
                    movePoint.X = tx - drawOrigin.X + (Cursor.Position.X - tx) / 15;
                }
                else
                {
                    movePoint.X = tx + drawOrigin.X - (tx - Cursor.Position.X) / 15;
                }


                if (Cursor.Position.Y - lastPoint.Y > 0)
                {
                    movePoint.Y = ty - drawOrigin.Y + (Cursor.Position.Y - ty) / 15;
                }
                else
                {
                    movePoint.Y = ty + drawOrigin.Y - (ty - Cursor.Position.Y) / 15;
                }

                mainCube.RotateX = movePoint.X;
                mainCube.RotateY = movePoint.Y;
                Render();
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(Cursor.Position.X, Cursor.Position.Y);
        }
    }
}
