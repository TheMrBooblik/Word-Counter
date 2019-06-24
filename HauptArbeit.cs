using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace Проект_курсовая_
{
    class HauptArbeit
    {
        // содержимое поля объекта
        // поле

        private string Data  ; // входные данные
        
        private string Result; // поле результата
       
        private System.DateTime TimeBegin; // время начала работы программы
        public bool Modify;
        private string SaveFileName; //
        private string OpenFileName;

        public Stack myStack = new Stack();
        public string[] myArr = new string[100];

        public Queue myQueue = new Queue();
        public string[] QueueArr = new string[100];

        public void WriteSaveFileName(string S) // метод записи данных в объект
        {
            this.SaveFileName = S; // запомнить имя файла для записи

        }

        public void WriteOpenFileName(string S)
        {
            this.OpenFileName = S; // запомнить имя файла для открытия 
        }

        // методы



        public void Write(string D, string F ) // метод записи данных в объект из первой строки
        {
          this.Data = D;
            this.Result = F;


        }

        


        public string Read()
        {
            return this.Result; // метод отображения результата

        }

       


        // В методе Task реализуеться выполнение задания: если количество введенных цифр больше 5, 
        // то результат = true, иначе - false.

        public void Task() // метод реализации программного задания
        {

          //  if (this.Data.Length == 5)
          //  {
         //       this.Result = Convert.ToString(true);
         //   }
         //   else
         //   {
         //       this.Result = Convert.ToString(false);
         //   }

           


            this.Modify = true; // разрешение записи
        }







        public void SetTime() // метод подсчета времени
        {

            this.TimeBegin = System.DateTime.Now; // начать считать время работы программы
        }
        public System.DateTime GetTime() // метод получения времени работы программы
        {
            return this.TimeBegin; // получить время работы программы
        }

        private int Key;


        public void SaveToFile() // запись данных в файл
        {
            if (!this.Modify)
                return;
            try
            {
                Stream S; // создание потока
                if (File.Exists(this.SaveFileName)) // существует ли файл
                    S = File.Open(this.SaveFileName, FileMode.Append);
                // открытие сохранённого файла
                else
                    S = File.Open(this.SaveFileName, FileMode.Create);
                // создать
                Buffer D = new Buffer(); // создание буфферной переменной
                D.Data = this.Data; // присвоение поля входных значений
               
                D.Result = Convert.ToString(this.Result); // присвоение поля результата
                D.Key = Key; // присвоение поля ключ
                Key++;
                BinaryFormatter BF = new BinaryFormatter();
                //  создание объекта для форматирования
                BF.Serialize(S, D);
                S.Flush(); // очистка буфера потока
                S.Close(); // закрытие потока
                this.Modify = false; // запрет повторной записи
            }
            catch
            {
                MessageBox.Show("Ошибка работы с файлом"); // Вывод на экран сообщения "Ошибка работы с файлом"

            }
        }

        public void ReadFromFile(System.Windows.Forms.DataGridView DG) // считывание из файла
        {
            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("файла нет"); // Вывод на экран сообщения "файла нет"
                    return;
                }
                Stream S; // создание потока
                S = File.Open(this.OpenFileName, FileMode.Open); // считывание данных из файла
                Buffer D;
                object O; // буферная переменная для контроля формата
                BinaryFormatter BF = new BinaryFormatter();
                // создание объекта для форматирования

                System.Data.DataTable MT = new System.Data.DataTable();
                System.Data.DataColumn cKey = new
                    System.Data.DataColumn("Ключ"); // формирует колонку "Ключ"
                System.Data.DataColumn cInput = new
                    System.Data.DataColumn("Входящие данные(1)");
                //формируем колонку "Входные данные"
             

                System.Data.DataColumn cResult = new
                    System.Data.DataColumn("Результат"); // формируем колонку "Результат"
                MT.Columns.Add(cKey); // добавление ключа
                MT.Columns.Add(cResult); // добавление результата
                MT.Columns.Add(cInput);
              


                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    // вывод данных на экран
                    System.Data.DataRow MR; // объявили строчку
                    MR = MT.NewRow();
                    MR["Ключ"] = D.Key; // Занесение в таблицу номера 
                    MR["Входящие данные(1)"] = D.Data;
                 
                    // Занесение в таблицу входных данных
                    MR["Результат"] = D.Result; // Занесение в таблицу результата
                    MT.Rows.Add(MR);

                }
                DG.DataSource = MT;
                S.Close(); // закрытие

            }
            catch
            {
                MessageBox.Show("Ошибка файла"); // Вывод на экран сообщеня "Ошибка файла"
            }
        } // ReadFromFile закончился


        public void Generator() // метод форматирования ключевого поля
        {
            try
            {
                if (!File.Exists(this.SaveFileName)) // существует ли файл
                {
                    Key = 1;
                    return;
                }
                Stream S; // создание потока
                S = File.Open(this.SaveFileName, FileMode.Open);
                //открытие файла
                Buffer D;
                object O; // буферная переменная для контроля формата
                BinaryFormatter BF = new BinaryFormatter();
                // создание элемента для форматирования
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;
                    Key = D.Key;

                }
                Key++;
                S.Close();

            }
            catch
            {
                MessageBox.Show("Ошибка файла"); // Вывод на экран сообщения "Ошибка файла"
            }
        }

        public bool SaveFileNameExists()
        {
            if (this.SaveFileName == null)
                return false;
            else return true;
        }

        public void NewRec() // новая запись
        {
            this.Data = ""; // ""- признак пустой строки
            this.Result = null; // для бул для стринг - null
        }

        




        public void Find(string Num) // поиск
        {
            int N;
            try
            {
                N = Convert.ToInt16(Num); // преобразование номера строки в int16  дляотображения
            }
            catch
            {
                MessageBox.Show("ошибка поискового запроса");
                // Вывод на экран сообщения "ошибка поискового запроса"
                return;
            }

            try
            {
                if (!File.Exists(this.OpenFileName))
                {
                    MessageBox.Show("файла нет");
                    // вывод на экран сообщения "Прошло 5 секунд"
                    return;
                }
                Stream S; // создание потока
                S = File.Open(this.OpenFileName, FileMode.Open);
                // открытие файла
                Buffer D;
                object O; // буферная перемення для контроля формата
                BinaryFormatter BF = new BinaryFormatter();
                // создание объекта для форматирования
                while (S.Position < S.Length)
                {
                    O = BF.Deserialize(S);
                    D = O as Buffer;
                    if (D == null) break;

                    if (D.Key == N) // проверка равен ли номер поиска номеру строки в таблице
                    {
                        
                        string ST;
                        ST = "Запись содержит:" + (char)13 + "№ " + "1" +
                            "Входные данные: " + D.Data    
                            +(char)13 + "№ " + "2" + "Входные данные: " + D.Data1 +
                            (char)13 + "Результат: " + D.Result;
                        MessageBox.Show(ST, "Запись найдена");
                        // Вывод на экран сообщения "запись содержит", номер, входных данных и результат
                        S.Close();
                        return;

                    }
                }
                S.Close();
                MessageBox.Show("Запись не найдена");
                // Вывод на экран сообщения "Запись не найдена"



            }
            catch
            {
                MessageBox.Show("Ошибка файла"); // Ввывод на экран сообщения "Ошибка файла"

            }

    }
        private string SaveTextFileName; //

        public void WriteSaveTextFileName(string S)
        {
            this.SaveTextFileName = S;
        }

        public bool SaveTextFileNameExists()
        {
            if (this.SaveTextFileName == null)
                return false;
            else return true;
        }

        public void SaveToTextFile(string name, System.Windows.Forms.DataGridView D)
        {
            try
            {
                System.IO.StreamWriter textFile;
                if (!File.Exists(name))
                {
                    textFile = new System.IO.StreamWriter(name);

                }
                else
                {
                    textFile = new System.IO.StreamWriter(name, true);


                }
                for (int i = 0; i < D.RowCount - 1; i++)
                {
                    textFile.WriteLine("{0}; {1}; {2}", D[0, i].Value.ToString(), D[1,
                        i].Value.ToString(), D[2, i].Value.ToString());
                }
                textFile.Close();

                    
            }
            catch
            {
                MessageBox.Show("Ошибка работы с файлом");
            }
        }
        private string OpenTextFileName;

        public void WriteOpenTextFileName(string S)
        {
            this.OpenTextFileName = S;
        }
  }
}

