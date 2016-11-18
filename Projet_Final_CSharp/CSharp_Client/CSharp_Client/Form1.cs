﻿using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Windows.Forms;

using chatLibrary;
using System.Threading;

namespace CSharp_Client
{
    public partial class Form1 : Form
    {

        public static NetworkStream stream;
        public static TcpClient client;
        public static string login;

        public Form1()
        {
            InitializeComponent();   
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient("192.168.58.100", 1337);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Le serveur distant n'est pas connecté");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
           
            stream = client.GetStream();
        }

        private void connect_Click(object sender, EventArgs e)
        {

            AuthPacket ap = new AuthPacket(login_input.Text, password_input.Text);

            Packet.Send(ap, stream);

            Thread.Sleep(100);

            Packet paquet = Packet.Receive(stream);

            bool flag = false;

            if(paquet is LoginPacket)
            {
                LoginPacket bp = (LoginPacket)paquet;

                if (bp.value == 1)
                {
                    MessageBox.Show("Welcome " + login_input.Text, "Connexion successful !", MessageBoxButtons.OK);
                    login = login_input.Text;
                    flag = true;
                }
                else if(bp.value == 2)
                    MessageBox.Show("Wrong password ", "Connexion failed !", MessageBoxButtons.OK);
                else
                    MessageBox.Show("USer Unknown", "Connexion failed !", MessageBoxButtons.OK);
            }

            if(flag)
            {
                //Application.Run(new Form2());

                Form2 f2 = new Form2();
                f2.ShowDialog();

               /* this.Invoke(new MethodInvoker(delegate ()
                {
                    Form form2 = new Form2();


                    form2.TopMost = true;
                    form2.Show();

                    
                }));*/
                

            }
        }

        private void register_Click(object sender, EventArgs e)
        {
            SubscribePacket sb = new SubscribePacket(login_input.Text, password_input.Text);
            Packet.Send(sb, stream);

            Thread.Sleep(100);

            Packet paquet = Packet.Receive(stream);

            if(paquet is SubscribeValidation)
            {
                SubscribeValidation sv = (SubscribeValidation)paquet;

                if(sv.value)
                    MessageBox.Show("New user created !", "Registration completed", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Wrong informations", "Registration failed", MessageBoxButtons.OK);
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}