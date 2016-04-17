using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace weather
{
    class FormState
    {
        private PictureBox pictureBox1;

        public FormState(PictureBox picturebox1)
        {
            Image image = Image.FromFile("C:\\Users\\Админ\\Desktop\\map.png");
            this.pictureBox1 = picturebox1;
            pictureBox1.Image = image;
            pictureBox1.Height = image.Height;
            pictureBox1.Width = image.Width;
        }
    }
}
