using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace PracticaRSA
{
    public partial class frmDesencriptar : Form
    {
        string fileName = "";
        string filePath = @"C:\";
        string fileContent = "";

        RSACryptoServiceProvider rsa;
        UnicodeEncoding ByteConverter = new UnicodeEncoding();

        byte[] encryptedText;

        public frmDesencriptar()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tbx_decrypted_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbx_crypted_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btn_decrypt_Click(object sender, EventArgs e)
        {
            string cryptedText = tbx_crypted.Text;
            //tbx_crypted.Text = frmEncriptar.encryptedMessage;
            encryptedText = frmEncriptar.encryptedArray;
            //desencriptar el mensaje cifrado
            byte[] decryptedtex = Decryption(encryptedText,
            rsa.ExportParameters(true), false);
            //convertir array de bytes en string
            tbx_decrypted.Text = ByteConverter.GetString(decryptedtex);
        }

        private void pnl_keys_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_routeXML_Click(object sender, EventArgs e)
        {
            //establecer una ruta

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

                    tbx_routeXML.Text = filePath;
                   //MessageBox.Show("File " + fileName + " on hold.");
                }
            }
        }

        private void btn_generate_Click(object sender, EventArgs e)
        {
            string keycontainer = tbx_container.Text;
            string routeXML = tbx_routeXML.Text;
            
            //Creo contenedor de claves
            CspParameters cspp = new CspParameters();
            cspp.KeyContainerName = keycontainer;
            //Crear las claves, pasándole keycontainer como parámetro
            rsa = new RSACryptoServiceProvider(cspp);
            
            //Persisto clave privada
            rsa.PersistKeyInCsp = true;
            string publicKey = rsa.ToXmlString(false);

            //Guardar clave pública en fichero especificado
            File.WriteAllText(routeXML, publicKey);
            
        }

        private void frmDesencriptar_Activated(object sender, EventArgs e)
        {
            tbx_crypted.Text = frmEncriptar.encryptedMessage;
        }

        public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;

                decryptedData = rsa.Decrypt(Data, DoOAEPPadding);
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
