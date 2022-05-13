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
        MemoryStream memoryStream;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Images|*.png;*.jpg;*.jpeg";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            var bitmap = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bitmap;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            memoryStream = new MemoryStream(File.ReadAllBytes(openFileDialog1.FileName));
        }

        private void ResizeAll(Bitmap image)
        {


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

            List<Bounds> bounds = new List<Bounds>();

            client.DetectLabels(new Amazon.Rekognition.Model.DetectLabelsRequest()
            {
                Image = new Amazon.Rekognition.Model.Image()
                {
                    Bytes = memoryStream
                }
            }).Labels.ForEach(x =>
            {
                listBox1.Items.Add(x.Name);
                
            });

            Brush brush = new SolidBrush(Color.YellowGreen);
            Pen pen = new Pen(brush, 2);
        }
    }
}
