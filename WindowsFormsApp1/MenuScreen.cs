using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace Движение
{
    public partial class MenuScreen : Form
    {
        public MenuScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            Size = new Size(1920, 1080);
            var label = new Label();
            label.Size = new Size(200, 50);
            label.Location = new Point(ClientSize.Width / 2, ClientSize.Height / 5);
            label.Text = "Start Menu";
            Controls.Add(label);

            var button = new Button();
            button.Size = label.Size;
            button.Location = new Point(ClientSize.Width / 2, 4 * ClientSize.Height / 5);
            button.Text = "START GAME!";
            button.Click += (sender, args) =>
              {
                  var mapScreen = new MapScreen();
                  mapScreen.Show();
                  this.Hide();
              };
            Controls.Add(button);
        }
    }
}
