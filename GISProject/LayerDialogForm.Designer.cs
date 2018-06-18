﻿namespace MyGIS
{
    partial class LayerDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnAddLayer = new System.Windows.Forms.Button();
            this.btnRemoveMap = new System.Windows.Forms.Button();
            this.btnMoveUpMap = new System.Windows.Forms.Button();
            this.btnMoveDownMap = new System.Windows.Forms.Button();
            this.btnExportLayer = new System.Windows.Forms.Button();
            this.btnSaveDocument = new System.Windows.Forms.Button();
            this.btnAttributeTable = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnInsideColor = new System.Windows.Forms.Button();
            this.btnOutsideColor = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(29, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(662, 274);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(29, 338);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 19);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "可选择";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(29, 396);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(59, 19);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "可视";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(29, 450);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(89, 19);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "自动标注";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(149, 446);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(241, 23);
            this.comboBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(381, 342);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "文件地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(381, 393);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "图层名称：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(459, 383);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 25);
            this.textBox1.TabIndex = 7;
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(726, 382);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(84, 23);
            this.btnModify.TabIndex = 8;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAddLayer
            // 
            this.btnAddLayer.Location = new System.Drawing.Point(315, 307);
            this.btnAddLayer.Name = "btnAddLayer";
            this.btnAddLayer.Size = new System.Drawing.Size(75, 23);
            this.btnAddLayer.TabIndex = 9;
            this.btnAddLayer.Text = "添加图层";
            this.btnAddLayer.UseVisualStyleBackColor = true;
            this.btnAddLayer.Click += new System.EventHandler(this.btnAddLayer_Click);
            // 
            // btnRemoveMap
            // 
            this.btnRemoveMap.Location = new System.Drawing.Point(405, 307);
            this.btnRemoveMap.Name = "btnRemoveMap";
            this.btnRemoveMap.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveMap.TabIndex = 10;
            this.btnRemoveMap.Text = "删除图层";
            this.btnRemoveMap.UseVisualStyleBackColor = true;
            this.btnRemoveMap.Click += new System.EventHandler(this.btnRemoveMap_Click);
            // 
            // btnMoveUpMap
            // 
            this.btnMoveUpMap.Location = new System.Drawing.Point(735, 59);
            this.btnMoveUpMap.Name = "btnMoveUpMap";
            this.btnMoveUpMap.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUpMap.TabIndex = 11;
            this.btnMoveUpMap.Text = "上移";
            this.btnMoveUpMap.UseVisualStyleBackColor = true;
            this.btnMoveUpMap.Click += new System.EventHandler(this.btnMoveUpMap_Click);
            // 
            // btnMoveDownMap
            // 
            this.btnMoveDownMap.Location = new System.Drawing.Point(735, 101);
            this.btnMoveDownMap.Name = "btnMoveDownMap";
            this.btnMoveDownMap.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDownMap.TabIndex = 12;
            this.btnMoveDownMap.Text = "下移";
            this.btnMoveDownMap.UseVisualStyleBackColor = true;
            this.btnMoveDownMap.Click += new System.EventHandler(this.btnMoveDownMap_Click);
            // 
            // btnExportLayer
            // 
            this.btnExportLayer.Location = new System.Drawing.Point(498, 307);
            this.btnExportLayer.Name = "btnExportLayer";
            this.btnExportLayer.Size = new System.Drawing.Size(75, 23);
            this.btnExportLayer.TabIndex = 13;
            this.btnExportLayer.Text = "导出图层";
            this.btnExportLayer.UseVisualStyleBackColor = true;
            this.btnExportLayer.Click += new System.EventHandler(this.btnExportLayer_Click);
            // 
            // btnSaveDocument
            // 
            this.btnSaveDocument.Location = new System.Drawing.Point(588, 307);
            this.btnSaveDocument.Name = "btnSaveDocument";
            this.btnSaveDocument.Size = new System.Drawing.Size(75, 23);
            this.btnSaveDocument.TabIndex = 14;
            this.btnSaveDocument.Text = "存储文档";
            this.btnSaveDocument.UseVisualStyleBackColor = true;
            this.btnSaveDocument.Click += new System.EventHandler(this.btnSaveDocument_Click);
            // 
            // btnAttributeTable
            // 
            this.btnAttributeTable.Location = new System.Drawing.Point(735, 177);
            this.btnAttributeTable.Name = "btnAttributeTable";
            this.btnAttributeTable.Size = new System.Drawing.Size(75, 23);
            this.btnAttributeTable.TabIndex = 15;
            this.btnAttributeTable.Text = "打开属性表";
            this.btnAttributeTable.UseVisualStyleBackColor = true;
            this.btnAttributeTable.Click += new System.EventHandler(this.btnAttributeTable_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(571, 445);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 16;
            this.btnApply.Text = "应用";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(691, 445);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.btnOutsideColor);
            this.groupBox1.Controls.Add(this.btnInsideColor);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(865, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(388, 605);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "显示设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "label5";
            // 
            // btnInsideColor
            // 
            this.btnInsideColor.Location = new System.Drawing.Point(133, 43);
            this.btnInsideColor.Name = "btnInsideColor";
            this.btnInsideColor.Size = new System.Drawing.Size(231, 23);
            this.btnInsideColor.TabIndex = 3;
            this.btnInsideColor.UseVisualStyleBackColor = true;
            this.btnInsideColor.Click += new System.EventHandler(this.SettingColor_Click);
            // 
            // btnOutsideColor
            // 
            this.btnOutsideColor.Location = new System.Drawing.Point(133, 146);
            this.btnOutsideColor.Name = "btnOutsideColor";
            this.btnOutsideColor.Size = new System.Drawing.Size(231, 23);
            this.btnOutsideColor.TabIndex = 4;
            this.btnOutsideColor.UseVisualStyleBackColor = true;
            this.btnOutsideColor.Click += new System.EventHandler(this.SettingColor_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(133, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(231, 25);
            this.textBox2.TabIndex = 5;
            this.textBox2.TextChanged += new System.EventHandler(this.Clicked);
            // 
            // LayerDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 654);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnAttributeTable);
            this.Controls.Add(this.btnSaveDocument);
            this.Controls.Add(this.btnExportLayer);
            this.Controls.Add(this.btnMoveDownMap);
            this.Controls.Add(this.btnMoveUpMap);
            this.Controls.Add(this.btnRemoveMap);
            this.Controls.Add(this.btnAddLayer);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.listBox1);
            this.Name = "LayerDialogForm";
            this.Text = "Form3";
            this.Shown += new System.EventHandler(this.Form3_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnAddLayer;
        private System.Windows.Forms.Button btnRemoveMap;
        private System.Windows.Forms.Button btnMoveUpMap;
        private System.Windows.Forms.Button btnMoveDownMap;
        private System.Windows.Forms.Button btnExportLayer;
        private System.Windows.Forms.Button btnSaveDocument;
        private System.Windows.Forms.Button btnAttributeTable;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnOutsideColor;
        private System.Windows.Forms.Button btnInsideColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}