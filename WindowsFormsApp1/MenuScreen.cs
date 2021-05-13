using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace Движение
{
    public partial class MenuScreen : Form
    {
        PrivateFontCollection fonts;

        private void fontsProjects()
        {
            fonts = new PrivateFontCollection();
            fonts.AddFontFile(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"myFonts\galaxyGirl.ttf"));
            fonts.AddFontFile(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"myFonts\Consolas.ttf"));
            fonts.AddFontFile(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())
                .Parent.Parent.Parent.FullName.ToString(), @"myFonts\PostModernOne.ttf"));
        }

        bool closeFlag = false;
        public MenuScreen()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Size = new Size(1920, 1080);
            var label = new Label();
            label.Size = new Size(200, 50);
            label.Location = new Point(ClientSize.Width / 2 - 150, ClientSize.Height / 5 - 50);
            label.Text = "Start Menu";
            Controls.Add(label);

            var timer = new Timer();
            timer.Interval = 500;
            timer.Tick += (sender, args) =>
            {
                if (closeFlag) Close();
            };

            timer.Start();
            var button = new Button();
            button.Size = label.Size;
            button.Location = new Point(Width / 2 - 200, Height / 2 - 50);
            button.Text = "START GAME!";
            button.Click += (sender, args) =>
            {
                var mapScreen = new MapScreen();
                mapScreen.Show();
                Hide();
            };
            Controls.Add(button);
            Invalidate();
        }
    }
}
