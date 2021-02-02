using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PracticaRSA
{
    public partial class frmEncriptar : Form
    {
        string fileName = "";
        string filePath = @"C:\";
        string fileContent = "";

        public static string encryptedMessage;
        public static byte[] encryptedArray;

        RSACryptoServiceProvider rsaEnc = new RSACryptoServiceProvider();

        public frmEncriptar()
        {
            InitializeComponent();
        }

        private void btn_obtainKey_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "Xml files (*.xml)|*.xml" + "|" + "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    fileName = openFileDialog.SafeFileName;
                    Stream fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    //MessageBox.Show("File " + fileName + " on hold.");
                }
            }


            string xmlKey = File.ReadAllText(filePath);
            rsaEnc.FromXmlString(xmlKey);
        }

        private void btn_encrypt_Click(object sender, EventArgs e)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] plaintext;
            byte[] encryptedtext;

            //tbx_original - mensaje a encriptar 
            //tbx_crypted - mensaje encriptado

            //convertir el texto plano en un un array de bytes 
            plaintext = ByteConverter.GetBytes(tbx_original.Text);
            //encriptar
            encryptedtext = Encryption(plaintext, rsaEnc.ExportParameters(false), false);
            encryptedArray = encryptedtext;
            //convertir a texto para mostrarlo en textbox
            tbx_crypted.Text = ByteConverter.GetString(encryptedtext);         
            
        }
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private void btn_showKey_Click(object sender, EventArgs e)
        {
            tbx_pubkey.Text = GetXmlString(filePath);
        }

        static string GetXmlString(string strFile)
        {
            // Load the xml file into XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(strFile);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
            }
            // Now create StringWriter object to get data from xml document.
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xmlDoc.WriteTo(xw);
            return sw.ToString();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            //guardar texto en una variable para que lo lea el otro form
            encryptedMessage = tbx_crypted.Text;
        }
    }
}
