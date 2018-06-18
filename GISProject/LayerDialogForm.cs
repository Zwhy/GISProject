using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGIS
{
    public partial class LayerDialogForm : Form
    {
        GISDocument Document;
        GISPanel MapWindow;

        public LayerDialogForm(GISDocument document, GISPanel mapwindow)
        {
            InitializeComponent();
            Document = document;
            MapWindow = mapwindow;
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < Document.layers.Count; i++)
            {
                listBox1.Items.Insert(0,Document.layers[i].Name);//使用insert，listbox中列出的顺序与图层顺序相反。
                //地图窗口中最后添加的图层往往是最后绘制的，也就是说绘制在最上面的。
            }
            if (Document.layers.Count > 0)
                listBox1.SelectedIndex = 0;//令缺省选择项为第一项。
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            GISLayer layer = Document.GetLayer(listBox1.SelectedItem.ToString());
            checkBox1.Checked = layer.Selectable;
            checkBox2.Checked = layer.Visible;
            checkBox3.Checked = layer.DrawAttributeOrNot;
            comboBox1.Items.Clear();
            for (int i = 0; i < layer.Fields.Count; i++)
            {
                comboBox1.Items.Add(layer.Fields[i].name);
            }
            comboBox1.SelectedIndex = layer.LabelIndex;
            label1.Text = layer.Path;
            textBox1.Text = layer.Name;
        }
        private void Clicked(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            GISLayer layer = Document.GetLayer(listBox1.SelectedItem.ToString());
            layer.Selectable = checkBox1.Checked;
            layer.Visible = checkBox2.Checked;
            layer.DrawAttributeOrNot = checkBox3.Checked;
            layer.LabelIndex = comboBox1.SelectedIndex;
        }
        //修改按钮
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if(i!=listBox1.SelectedIndex)
                    if (listBox1.Items[i].ToString() == textBox1.Text)
                    {
                        MessageBox.Show("不能与已有图层名重复");
                        return;
                    }
            }
            GISLayer layer = Document.GetLayer(listBox1.SelectedItem.ToString());
            layer.Name = textBox1.Text;
            listBox1.SelectedItem = textBox1.Text;
        }
        //添加按钮
        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "GIS Files(*." + GISConst.SHPFILE + ", *." + GISConst.MYFILE + ")|*." + GISConst.SHPFILE + ";*." + GISConst.MYFILE;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            GISLayer layer = Document.AddLayer(openFileDialog.FileName);
            listBox1.Items.Insert(0, layer.Name);
            listBox1.SelectedIndex = 0;
        }
        //删除按钮
        private void btnRemoveMap_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            Document.RemoveLayer(listBox1.SelectedItem.ToString());
            listBox1.Items.Remove(listBox1.SelectedItem);
            if (listBox1.Items.Count > 0) listBox1.SelectedIndex = 0;
        }
        //上移
        private void btnMoveUpMap_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            //当前选择无法上移
            if (listBox1.SelectedIndex == 0) return;
            //当前图层名
            string seletedname = listBox1.SelectedItem.ToString();
            //需要调换的图层名
            string uppername = listBox1.Items[listBox1.SelectedIndex - 1].ToString();
            //在listBox1中完成调换
            listBox1.Items[listBox1.SelectedIndex - 1] = seletedname;
            listBox1.Items[listBox1.SelectedIndex] = uppername;
            //在Document中完成调换
            Document.SwitchLayer(seletedname, uppername);
            listBox1.SelectedIndex--;
        }
        //图层下移
        private void btnMoveDownMap_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            if (listBox1.Items.Count == 1) return;
            if (listBox1.SelectedIndex == listBox1.Items.Count - 1) return;//选择的是最后一项
            string selectedname = listBox1.SelectedItem.ToString();
            string lowername = listBox1.Items[listBox1.SelectedIndex + 1].ToString();
            listBox1.Items[listBox1.SelectedIndex + 1] = selectedname;
            listBox1.Items[listBox1.SelectedIndex] = lowername;
            Document.SwitchLayer(selectedname, lowername);
            listBox1.SelectedIndex++;
        }
        //导出图层
        private void btnExportLayer_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GIS file(*." + GISConst.MYFILE + ")|*." + GISConst.MYFILE;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                GISLayer layer = Document.GetLayer(listBox1.SelectedItem.ToString());
                GISMyFile.WriteFile(layer, saveFileDialog.FileName);
                MessageBox.Show("Done!");
            }
        }
        //存储文档
        private void btnSaveDocument_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GIS Document (*." + GISConst.MYDOC + ")|*." + GISConst.MYDOC;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = false;
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Document.Write(saveFileDialog.FileName);
                MessageBox.Show("Done!");
            }
        }
        //打开属性表
        private void btnAttributeTable_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            GISLayer layer = Document.GetLayer(listBox1.SelectedItem.ToString());
            MapWindow.OpenAttributeWindow(layer);
        }
        //关闭窗口
        private void btnClose_Click(object sender, EventArgs e)
        {
            MapWindow.UpdateMap();
            Close();
        }
        //应用按钮
        private void btnApply_Click(object sender, EventArgs e)
        {
            MapWindow.UpdateMap();
        }
    }
}
