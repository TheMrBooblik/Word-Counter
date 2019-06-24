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
using System.IO.Ports;

namespace Проект_курсовая_
{
    public partial class Form1 : Form
    {
        private bool Mode; // Режим разрешения / запрета ввода даных
        private SaveFileDialog sf;

        private HauptArbeit HauptObject; //поле содержит рабочий объект

        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        Timer timer;
        string InputData = String.Empty;
        delegate void SetTextCallback(string text);

        string inputWords;
        string[] outputWords = new string[50];
        bool error1;
        bool error2;
        bool error3 = true;

        public Form1()
        {
            InitializeComponent();

            infoLabel = new ToolStripLabel();
            infoLabel.Text = " Сurrent date and time";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);

            timer = new Timer() { Interval = 1000 };
            timer.Tick += timer_Tick;
            timer.Start();

                
        }

    

        private void Form1_Load(object sender, EventArgs e)
        {
            label11.Text = null;
            label12.Text = null;

            HauptObject = new HauptArbeit();
            HauptObject.SetTime();
            HauptObject.Modify = false; // запрет записи

            About A = new About(); // создание формы About
           
            A.tAbout.Start();
            
            A.ShowDialog(); // отображения диалогового окна About

            toolTip1.SetToolTip(bStart, "Click the button to begin data entry.");
            toolTip1.IsBalloon = true;
            this.Mode = true;

            // получаем список СОМ портов системы
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            };
            
        }

        private void textBoxEnterClick(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                startCount();
            }
        }

        private void startCount()
        {
            label11.Text = "";
            inputWords = tbInput.Text;
            toArray(inputWords, ref outputWords);
            sorting(inputWords, ref outputWords);
        }

        private void toArray(string inputWords, ref string[] outputWords)
        {
            outputWords = inputWords.Split(',');
            char trimChars = ' ';

            for (int i = 0; i < outputWords.Length; i++)
            {
                outputWords[i] = outputWords[i].Trim(trimChars);
            }
        }

        private void sorting(string inputWords, ref string[] outputWords)
        {
            string[] temp_output_words = new string[outputWords.Length];
            int words_count = 0;

            for (int i = 0; i < outputWords.Length; i++)
            {
                temp_output_words[i] = outputWords[i];
            }

            for (int i = 0; i < temp_output_words.Length; i++)
            {
                for (int j = 0; j < temp_output_words.Length; j++)
                {
                    if (outputWords[i] == temp_output_words[j] && outputWords[i] != "")
                    {
                        temp_output_words[j] = null;
                        words_count++;
                    }
                }

                if (words_count != 0)
                {
                    label11.Text += outputWords[i] + ": " + words_count + '\n';
                }

                words_count = 0;

                if (outputWords[i].Contains("."))
                {
                    error3 = false;
                }
                else
                {
                    error3 = true;
                }

                if (outputWords[i].Length > 7)
                {
                    error1 = true;
                }
                else if (outputWords[i].Length < 3)
                {
                    error2 = true;
                }
                else
                {
                    error1 = false;
                    error2 = false;
                    continue;
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }

        private void tClock_Tick(object sender, EventArgs e)
        {
            tClock.Stop();
            // MessageBox.Show("Прошло 5 секунд", "Внимание"); // Вывод сообщения "Прошло 5 секунд" "Внимание" на экран

            tClock.Start();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            if (Mode)
            {
                tbInput.Enabled = true; // Режим размещения ввода
                tbInput.Focus();
              
                tClock.Start();
                bStart.Text = "Stop"; // смена текста на кнопке на "стоп"
                this.Mode = false;
                пускToolStripMenuItem.Text = "Stop";
                

                error1 = false;
                error2 = false;
                error3 = true;
                startCount();
            }
            else
            {
                tbInput.Enabled = false; // Режим запрета ввода
              
                tClock.Stop();
                bStart.Text = "Start"; // смена текста на кнопке на "Пуск"
                this.Mode = true;
                HauptObject.Write(tbInput.Text,label11.Text); // запись данных внутрь объекта
             

                HauptObject.Task();


                if (error1 == true)
                {
                    tClock.Stop();
                    MessageBox.Show("You know buddy, I'm afraid of long words.\nIf you din't mind, please use words up to 7 characters", "Error");
                    tClock.Start();
                }

                if (error2 == true)
                {
                    tClock.Stop();
                    MessageBox.Show("One of the words too short!\nIf you din't mind, please use words with 3 characters and moar", "Error");
                    tClock.Start();
                }

                if (error3 == true)
                {
                    tClock.Stop();
                    MessageBox.Show("U forgot type point in the end.", "Error");
                    tClock.Start();
                }

                //label11.Text = HauptObject.Read();
                пускToolStripMenuItem.Text = "Start";

                error1 = false;
                error2 = false;


            }
            
        }

        private void tbInput_KeyPress(object sender, KeyPressEventArgs e) // разрешение ввода символов для первой строки
        {
            tClock.Stop();
            tClock.Start();

            if ((e.KeyChar == (char)8) |
                (e.KeyChar == (char)44)|
                (e.KeyChar == (char)45)|
                (e.KeyChar == (char)46)|
                (e.KeyChar == (char)3)|
                (e.KeyChar == (char)22)|
                 (e.KeyChar >= 'A') &
                 (e.KeyChar <= 'Z') |
                 (e.KeyChar >= 'a') &
                 (e.KeyChar <= 'z') |
                 (e.KeyChar == 32 ) |
                 (e.KeyChar == 13))
            {
                return;
            }
            else {
                tClock.Stop();
                MessageBox.Show("Wrong character", "Error");
                tClock.Start();
                e.KeyChar = (char)0;           
            }
        }

        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) // метод закрытия Form1
        {
            string s;
            s = (System.DateTime.Now - HauptObject.GetTime()).ToString();
            MessageBox.Show(s, " Time tracker");

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About A = new About();
            A.progressBar1.Visible = false;

            A.ShowDialog();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSave.ShowDialog() == DialogResult.OK) // Вызов диалога сохранения файла
            {
                HauptObject.WriteSaveFileName(sfdSave.FileName); // написание имени

                HauptObject.Generator();
                HauptObject.SaveToFile(); // метод сохранения в файл
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ofdOpen.ShowDialog() == DialogResult.OK) // Вызов диалога открытия файла
            {
                HauptObject.WriteOpenFileName(ofdOpen.FileName);
                // Вызов диалога открытия файла
                HauptObject.ReadFromFile(dgwOpen);  // чтение данных из файла
               

            }

        }

        private void оНакопителяхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] disks = System.IO.Directory.GetLogicalDrives(); // строковый массив из логических дисков
            string disk = "";
            for (int i = 0; i < disks.Length; i++)
            {
                

                try
                {

                    System.IO.DriveInfo D = new
                        System.IO.DriveInfo(disks[i]);
                    disk += D.Name + "-" + (Convert.ToDouble(D.TotalSize.ToString())) /1000000000 + "-" +
                      (Convert.ToDouble( D.TotalFreeSpace.ToString())) / 1000000000 + (char)13; 

                }
                catch
                {
                    disk += disks[i] + "- не готов" + (char)13;
                }

            }
                MessageBox.Show(disk, " накопители");
                    
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
            // поле сохранить
        {
            if (HauptObject.SaveFileNameExists()) // существует ли имя файла
                HauptObject.SaveToFile();
            else
                сохранитьКакToolStripMenuItem_Click(sender, e);
            // сохранить данные в "имя файла"
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // очистка всего, что было в поле результата
            HauptObject.NewRec();
            tbInput.Clear(); //очистить содержимое текст бокса
            
       
            label1.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HauptObject.Modify)
                if (MessageBox.Show("Data not saved. Are you sure you want to exit without saving?",
                    "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                    // Вывод на экран сообщения "Data not saved. Are you sure you want to exit without saving?"
                    e.Cancel = true; // прекратить закрытие

            button1.Text = "Стоп";
            Application.DoEvents(); // Обрабатывает все сообщения Windows, которые в данный момент 
            // находятся в очереди сообщений
            button1_Click(sender, e);
            Application.DoEvents();
                    
        }

        private void Поиск_Click(object sender, EventArgs e)
        {
            HauptObject.Find(tbSearch.Text); // поиск текста 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HauptObject.myStack.Push(Stacktb.Text);
            HauptObject.myArr[HauptObject.myArr.Length - HauptObject.myStack.Count] = Stacktb.Text;
               
            LabeLStack.Text = "";

            for (int i = 0; i < HauptObject.myArr.Length; i++)
            {
                if (HauptObject.myArr[i] != null)
                {
                    LabeLStack.Text += HauptObject.myArr[i] + (char)13;

                }
                else
                {
                    continue;
                }
            }

            if (button1.Text == "Старт")
            {
                if (port.IsOpen) port.Close();

                #region Задаем параметры порта 
                port.PortName = comboBox1.Text;
                port.BaudRate = Convert.ToInt32(comboBox2.Text);
                port.DataBits = Convert.ToInt32(comboBox3.Text);
                switch (comboBox4.Text)
                {
                    case "Пробел":
                        port.Parity = Parity.Space;
                        break;
                    case "Чет":
                        port.Parity = Parity.Even;
                        break;
                    case "Нечет":
                        port.Parity = Parity.Odd;
                        break;
                    case "Маркер":
                        port.Parity = Parity.Mark;
                        break;
                    default:
                        port.Parity = Parity.None;
                        break;
                }
                switch (comboBox5.Text)
                {
                    case "2":
                        port.StopBits = StopBits.Two;
                        break;
                    case "1.5":
                        port.StopBits = StopBits.OnePointFive;
                        break;
                    case "Нет":
                        port.StopBits = StopBits.None;
                        break;
                    default:
                        port.StopBits = StopBits.One;
                        break;
                }
                switch (comboBox6.Text)
                {
                    case "Xon/Xoff":
                        port.Handshake = Handshake.XOnXOff;
                        break;
                    case "Аппаратное":
                        port.Handshake = Handshake.RequestToSend;
                        break;
                    default:
                        port.Handshake = Handshake.None;
                        break;
                }
                #endregion

                try
                {
                    port.Open();
                    button1.Text = "Стоп";
                    button2.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("Порт " + port.PortName + " невозможно открыть!","Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox1.SelectedText = "";
                    button1.Text = "Старт";

                }

            }
            else
            {
                if (port.IsOpen) port.Close();
                button1.Text = "Старт";
                button2.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (HauptObject.myStack.Count > 0)
            {
                MessageBox.Show("Peek " + HauptObject.myStack.Peek());

            }
            if (HauptObject.myStack.Count == 0)
                MessageBox.Show("\n Стек пуст ! ");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (HauptObject.myStack.Count == 0)
                MessageBox.Show("\n Стек пуст ! ");
            else
            {
                HauptObject.myArr[HauptObject.myArr.Length - HauptObject.myStack.Count] = null;

                if (HauptObject.myStack.Count > 0 )
                {
                    MessageBox.Show("Pop " + HauptObject.myStack.Pop());

                }
                LabeLStack.Text = " ";
                for (int i = 0; i < HauptObject.myArr.Length; i ++)
                {
                    if (HauptObject.myArr[i] != null)
                    {
                        LabeLStack.Text += HauptObject.myArr[i] + (char)13;

                    }
                    else
                    {
                        continue;
                    }
                    if (HauptObject.myStack.Count == 0)
                        MessageBox.Show("\n Стек пуст !");

                }

            }
        }

        private void Enqueue_Click(object sender, EventArgs e)
        {
            HauptObject.myQueue.Enqueue(Queuetb.Text);
            HauptObject.QueueArr[HauptObject.myQueue.Count - 1] = Queuetb.Text;
            LabelQueue.Text = " ";

            for (int i = 0; i < HauptObject.QueueArr.Length; i ++)
            {
                if (HauptObject.QueueArr[i] != null)
                {
                    LabelQueue.Text += HauptObject.QueueArr[i] + (char)13;

                }
                else
                {
                    continue;
                }
            }
        }

        private void Peek_q_Click(object sender, EventArgs e)
        {
            if (HauptObject.myQueue.Count > 0)
            {
                MessageBox.Show("Peek " + HauptObject.myQueue.Peek());

            }
            if (HauptObject.myQueue.Count == 0)
                MessageBox.Show("\n Очередь пустая !");
        }

        private void Dequeue_Click(object sender, EventArgs e)
        {
            if (HauptObject.myQueue.Count == 0)
                MessageBox.Show("\n Очередь пустая !");
            else
            {
                HauptObject.QueueArr[0] = null;
                // сдвиг элементов влево на одну позицию
                for (int i = 0; i < HauptObject.QueueArr.Length - 1; i++)
                {
                    HauptObject.QueueArr[i] = HauptObject.QueueArr[i + 1];
                }
                // извлечение элемента из очереди
                if (HauptObject.myQueue.Count > 0)
                {
                    MessageBox.Show("Dequeue " + HauptObject.myQueue.Dequeue());

                }
                // формирование текста для вывода на экран 

                LabelQueue.Text = " ";
                for (int i = 0; i < HauptObject.QueueArr.Length - 1; i++)
                {
                    if (HauptObject.QueueArr[i] != null)
                    {
                        LabelQueue.Text += HauptObject.QueueArr[i] + (char)13;

                    }
                    else
                    {
                        continue;
                    }

                }
                if (HauptObject.myQueue.Count == 0)
                    MessageBox.Show("\n Очередь пустая !");
            }
        }

        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (HauptObject.SaveTextFileNameExists())
                HauptObject.SaveToTextFile(sf.FileName, dgwOpen);
            else
                сохранитьКакToolStripMenuItem1_Click(sender, e);
        }

        private void сохранитьКакToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = @"Текстовый файл (*.txt) |*.txt| Текстовый файл TXT(*.txt) |*.txt|CSV-файл 
(*.csv) |*.csv|Bin-файл (*.bin)|*.bin ";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                HauptObject.WriteSaveTextFileName(sf.FileName);
                HauptObject.SaveToTextFile(sf.FileName, dgwOpen);
            }
        }

        private void открытьToolStripMenuItem1_Click(object sender, EventArgs e)  //
       {
          OpenFileDialog o = new OpenFileDialog();
            o.Filter = @"Текстовый файл (*.txt)|*.txt| Текстовый файл 
  TXT(*.txt)|*.txt|CSV-файл (*.csv)|*.csv|Bin-файл (*.bin)|*.bin";
           if(o.ShowDialog() == DialogResult.OK)
          {
              richTextBox1.Text = File.ReadAllText(o.FileName, Encoding.Default);

          }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "Старт")
            {
                button5.Text = "Стоп";
                button4.Enabled = false;
            }
            else if (button5.Text == "Стоп")
            {
                button5.Text = "Старт";
                button4.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Старт")
            {
                if (port.IsOpen) port.Close();
                #region Задаем параметры порта
                port.PortName = comboBox1.Text;
                port.BaudRate = Convert.ToInt32(comboBox2.Text);
                port.DataBits = Convert.ToInt32(comboBox3.Text);
                switch (comboBox4.Text)
                {
                    case "Пробел":
                        port.Parity = Parity.Space;
                        break;
                    case "Чет":
                        port.Parity = Parity.Even;
                        break;
                    case "Нечет":
                        port.Parity = Parity.Odd;
                        break;
                    case "Маркер":
                        port.Parity = Parity.Mark;
                        break;
                    default:
                        port.Parity = Parity.None;
                        break;
                }
                switch (comboBox5.Text)
                {
                    case "2":
                        port.StopBits = StopBits.Two;
                        break;
                    case "1.5":
                        port.StopBits = StopBits.OnePointFive;
                        break;
                    case "Нет":
                        port.StopBits = StopBits.None;
                        break;
                    default:
                        port.StopBits = StopBits.One;
                        break;
                }
                switch (comboBox6.Text)
                {
                    case "Xon/Xoff":
                        port.Handshake = Handshake.XOnXOff;
                        break;
                    case "Аппаратное":
                        port.Handshake = Handshake.RequestToSend;
                        break;
                    default:
                        port.Handshake = Handshake.None;
                        break;
                }
                #endregion
                try
                {
                    port.Open();
                    button5.Text = "Стоп";
                    button4.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("Порт" + port.PortName + "невозможно открыть!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox1.SelectedText = "";
                    button5.Text = "Старт";
                }
            }
            else
            {
                if (port.IsOpen) port.Close();
                button5.Text = "Старт";
                button4.Enabled = true;
            }
        }

        void AddData (string text)
        {
            listBox1.Items.Add(text);

        }
        private void SetText(string text)
        {
            if(this.listBox1.InvokeRequired)
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.



                SetTextCallback d = new SetTextCallback(SetText); // метод передачи данных
                                                                  //из одного потока в другой

                this.Invoke(d, new object[] { text });


            }
            else
            {
                this.AddData(text);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                groupBox2.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else
            {
                groupBox2.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = true;
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            InputData = port.ReadExisting();
            if (InputData != String.Empty)
            {
                SetText(InputData);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tbInput_KeyUp(object sender, KeyEventArgs e)
        {
            startCount();

            int n = 0;

            for (int i = 0; i < outputWords.Length; i++)
            {
                if (outputWords[i] != null && outputWords[i] != "")
                {
                    n++;
                }
            }

            label12.Text = n + "/50 words";

            if (n <= 50)
            {
                return;
            }
            else
            {
                tClock.Stop();
                MessageBox.Show("Oh, sorry, your text too long for me. \n Now I able to count up to 50 words, not more.", "Error");
                tClock.Start();
            }

        }

        private void tbInput_KeyPress_1(object sender, KeyPressEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dgwOpen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
               