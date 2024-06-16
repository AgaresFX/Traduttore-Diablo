using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Traduttore_Diablo4
{
    public partial class Form1 : Form
    {
        private List<string> allAspects;
        private Dictionary<string, string> aspectTranslations;
        public Form1()
        {
            InitializeComponent();
            InitializeData();
            SetupComboBox();
        }

        private void InitializeData()
        {
            aspectTranslations = new Dictionary<string, string>();
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Json");
            string[] jsonFiles = {
                "Aspetti.json",
                "Glifi.json",
                "Nodi_Glifi.json",
                "Barbaro.json",
                "Tagliagole.json",
                "Druido.json",
                "Incantatrice.json",
                "Negromante.json",
                "Unici.json"
            };
            foreach (var fileName in jsonFiles)
            {
                string filePath = Path.Combine(directoryPath, fileName);
                if (File.Exists(filePath))
                {
                    try
                    {
                        string jsonContent = File.ReadAllText(filePath);
                        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
                        foreach (var kvp in dictionary)
                        {
                            aspectTranslations[kvp.Key] = kvp.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Errore nel caricamento del file {fileName}: {ex.Message}", "Errore di Caricamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Il file {filePath} non esiste nel percorso specificato.", "File Mancante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Ottieni tutti gli aspetti
            allAspects = aspectTranslations.Keys.ToList();
        }

        private void SetupComboBox()
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.TextUpdate += ComboBox1_TextUpdate;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            comboBox1.Items.AddRange(allAspects.ToArray());
        }

        private void ComboBox1_TextUpdate(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            string searchText = comboBox.Text.ToLower();
            var filteredAspects = allAspects.Where(a => a.ToLower().Contains(searchText)).ToArray();
            string currentText = comboBox.Text;
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(filteredAspects);
            comboBox1.Text = currentText;
            comboBox1.SelectionStart = currentText.Length;
            comboBox1.SelectionLength = 0;
            comboBox1.DroppedDown = true;
            comboBox1.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.SelectedItem != null)
            {
                string selectedAspect = comboBox.SelectedItem.ToString();
                if (aspectTranslations.TryGetValue(selectedAspect, out var translation))
                {
                    txtTraduttore.Text = translation;
                }
                else
                {
                    txtTraduttore.Text = "Traduzione non trovata";
                }
            }
            else
            {
                txtTraduttore.Text = "Seleziona un aspetto";
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr uno, int due, int tre, int quattro);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            RelaseCapture();
        }

        private void RelaseCapture()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }
    }
}
