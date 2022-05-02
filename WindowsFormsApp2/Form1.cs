using Amazon.Rekognition;
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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        struct Bounds { public int Top; public int Left; public int Width; public int Height; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Images|*.png;*.jpg;*.jpeg";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            var bitmap = new Bitmap(openFileDialog1.FileName);
            ResizeAll(bitmap);
            pictureBox1.Image = bitmap;
        }

        private void ResizeAll(Bitmap image)
        {
            pictureBox1.Width = image.Width;
            pictureBox1.Height = image.Height;

            this.Width = image.Width + 43;
            this.Height = image.Height + 120;

            this.button1.Width = this.Width / 2 - 26;
            this.button2.Width = this.Width / 2 - 26;

            this.button1.Location = new Point(button1.Location.X, this.Height - 94);
            this.button2.Location = new Point(this.Width / 2, this.Height - 94);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(aws.accesstoken, aws.secretkey);
            var client = new AmazonRekognitionClient(awsCreden‌​tials, Amazon.RegionEndpoint.EUWest2);


            MemoryStream memoryStream = new MemoryStream();
            pictureBox1.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            List<Bounds> bounds = new List<Bounds>();

            client.DetectFaces(new Amazon.Rekognition.Model.DetectFacesRequest()
            {
                Image = new Amazon.Rekognition.Model.Image()
                {
                    Bytes = memoryStream
                }
            }).FaceDetails.ForEach(x =>
            {
                bounds.Add(new Bounds()
                {
                    Top = (int)(x.BoundingBox.Top * pictureBox1.Height),
                    Left = (int)(x.BoundingBox.Left * pictureBox1.Width),
                    Height = (int)(x.BoundingBox.Height * pictureBox1.Height),
                    Width = (int)(x.BoundingBox.Width * pictureBox1.Width)
                });
            });

            Graphics g = pictureBox1.CreateGraphics();

            Brush brush = new SolidBrush(Color.YellowGreen);
            Pen pen = new Pen(brush, 2);

            bounds.ForEach(x =>
            {
                g.DrawRectangle(pen, new Rectangle(x.Left, x.Top, x.Width, x.Height));
            });
        }
    }
}
