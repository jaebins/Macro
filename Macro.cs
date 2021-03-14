using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Macro
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern int mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        private const uint l_down = 0x002; // 왼쪽 마우스 클릭
        private const uint l_up = 0x004; // 왼쪽 마우스 뗌
        private const uint keydown = 0x1;
        private const uint keyup = 0x2;

        public bool isStop = true;
        public bool isRepeatStart;
        public string[] setButton = new string[8];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1_Tick(sender, e);
            timer1.Interval = 300;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            XLabel.Text = "X : " + Cursor.Position.X.ToString();
            YLabel.Text = "Y : " + Cursor.Position.Y.ToString();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (isRepeatStart && !isStop)
            {
                return;
            }
            Thread t1 = new Thread(new ThreadStart(RepeatStart));
            t1.IsBackground = true;
            t1.Start();
        }

        private void LoopStartButton_Click(object sender, EventArgs e)
        {
            if (isRepeatStart && !isStop)
            {
                return;
            }
            Thread t1 = new Thread(new ThreadStart(StartMacro));
            t1.IsBackground = true;
            t1.Start();
        }

        private void RepeatStart()
        {
            isRepeatStart = true;

            Point point;
            if (!String.IsNullOrEmpty(InputMouseX.Text) && !String.IsNullOrEmpty(InputMouseY.Text))
            {
                point = new Point(Convert.ToInt32(InputMouseX), Convert.ToInt32(InputMouseY.Text));
                Cursor.Position = point;
            }
            while (isRepeatStart)
            {
                //mouse_event(l_down, 0, 0, 0, 0);
                //mouse_event(l_up, 0, 0, 0, 0);
                Thread.Sleep(100);
            }

            isRepeatStart = false;
        }

        private void StartMacro()
        {
            isStop = false;

            setButton[0] = InputSet1.Text;
            setButton[1] = InputSet2.Text;
            setButton[2] = InputSet3.Text;
            setButton[3] = InputSet4.Text;
            setButton[4] = InputSet5.Text;
            setButton[5] = InputSet6.Text;
            setButton[6] = InputSet7.Text;
            setButton[7] = InputSet8.Text;

            int loop_cur = Convert.ToInt32(InputRepeatCount.Text);

            if (CheckLoop.Checked)
            {
                while (!isStop)
                {
                    for (int j = 0; j < setButton.Length; j++)
                    {
                        if (String.IsNullOrEmpty(setButton[j]))
                        {
                            break;
                        }
                        string[] mousePos = setButton[j].Split(',');
                        Cursor.Position = new Point(Convert.ToInt32(mousePos[0]), Convert.ToInt32(mousePos[1]));

                        for (int i = 0; i < loop_cur; i++)
                        {
                            mouse_event(l_down, 0, 0, 0, 0);
                            mouse_event(l_up, 0, 0, 0, 0);
                            Thread.Sleep(50);
                            i++;
                        }
                    }
                }
            }
            else
            {
                for(int j = 0; j < setButton.Length; j++)
                {
                    if (String.IsNullOrEmpty(setButton[j]))
                    {
                        break;
                    }
                    string[] mousePos = setButton[j].Split(',');
                    Cursor.Position = new Point(Convert.ToInt32(mousePos[0]), Convert.ToInt32(mousePos[1]));

                    for (int i = 0; i < loop_cur; i++)
                    {
                        mouse_event(l_down, 0, 0, 0, 0);
                        mouse_event(l_up, 0, 0, 0, 0);
                        Thread.Sleep(50);
                        i++;
                    }
                }
            }

            isStop = true;
            MessageBox.Show("오토마우스가 끝남");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F4:
                    StopMacro();
                    break;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StopMacro();
        }

        private void StopMacro()
        {
            if (isRepeatStart)
            {
                isRepeatStart = false;
            }
            if (!isStop)
            {
                isStop = true;
            }
        }
    }
}
