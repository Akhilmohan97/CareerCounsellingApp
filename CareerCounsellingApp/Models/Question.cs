using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace CareerCounsellingApp.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string QuestionText { get; set; } = "";
        public string QuestionTextMalayalam { get; set; } = "";
        public int CategoryId { get; set; }
        public decimal MaximumScore { get; set; }
        public Category? Category { get; set; }
        public QuestionImage? Image { get; set; }
        public ICollection<QuestionOption> Options { get; set; }
       = new List<QuestionOption>();

        public Bitmap? ImageBitmap
        {
            get
            {
                if (Image?.ImageData == null || Image.ImageData.Length == 0)
                    return null;

                return new Bitmap(new MemoryStream(Image.ImageData));
            }
        }

        public bool HasImage => Image?.ImageData != null && Image.ImageData.Length > 0;
    }
}
