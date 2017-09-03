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
using MikeHelper;

namespace Adventure_Save_Editor
{
    public partial class Main : Form
    {
        public Dictionary<string, string> SaveData = new Dictionary<string, string>();
        public string CurrentFile;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var filePath = Environment.ExpandEnvironmentVariables("%USERPROFILE%") + "/AppData/LocalLow/Mike Inel/AT 3D AG/SaveData.dat";
            if (File.Exists(filePath))
                LoadFile(filePath);
        }

        private void LoadFile(string path)
        {
            // Create Backup Save
            var backupPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".bak";
            File.Copy(path, backupPath, true);

            // Clear Old Dictionary
            SaveData.Clear();

            using (var sr = new StreamReader(path))
            {
                try
                {
                    for (var rhs = MikeCypher.Cypher(-69, sr.ReadLine()); !string.IsNullOrEmpty(rhs); rhs = MikeCypher.Cypher(-69, sr.ReadLine()))
                    {
                        string[] data = rhs.Split(' ');
                        if (data.Length >= 2)
                            SaveData.Add(data[0], data[1]);
                        else
                            MessageBox.Show(rhs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }
            }

            foreach (var entry in SaveData)
            {
                dgvSaveData.Rows.Add(entry.Key, entry.Value);
            }

            CurrentFile = path;
        }

        private void SaveFile(string path)
        {
            using (var sr = new StreamWriter(path))
            {
                int num1 = 0;
                int num2 = 0;
                for (int i = 0; i < dgvSaveData.Rows.Count - 1; ++i)
                {
                    string key = dgvSaveData.Rows[i].Cells[0].Value.ToString();
                    string value = dgvSaveData.Rows[i].Cells[1].Value.ToString();
                    string line = MikeCypher.Cypher(69, key + " " + value);

                    if (i > 2)
                    {
                        if (num2 == 0)
                            num1 += Convert.ToInt32(value);
                        if (num2 == 1)
                            num1 -= Convert.ToInt32(value);
                        if (num2 == 2)
                            num1 *= Convert.ToInt32(value) + 1;
                        if (num2 == 3)
                            num1 *= Convert.ToInt32(value) + 2;

                        if (num1 == 0)
                            num1 = num2 + 1;

                        num2++;

                        if (num2 > 3)
                            num2 = 0;
                    }

                    sr.WriteLine(line);
                }

                string validity = "VALID: " + num1.ToString();
                sr.WriteLine(MikeCypher.Cypher(69, validity));
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Data File | *.dat";
            var savePath = Environment.ExpandEnvironmentVariables("%USERPROFILE%") + "/AppData/LocalLow/Mike Inel/AT 3D AG/";

            if (Directory.Exists(savePath))
                ofd.InitialDirectory = savePath;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadFile(ofd.FileName);
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            sfd.Filter = "Data File | *.dat";
            var savePath = Environment.ExpandEnvironmentVariables("%USERPROFILE%") + "/AppData/LocalLow/Mike Inel/AT 3D AG/";

            if (Directory.Exists(savePath))
                sfd.InitialDirectory = savePath;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveFile(sfd.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will override your current save file, is this okay?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SaveFile(CurrentFile);
            }
        }
    }
}
