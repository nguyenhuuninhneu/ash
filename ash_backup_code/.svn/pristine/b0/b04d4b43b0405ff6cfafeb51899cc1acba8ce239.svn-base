using System.Drawing;
using System.Web.Mvc;
using Web.Core;

namespace HRM.Controllers
{
    public class ImageCropController : Controller
    {
        //
        // GET: /ImageCrop/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(int x1, int y1, int h, int w)
        {
            if(w > 0 && h > 0)
            {
                using (var absentRectangleImage = (Bitmap)Bitmap.FromFile(Common.GetPath("~/Upload/Images/Normal/2017/1/", "fe6a1c47150459e33230ab799a0bdb40-123.jpg")))
                {
                    using (var currentTile = new Bitmap(w, h))
                    {
                        currentTile.SetResolution(absentRectangleImage.HorizontalResolution, absentRectangleImage.VerticalResolution);

                        using (var currentTileGraphics = Graphics.FromImage(currentTile))
                        {
                            currentTileGraphics.Clear(Color.Black);
                            var absentRectangleArea = new Rectangle(x1, y1, w, h);
                            currentTileGraphics.DrawImage(absentRectangleImage, 0, 0, absentRectangleArea, GraphicsUnit.Pixel);
                        }

                        currentTile.Save(Common.GetPath("~/", "abc.jpg"));
                    }
                }
            }
            return View();
        }

    }
}
