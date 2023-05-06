using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace Note_Taking_Application
{
    public partial class Form1 : Form
    {

        XmlDocument xmlDoc = new XmlDocument();

        public Form1()
        {
            InitializeComponent();


            if(File.Exists("todo.xml"))
            {
                xmlDoc.Load("todo.xml");
                XmlNodeList nodes = xmlDoc.SelectNodes("/TodoList/Item/*");

                foreach (XmlNode item in nodes)
                {
                    TodoList.Items.Add(item.Attributes["Title"].Value);
                }
            }
        }


        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if(Title.Text != "")
            {
                if (!File.Exists("todo.xml")) //File Yoksa
                {
                    TodoList.Items.Add(Title.Text);
                    XmlReader();
                }
                else //File Varsa
                {
                    xmlDoc.Load("todo.xml");
                    XmlNodeList nodes = xmlDoc.SelectNodes("/TodoList/Item/*");

                    if(TodoList.Items.Count != 0)
                    {
                        foreach (XmlNode item in nodes)
                        {
                            if (Title.Text == item.Attributes["Title"].Value)
                            {
                                XmlUpdate(item);
                            }
                            else
                            {
                                XmlAdd();
                            }
                        }
                    }
                    else
                    {
                        XmlAdd();
                    }

                }
            }
            else
            {
                MessageBox.Show("Title Cannot Be Empty","Alert");
            }

        }

        public void XmlAdd()
        {
            xmlDoc.Load("todo.xml");

            XmlElement Todo = xmlDoc.CreateElement("Todo");
            XmlAttribute name = xmlDoc.CreateAttribute("Title");
            name.Value = Title.Text;
            Todo.Attributes.Append(name);

            XmlElement message = xmlDoc.CreateElement("Message");
            message.InnerText = Message.Text;

            XmlNode todo = xmlDoc.SelectSingleNode("/TodoList/Item");
            Todo.AppendChild(message);
            todo.AppendChild(Todo);

            TodoList.Items.Add(Title.Text);
            xmlDoc.Save("todo.xml");
        }

        public void XmlUpdate(XmlNode node)
        {
            node.FirstChild.InnerText = Message.Text;
            xmlDoc.Save("todo.xml");
        }

        public void XmlReader()
        {
            XmlElement TodoList = xmlDoc.CreateElement("TodoList");
            xmlDoc.AppendChild(TodoList);

            XmlElement item = xmlDoc.CreateElement("Item");
            TodoList.AppendChild(item);

            XmlElement Todo = xmlDoc.CreateElement("Todo");
            item.AppendChild(Todo);
            XmlAttribute name = xmlDoc.CreateAttribute("Title");
            name.Value = Title.Text;
            Todo.Attributes.Append(name);

            XmlElement message = xmlDoc.CreateElement("Message");
            Todo.AppendChild(message);
            message.InnerText = Message.Text;

            xmlDoc.Save("todo.xml");

            //<TodoList>
            //  <item>
                    //<Todo title="">
            //          <Message> sgsdgsdgsdgsdg </Message>
                    //</Todo>
            //   </item>
            //</TodoList>
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            xmlDoc.Load("todo.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes("/TodoList/Item/*");

            foreach (XmlNode item in nodes)
            {
                if (item.Attributes["Title"].Value == TodoList.SelectedItem.ToString())
                {
                    item.ParentNode.RemoveChild(item);
                    xmlDoc.Save("todo.xml");
                }
            }

            TodoList.Items.Remove(TodoList.SelectedItem);
        }

        private void ReadBtn_Click(object sender, EventArgs e)
        {
            string a = TodoList.SelectedItem.ToString();
            Title.Text = a;
            xmlDoc.Load("todo.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes("/TodoList/Item/*");
            foreach (XmlNode item in nodes)
            {
                string b = item.Attributes["Title"].Value;
                if (b == a)
                {
                    Message.Text = item.FirstChild.InnerText;
                }
            }
        }
    }
}
