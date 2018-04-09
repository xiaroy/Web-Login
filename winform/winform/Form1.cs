﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;


namespace winform
{
        public partial class Form1 : Form
        {

     

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'royDataSet.Logins' table. You can move, or remove it, as needed.
            this.loginsTableAdapter.Fill(this.royDataSet.logins);

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void smtpPasswordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.loginsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.royDataSet);

        }

        private void loginsBindingNavigator_RefreshItems(object sender, EventArgs e)
        {
        
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
        
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void registerButton_Click_1(object sender, EventArgs e)
        {

        }

        private void AddUser(string username, string password, string confirmPass, string email)
        {
            //Local variables to hold values
            string smtpEmail = smtpUserNameTextBox.Text;
            string smtpPassword = smtpPasswordTextBox.Text;
            int smtpPort = (int)smtpPortNumericUD.Value;
            string smtpAddress = smtpAddressTextBox.Text;

            //Regex for making sure Email is valid
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            //Loop through Logins Table
            foreach (DataRow row in royDataSet.logins)
            {
                //And look for matching usernames
                if (row.ItemArray[0].Equals(username))
                {
                    //If one is found, show message:
                    MessageBox.Show("Username already exists");
                    return;
                }
            }

            //Confirm pass must equal password.
            if (password != confirmPass)
            {
                MessageBox.Show("Passwords do not match");
            }
            //Password must be at least 8 characters long
            else if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long");
            }
            //If email is NOT valid
            else if (!match.Success)
            {
                MessageBox.Show("Invalid Email");
            }
            //If there is no username
            else if (username == null)
            {
                MessageBox.Show("Must have Username");
            }
            //If all is well, create the new user!
            else
            {
                RoyDataSet.loginsRow newUserRow = royDataSet.logins.NewloginsRow(); 

                string EncryptedPass = HashPass(password);
                newUserRow.Username = username;
                newUserRow.Password = EncryptedPass;
                newUserRow.Email = email;

                royDataSet.logins.Rows.Add(newUserRow);
                registerUserName.Text = String.Empty;
                registerPassword.Text = String.Empty;
                registerConfirmPassword.Text = String.Empty;
                registerEmail.Text = String.Empty;
                MessageBox.Show("Thank you for Registering!");
                if (String.IsNullOrWhiteSpace(smtpEmail) || String.IsNullOrWhiteSpace(smtpPassword) || String.IsNullOrWhiteSpace(smtpAddress) || smtpPort <= 0)
                {
                    MessageBox.Show("Email configuration is not set up correctly! \nCannot sent email!");

                }
                else
                {
                    SendMessage(email.ToString(), username.ToString(), password.ToString());
                }
            }
        }

        public string HashPass(string password)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();

            //compute hash from the bytes of text
            sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it
            byte[] result = sha.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }


        public void SendMessage(string ToAddress, string ToName, string password)
        {

            var client = new SmtpClient(smtpAddressTextBox.Text, (int)smtpPortNumericUD.Value)
            {
                Credentials = new NetworkCredential(smtpUserNameTextBox.Text, smtpPasswordTextBox.Text),
                EnableSsl = true
            };
            client.Send(smtpUserNameTextBox.Text, ToAddress, "Thank You!", "Thank you for registering with us today! \n Your username/passwords are: \n \nUsername: "
                                                                        + ToName.ToString() + "\nPassword: " + password.ToString());
        }

        public void registerButton_Click(object sender, EventArgs e)
        {
            AddUser(registerUserName.Text, registerPassword.Text, registerConfirmPassword.Text, registerEmail.Text);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //Locals to hold values
            string username = loginUserName.Text;
            string password = HashPass(loginPassword.Text);


            //Loop through database
            foreach (DataRow row in royDataSet.logins)
            {
                //And search for Username and Pass that match
                if (row.ItemArray[0].Equals(username) && row.ItemArray[1].Equals(password))
                {
                    loginUserName.Text = String.Empty;
                    loginPassword.Text = String.Empty;
                    MessageBox.Show("Login Success");
                    break;
                }
                //If not, then show this message.
                else
                {
                    MessageBox.Show("Username/Password incorrect");
                    break;
                }
            }
        } 



        public Form1()
        {
            InitializeComponent();
        }

        private void bindingNavigatorCountItem_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
