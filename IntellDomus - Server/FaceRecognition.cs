using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;
using System.Drawing;


namespace IntellDomus___Server
{
    class FaceRecognition
    {
        //declaring global variables
        private Capture capture;        //takes images from camera as image frames
        private bool faceSee;
        private static readonly CascadeClassifier Classifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        //Elementi UI
        System.Windows.Controls.Image pictureBox;

        public FaceRecognition(System.Windows.Controls.Image pictureBox)
        {
            this.pictureBox = pictureBox;
            reco();
        }

        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>

        //Convertitore tipo di Bitmap
        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            //Cattura del Frame del video, quindi un image
            Image<Bgr, Byte> imageFrame = capture.QueryFrame();

            //convert the image to gray scale
            Image<Gray, byte> grayFrame = imageFrame.Convert<Gray, byte>();


            //var detectedFaces = Classifier.DetectMultiScale(grayFrame, dd, 1, minSize, maxSize);
            //Attraverso scala di gray individua un volto presente nella scena
            Rectangle[] rectangles = Classifier.DetectMultiScale(grayFrame, 1.4, 0, new System.Drawing.Size(100, 100), new System.Drawing.Size(800, 800));


            //Int32Rect faceSave = new Int32Rect();

            //Estrazione di un volto per volta, da tutti quelli individuati
            foreach (var face in rectangles)
            {
                //Rectangle rect = new Rectangle(face.X, face.Y, face.Width, face.Height);
                
                imageFrame.Draw(face, new Bgr(0, 0, 0), 5);
                //faceSave = new Int32Rect(rect.X, rect.Y, rect.Width, rect.Height);
            }

            /*if (faceSee)
            {
                //faceBox.Source = new CroppedBitmap(ToBitmapSource(imageFrameClear), faceSave);
                faceSee = false;
            }*/

            pictureBox.Source = ToBitmapSource(imageFrame); //COnversione in Bitmap per WPF
        }

        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        private void reco()
        {
            ReleaseData();
            try
            {
                capture = new Capture(0);
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            //Per il Recognize
            /*Image<Bgr, Byte> imageExample = new Image<Bgr, byte>("img/image1.jpg");  //Immagine DB
            Image<Gray, Byte> grayImageExample = imageExample.Convert<Gray, byte>();  //Immagine DB
            var detectedFacesExample = haar.Detect(grayImageExample);*/
 
            //Avvio Thread per il video e l'individuazione dei volti 
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ProcessFrame);
            dispatcherTimer.Start();
        }
    }    
}
