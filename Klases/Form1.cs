using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace klases
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Klases by VG";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ************ 1. uzdevums ****************
            addText("1. uzdevums");
            Child me = new Child();
            Child you = new Child("Valentins", "Gilnics", 14);
            Pupil he = new Pupil("Donald", "Trump", 8, "Riga skola", "4");
            Student she = new Student("J", "Lo", 22, "Latvijas Universitet", "IT", "2");
            me.DoJob();
            you.DoJob();
            he.DoJob();
            he.DoJob();
            she.DoJob();
            addText(you.ToString());
            addText(me.ToString());
            addText(he.ToString());
            addText(she.ToString());

            addText("");

            // ************ 2. uzdevums ****************
            addText("2. uzdevums");
            List<Child> kids2 = getRandomKids(10);
            foreach (Child kid in kids2)
            {
                addText(kid.ToString());
            }

        }

        //Generate list of random kids
        public List<Child> getRandomKids(int count)
        {
            List<Child> childs = new List<Child>();
            var generator = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 1; i < count + 1; i++)
            {
                System.Threading.Thread.Sleep(10); //Sleep thread to get random numbers
                int num = generator.Next(200);
                if (num < 51)
                {
                    //Get random Child
                    childs.Add(new Child(i));
                }
                else if (num > 80)
                {
                    //Get random Student
                    childs.Add(new Student(i));
                }
                else
                {
                    //Get random Pupil
                    childs.Add(new Pupil(i));
                }

            }
            return childs;
        }

        //Add text to TextBox
        public void addText(String str)
        {
            textBox1.Text = textBox1.Text + str + Environment.NewLine;
        }

        public class Child
        {
            public String Name;
            public String Surname;
            public int Age;

            public Child()
            {
                Name = "?";
                Surname = "?";
                Age = 0;
            }

            public Child(String aName, String aSurname, int aAge)
            {
                Name = aName;
                Surname = aSurname;
                Age = aAge;
            }

            override
            public String ToString()
            {
                return this.GetType().Name + " " + this.Name + " " + this.Surname + " is " + this.Age + " years old";
            }

            public void DoJob()
            {
                this.Age++;
            }

            //Generate random child
            public Child(int aId)
            {
                var generator = new Random();
                Name = this.GetType().Name + aId;
                Surname = this.getRandomString(aId, aId + 5);
                Age = generator.Next(1, 7);
            }

            //Generate random text string
            public String getRandomString(int a, int b)
            {
                String res;
                var chars = "abcdefghijklmnopqrstuvwxyz";
                var random = new Random(Guid.NewGuid().GetHashCode()); //Guid.NewGuid().GetHashCode() - get always random

                var stringChars = new char[random.Next(a,b)];

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                res = new String(stringChars);
                res = char.ToUpper(res[0]) + res.Substring(1);
                return res;
            }

        }

        public class Pupil : Child
        {
            public String School;
            public String Class;

            public Pupil(String aName, String aSurname, int aAge, String aSchool, String aClass)
            {
                Name = aName;
                Surname = aSurname;
                Age = aAge;
                School = aSchool;
                Class = aClass;
            }

            override
            public String ToString()
            {
                return this.GetType().Name + " " + this.Name + " " + this.Surname + " is " + this.Age + " years old and goes to " + this.School + " in " + this.Class + " class";
            }

            //Generate random pupil
            public Pupil(int aId)
            {
                var generator = new Random();
                Name = this.GetType().Name + aId;
                Surname = this.getRandomString(aId, aId + 5);
                Age = generator.Next(6, 20);
                School = this.getRandomString(aId, aId + 5) + " Skola";
                Class = generator.Next(1, 12).ToString();
            }

        }

        public class Student : Child
        {
            public String HiSchool;
            public String Faculty;
            public String Year;

            public Student(String aName, String aSurname, int aAge, String aHiSchool, String aFaculty, String aYear)
            {
                Name = aName;
                Surname = aSurname;
                Age = aAge;
                HiSchool = aHiSchool;
                Faculty = aFaculty;
                Year = aYear;
            }

            override
            public String ToString()
            {
                return this.GetType().Name + " " + this.Name + " " + this.Surname + " is " + this.Age + " years old and goes to " + this.HiSchool + " on faculty " + this.Faculty + " at level " + this.Year;
            }

            //Generate random student
            public Student(int aId)
            {
                var generator = new Random();
                Name = this.GetType().Name + aId;
                Surname = this.getRandomString(aId, aId + 5);
                Age = generator.Next(18, 70);
                HiSchool = this.getRandomString(aId, aId + 5) + " Universitate";
                Faculty = this.getRandomString(aId, aId + 5) + " Fakultate";
                Year = generator.Next(1, 4).ToString();
            }

        }

    }
}
