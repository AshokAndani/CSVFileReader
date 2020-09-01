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

namespace CSVReader
{
    public partial class Form1 : Form
    {
        private string[] ColumnLabels;
        private List<string[]> Data;
        private HashSet<string> DropDownData;
        private DataTable Table;

        public Form1()
        {
            InitializeComponent();
            DropDownBox.Enabled = false;
            DropDownData = new HashSet<string>();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            textBox1.Text = fileDialog.FileName;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Table = new DataTable();
            Data = new List<string[]>();
            if (DropDownBox.Enabled == false)
            {
                DropDownBox.Enabled = true;
            }
            var fileData = File.ReadAllLines(textBox1.Text);
            ColumnLabels = fileData[0].TrimEnd(',').Split(',');
            foreach (string data in ColumnLabels)
            {
                Table.Columns.Add(new DataColumn(data));
            }

            //// Filling the Data into the List
            for (int i = 1; i < fileData.Length; i++)
            {
                var rowData = fileData[i].TrimEnd(',').Split(',');
                string[] tempArray = new string[rowData.Length];
                for (int j = 0; j < rowData.Length; j++)
                {
                    tempArray[j] = rowData[j];
                    if (j == 1)
                    {
                        DropDownData.Add(rowData[j]);
                    }
                }
                Data.Add(tempArray);
            }
            FillGrid(Data, DropDownBox.Text);
        }
        private void FillGrid(List<string[]> data, string Filter)
        {
            List<string[]> tempList;
            if (Filter == "FilterBy")
            {
                tempList = Data;
            }
            else
            {
                tempList = Data.Where(q => q[1]==Filter).ToList();
                
            }

            foreach (string[] rowArray in tempList)
            {
                DataRow dataRow = Table.NewRow();
                for (int i = 0; i < rowArray.Length; i++)
                {
                    dataRow[ColumnLabels[i]] = rowArray[i];
                }
                Table.Rows.Add(dataRow);
            }
            dataGridView1.DataSource = Table;
            int rowNum = 0;
            foreach(string[] rowArray in tempList)
            {
                var last = double.Parse(rowArray[6]);
                var prev = double.Parse(rowArray[7]);
                if (last>prev)
                {
                    dataGridView1.Rows[rowNum++].Cells[0].Style.BackColor = Color.Green;
                    ////dataGridView1.Rows[rowNum++].DefaultCellStyle.BackColor = Color.Green;
                }
                else if(last < prev)
                {
                    dataGridView1.Rows[rowNum++].Cells[0].Style.BackColor = Color.Red;
                    ////dataGridView1.Rows[rowNum++].DefaultCellStyle.BackColor = Color.Red;
                }
                else if(last == prev)
                {
                    dataGridView1.Rows[rowNum++].Cells[0].Style.BackColor = Color.Yellow;
                    ////dataGridView1.Rows[rowNum++].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            foreach(var someData in DropDownData)
            {
                DropDownBox.Items.Add(someData);
            }
        }
    }
}
