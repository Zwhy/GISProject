﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyGIS;

namespace GISProject
{
    public partial class Form2 : Form
    {
        Form1 MapWindow = null;
        GISLayer Layer;
        bool FromMapWindow = true;
        public Form2(GISLayer layer, Form1 mapWindow)
        {
            InitializeComponent();
            Layer = layer;
            MapWindow = mapWindow;
        }

        private void FillValue()
        {
            //增加ID列
            dataGridView1.Columns.Add("ID", "ID");
            //增加其他列
            for (int i = 0; i < Layer.Fields.Count; i++)
            {
                dataGridView1.Columns.Add(Layer.Fields[i].name, Layer.Fields[i].name);
            }
            for (int i = 0; i < Layer.FeatureCount(); i++)
            {
                dataGridView1.Rows.Add();
                //增加ID值
                dataGridView1.Rows[i].Cells[0].Value = Layer.GetFeature(i).ID;
                //增加其他属性值
                for(int j = 0; j < Layer.Fields.Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j + 1].Value = Layer.GetFeature(i).GetAttribute(j);
                }
                //确定每行的选择状态
                dataGridView1.Rows[i].Selected = Layer.GetFeature(i).Selected;
            }
        }
        private void Form2_Shown(object sender, EventArgs e)
        {
            FromMapWindow = true;
            FillValue();
            FromMapWindow = false;
        }
        public void UpdateData()
        {
            FromMapWindow = true;
            dataGridView1.ClearSelection();
            foreach (GISFeature feature in Layer.Selection)
                SelectRowByID(feature.ID).Selected = true;
            FromMapWindow = false;
        }
        public DataGridViewRow SelectRowByID(int ID)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
                if ((int)(row.Cells[0].Value) == ID) return row;
            return null;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //如果是来自地图窗口的就不用继续了
            if (FromMapWindow) return;
            //如果两个窗口的当前选择及都是空，不用继续
            if (Layer.Selection.Count == 0 && dataGridView1.SelectedRows.Count == 0) return;
            //更新地图穿个选择集
            Layer.ClearSelection();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                //有时表格最后一行是空值也有可能被选中，所以需要空值检验
                if (row.Cells[0].Value != null) Layer.AddSelectedFeatByID((int)(row.Cells[0].Value));
            }
            MapWindow.UpdateMap();
        }
    }
}
