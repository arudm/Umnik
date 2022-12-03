namespace Umnik
{
    partial class DronesForm
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
            this.checkedListBoxOfDrones = new System.Windows.Forms.CheckedListBox();
            this.btnAddDrone = new System.Windows.Forms.Button();
            this.btnRemoveDrone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBoxOfDrones
            // 
            this.checkedListBoxOfDrones.FormattingEnabled = true;
            this.checkedListBoxOfDrones.Location = new System.Drawing.Point(12, 12);
            this.checkedListBoxOfDrones.Name = "checkedListBoxOfDrones";
            this.checkedListBoxOfDrones.Size = new System.Drawing.Size(327, 94);
            this.checkedListBoxOfDrones.TabIndex = 0;
            // 
            // btnAddDrone
            // 
            this.btnAddDrone.Location = new System.Drawing.Point(345, 12);
            this.btnAddDrone.Name = "btnAddDrone";
            this.btnAddDrone.Size = new System.Drawing.Size(75, 23);
            this.btnAddDrone.TabIndex = 1;
            this.btnAddDrone.Text = "Добавить";
            this.btnAddDrone.UseVisualStyleBackColor = true;
            this.btnAddDrone.Click += new System.EventHandler(this.btnAddDrone_Click);
            // 
            // btnRemoveDrone
            // 
            this.btnRemoveDrone.Location = new System.Drawing.Point(345, 41);
            this.btnRemoveDrone.Name = "btnRemoveDrone";
            this.btnRemoveDrone.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveDrone.TabIndex = 2;
            this.btnRemoveDrone.Text = "Удалить";
            this.btnRemoveDrone.UseVisualStyleBackColor = true;
            this.btnRemoveDrone.Click += new System.EventHandler(this.btnRemoveDrone_Click);
            // 
            // DronesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 112);
            this.Controls.Add(this.btnRemoveDrone);
            this.Controls.Add(this.btnAddDrone);
            this.Controls.Add(this.checkedListBoxOfDrones);
            this.Name = "DronesForm";
            this.Text = "Drones";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxOfDrones;
        private System.Windows.Forms.Button btnAddDrone;
        private System.Windows.Forms.Button btnRemoveDrone;
    }
}