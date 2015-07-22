using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;

namespace FaceDetection.Core
{
    public class FaceRecognizerService
    {
        /// <summary>
        /// Делает фото с веб-камеры
        /// </summary>
        public void StartCapture()
        {
            var capture = new Capture(); //create a camera captue
            var photo = capture.QueryFrame(); //draw the image obtained from camera
            photo.Save("test.jpg");
        }
    }
}
