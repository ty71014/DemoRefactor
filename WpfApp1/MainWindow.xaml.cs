﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasyModbus;
using System.Timers;
using System.Diagnostics;
using System.Windows.Threading;

namespace WpfApp1
{

    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CycTime();
             connected();
        }

        

        int W01, W02;
        ModbusClient plc1 = new ModbusClient();
        ModbusClient plc2 = new ModbusClient("192.168.50.11", 502);
       
        DispatcherTimer SynctTme001 = new DispatcherTimer();
        Timer TimerLocal = new Timer();
        Timer DefiniteTime = new Timer();
        Timer Recycle001 = new Timer();

        string[] btBox01 = new string[8];
        int[] btBox02 = new int[8];
        bool[] btBox03 = new bool[8];

        
        //一直執行的時間
        private void CycTime()
        {
            TimerLocal.Enabled = true;
            TimerLocal.Interval = 500;
            TimerLocal.Start();
            TimerLocal.Elapsed += timerLocal;

            DefiniteTime.Enabled = true;
            DefiniteTime.Interval = 20000;
            DefiniteTime.Start();
            DefiniteTime.Elapsed += definiteTime;

            Recycle001.Enabled = true;
            Recycle001.Interval = 3000;
            Recycle001.Start();
            Recycle001.Elapsed += recycle001;
        }
        //本地時間
        private void timerLocal(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                string a1= DateTime.Now.Millisecond.ToString();
                LOCALTIME01.Text = "Local時間:"  + DateTime.Now.ToString() +":"+ $"{a1}";

                textBoxT_Year.Text = DateTime.Now.Year.ToString();
                textBoxT_Month.Text = DateTime.Now.Month.ToString();
                textBoxT_Week.Text = ((int)DateTime.Now.DayOfWeek).ToString();
                textBoxT_Day.Text = DateTime.Now.Day.ToString();
                textBoxT_Hr.Text = DateTime.Now.Hour.ToString();
                textBoxT_Min.Text = DateTime.Now.Minute.ToString();
                textBoxT_Sec.Text = DateTime.Now.Second.ToString();
                textBoxT_MiSec.Text = DateTime.Now.Millisecond.ToString();
                
            }));

        }
        //顯示讀取
        private void syncTime001(object sender, EventArgs e)
        {
          
         
            //IP01
            if (ReadStrat01.Text == string.Empty && plc1.Connected == true)
            {

                textBox01.Text = "";
                textBox02.Text = "";
                textBox03.Text = "";
                textBox04.Text = "";
                textBox05.Text = "";
                textBox06.Text = "";
                textBox07.Text = "";
                textBox08.Text = "";

                StateReadSTR01.Text = "IP01缺少起始: " + DateTime.Now.ToString();

            }
            else if (ReadStrat01.Text != string.Empty && plc1.Connected == true)
            {

                try
                {
                    int[] Read001 = plc1.ReadHoldingRegisters(Convert.ToInt32(ReadStrat01.Text), 8);

                    textBox01.Text = Read001[0].ToString();
                    textBox02.Text = Read001[1].ToString();
                    textBox03.Text = Read001[2].ToString();
                    textBox04.Text = Read001[3].ToString();
                    textBox05.Text = Read001[4].ToString();
                    textBox06.Text = Read001[5].ToString();
                    textBox07.Text = Read001[6].ToString();
                    textBox08.Text = Read001[7].ToString();
                    Addr00.Content= int.Parse(ReadStrat01.Text) + 0;
                    Addr01.Content= int.Parse(ReadStrat01.Text) + 1;
                    Addr02.Content= int.Parse(ReadStrat01.Text) + 2;
                    Addr03.Content= int.Parse(ReadStrat01.Text) + 3;
                    Addr04.Content= int.Parse(ReadStrat01.Text) + 4;
                    Addr05.Content= int.Parse(ReadStrat01.Text) + 5;
                    Addr06.Content= int.Parse(ReadStrat01.Text) + 6;
                    Addr07.Content= int.Parse(ReadStrat01.Text) + 7;
                    StateReadSTR01.Text = "IP01順利讀取: " + DateTime.Now.ToString();
                }
                catch (Exception)
                {

                    StateReadSTR01.Text = "IP01起始位置內容格式不對或不正常通訊: " + DateTime.Now.ToString();
                    textBox01.Text = "";
                    textBox02.Text = "";
                    textBox03.Text = "";
                    textBox04.Text = "";
                    textBox05.Text = "";
                    textBox06.Text = "";
                    textBox07.Text = "";
                    textBox08.Text = "";
                }
                
            }


            //IP02
            if (ReadStrat02.Text == string.Empty && plc2.Connected==true)
            {
                
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";
                textBox15.Text = "";
                textBox16.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";

                StateReadSTR02.Text = "IP01缺少起始: " + DateTime.Now.ToString();

            }
            else if (ReadStrat02.Text != string.Empty && plc2.Connected == true)
            {

                try
                {
                    int[] Read002 = plc2.ReadHoldingRegisters(Convert.ToInt32(ReadStrat02.Text), 8);

                    textBox11.Text = Read002[0].ToString();
                    textBox12.Text = Read002[1].ToString();
                    textBox13.Text = Read002[2].ToString();
                    textBox14.Text = Read002[3].ToString();
                    textBox15.Text = Read002[4].ToString();
                    textBox16.Text = Read002[5].ToString();
                    textBox17.Text = Read002[6].ToString();
                    textBox18.Text = Read002[7].ToString();
                    Addr10.Content = int.Parse(ReadStrat02.Text) + 0;
                    Addr11.Content = int.Parse(ReadStrat02.Text) + 1;
                    Addr12.Content = int.Parse(ReadStrat02.Text) + 2;
                    Addr13.Content = int.Parse(ReadStrat02.Text) + 3;
                    Addr14.Content = int.Parse(ReadStrat02.Text) + 4;
                    Addr15.Content = int.Parse(ReadStrat02.Text) + 5;
                    Addr16.Content = int.Parse(ReadStrat02.Text) + 6;
                    Addr17.Content = int.Parse(ReadStrat02.Text) + 7;
                    StateReadSTR02.Text = "IP02順利讀取: " + DateTime.Now.ToString();
                }
                catch (Exception)
                {

                    StateReadSTR02.Text = "IP02起始位置內容格式不對或不正常通訊: " + DateTime.Now.ToString();
                    textBox11.Text = "";
                    textBox12.Text = "";
                    textBox13.Text = "";
                    textBox14.Text = "";
                    textBox15.Text = "";
                    textBox16.Text = "";
                    textBox17.Text = "";
                    textBox18.Text = "";
                }

            }

        }
        //手動連線
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            connected();
        }
        //手動斷線
        private void Button_Click(object sender, RoutedEventArgs e) { Disconnected(3); }
        //手動同步
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            sync01();
            sync02();
        }

        //斷訊條件
        private void  Disconnected(int num)
        {
            if(num==1)
            {
                plc1.Disconnect();
                StateReadSTR01.Text = "已斷線: " + DateTime.Now.ToString();
            }
            else if (num == 2)
            {
             plc2.Disconnect();
                StateReadSTR02.Text = "已斷線: " + DateTime.Now.ToString();
            }
            else if (num == 3)
            {
                plc1.Disconnect();
                StateReadSTR01.Text = "已斷線: " + DateTime.Now.ToString();
                plc2.Disconnect();
                StateReadSTR02.Text = "已斷線: " + DateTime.Now.ToString();
                SynctTme001.Stop();
               // TimerLocal.Stop();


            }

            

            
            LOCALTIME01.Text = "已斷線: " + DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString();
        }
        
        
        //通訊條件
        public void connected()
        {
           
               
            if (plc2.Connected == false)
            {
                //PLC01
                try
                {

                    plc1.IPAddress = txtIP01.Text;
                    plc1.Port = Convert.ToInt32(txtport01.Text);
                    plc1.UnitIdentifier = 1;
                    plc1.Connect();
                    plc1.Available(3000);

                    if (plc1.Connected == true)
                    {



                        SynctTme001.Interval = TimeSpan.FromSeconds(0.5);
                        SynctTme001.Start();
                        SynctTme001.Tick += syncTime001;


                        W01 = 0;
                        StateReadW01.Text = W01.ToString();
                        sync01();
                    }

                }
                catch (Exception)
                {
                    Disconnected(1);

                    W01 = -1;
                    StateReadW01.Text = W01.ToString();
                }
            }
            //PLC02
            if (plc1.Connected == false)
            {
                try
                {


                    plc2.IPAddress = txtIP02.Text;
                    plc2.Port = Convert.ToInt32(txtport02.Text);
                    plc2.UnitIdentifier = 1;
                    plc2.Connect();
                    plc2.Available(3000);
                    if (plc2.Connected == true)
                    {


                        SynctTme001.Interval = TimeSpan.FromSeconds(0.5);
                        SynctTme001.Start();
                        SynctTme001.Tick += syncTime001;


                        W02 = 0;
                        StateReadW02.Text = W02.ToString();
                        sync02();
                    }

                }
                catch (Exception)
                {
                    Disconnected(2);
                    W02 = -1;
                    StateReadW02.Text = W02.ToString();
                }
            }
            
        }

        

        //持續詢問已斷掉的IP
        private void recycle001(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //if (plc1.Connected == false)
                //{
                //    plc2.Connect();
                //}
                //else if (plc2.Connected == false)
                //{
                //    plc1.Connect();
                //}
                //else if (plc1.Connected == false && plc2.Connected == false)
                //{
                    
                //}
            }));
        }
        //呼叫定時
        private void definiteTime(object sender, ElapsedEventArgs e)
        {
           
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Timesyn01();

            }));
        }

        //現在時間
        public void LocalTime()
        {
            //
            btBox01[0] = DateTime.Now.Year.ToString();
            btBox01[1] = DateTime.Now.Month.ToString();
            btBox01[2] = ((int)DateTime.Now.DayOfWeek).ToString();
            btBox01[3] = DateTime.Now.Day.ToString();
            btBox01[4] = DateTime.Now.Hour.ToString();
            btBox01[5] = DateTime.Now.Minute.ToString();
            btBox01[6] = DateTime.Now.Second.ToString();
            btBox01[7] = DateTime.Now.Millisecond.ToString();
        }
        //同步
        public void sync01() 
        {
            LocalTime();
            for (int i = 0; i < 8; i++)
            {
                btBox03[i] = int.TryParse(btBox01[i], out btBox02[i]);
            }
            if (plc1.Connected == true)
            {
                plc1.WriteMultipleRegisters(Convert.ToInt32(ReadStrat01.Text), btBox02);
                System.Threading.Thread.Sleep(200);
                LOCALTIME01.Text = "暫停: " + DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                plc1.WriteSingleRegister(500, 0);
            }
               

        }
        //同步
        public void sync02()
        {
            LocalTime();
            for (int i = 0; i < 8; i++)
            {
                btBox03[i] = int.TryParse(btBox01[i], out btBox02[i]);
            }        
            if (plc2.Connected == true)
            {
                plc2.WriteMultipleRegisters(Convert.ToInt32(ReadStrat02.Text), btBox02);
                System.Threading.Thread.Sleep(200);
                LOCALTIME01.Text = "暫停: " + DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                plc2.WriteSingleRegister(500, 0);
            }
        }

        public void Timesyn01()
        {
           if( textBoxForHr01.Text==DateTime.Now.Hour.ToString() && textBoxForMin01.Text== DateTime.Now.Minute.ToString())
            {
                sync01();
                sync02();

            }
            else if (textBoxForHr02.Text == DateTime.Now.Hour.ToString() && textBoxForMin02.Text == DateTime.Now.Minute.ToString())
            {
                sync01();
                sync02();
            }

        }
        private void textBox0_TextChanged(object sender, TextChangedEventArgs e)
        {
            //timer3.Stop();
            //if (this.textBox1.Text.Length >= 4)
            //{
            //    plc2.WriteSingleRegister(int.Parse(ReadStrat01.Text) + 0, int.Parse(textBox1.Text));
            //}
            //if (this.textBox2.Text.Length >= 1) 
            //{
            //    plc2.WriteSingleRegister(int.Parse(ReadStrat01.Text) + 1, int.Parse(textBox2.Text));
            //}
            //if (this.textBox4.Text.Length >= 1)
            //{
            //    plc2.WriteSingleRegister(int.Parse(ReadStrat01.Text) + 3, int.Parse(textBox4.Text));
            //}
            //if (this.textBox5.Text.Length >= 1)
            //{
            //    plc2.WriteSingleRegister(int.Parse(ReadStrat01.Text) + 4, int.Parse(textBox5.Text));
            //}
            //if (this.textBox6.Text.Length >= 1)
            //{
            //    plc2.WriteSingleRegister(int.Parse(ReadStrat01.Text) + 5, int.Parse(textBox6.Text));
            //}
            //if (this.textBox7.Text.Length >= 1)
            //{
            //    plc2.WriteSingleRegister(int.Parse(ReadStrat01.Text) + 6, int.Parse(textBox7.Text));
            //}

            //if (int.Parse(this.textBox2.Text) > 1 && int.Parse(this.textBox2.Text) < 13 && this.textBox2.Text != string.Empty)
            //{
            //    //plc2.WriteSingleRegister(int.Parse(ReadStrat01.Text) + 1, int.Parse(textBox2.Text));
            //}
            // else if (int.Parse(textBox2.Text) < 1 && int.Parse(textBox2.Text) > 13) { }
           // timer3.Start();
        }

        private void ASCLL_KeyPress(object sender, KeyEventArgs e)
        {
            //判斷案件是不是輸入的類型
                //if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46 && (int)e.KeyChar != 45)//含負號
                if (((int)e.Key < 48 || (int)e.Key > 57) && (int)e.Key != 8 && (int)e.Key != 46)
                e.Handled = true;//不執行

            //小數點處理
            //if ((int)e.Key == 46)
            //{
            //    if (WriteStart01.Text.Length <= 0)
            //        e.Handled = true;   //小數點不可在第一位
            //    else
            //    {
            //        float f;
            //        float oldf;
            //        bool b1 = false, b2 = false;
            //        b1 = float.TryParse(WriteStart01.Text, out oldf);
            //        b2 = float.TryParse(WriteStart01.Text + e.Key.ToString(), out f);
            //        if (b2 == false)
            //        {
            //            if (b1 == true)
            //                e.Handled = true;
            //            else
            //                e.Handled = false;
            //        }
            //    }
            //}
        }
        


     

     
    }
}