using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Text;
using IronOcr;
using Tesseract; 

namespace OCR_CSharp
{
    class Program
    {
        public class TextData
        {
            public string Text { get; set; }
        }

        /// <summary>
        /// Normalizes the provided text by removing unwanted spaces and converting it to lowercase.
        /// This includes removing leading and trailing spaces, collapsing multiple spaces into a single space,
        /// and ensuring all characters are lowercase for uniformity.
        /// </summary>
        /// <param name="text">The text to be normalized.</param>
        /// <returns>A normalized version of the input text, with all letters in lowercase and extra spaces removed.</returns>
        private static string NormalizeText(string text)
        {
            return string.IsNullOrWhiteSpace(text) 
                ? string.Empty
                : string.Join(" ", text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToLower();
        }

        /// <summary>
        /// Performs Optical Character Recognition (OCR) on a given image using Tesseract to extract text 
        /// in both English and French. The extracted text is written to a result file and returned as a string.
        /// </summary>
        /// <param name="path_image">The path to the image to process.</param>
        /// <param name="path_result">The path to the file where the extracted text should be saved.</param>
        /// <returns>The extracted text from the image after processing by Tesseract.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the specified image file is not found.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown if the directory for the result file cannot be created.</exception>
        /// <exception cref="Exception">Thrown if any other error occurs during Tesseract execution.</exception>
        static string OCRTesseract(string path_image, string path_result)
        {
            try
            {
                using var engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng+fra", EngineMode.Default);

                using var img = Pix.LoadFromFile(path_image);

                using var page = engine.Process(img);
                string text = page.GetText();

                File.WriteAllText(path_result, text);

                Console.WriteLine($"Text extracted from {path_image}: {text}");
                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Tesseract execution: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Performs Optical Character Recognition (OCR) on a given image using IronTesseract to extract text. 
        /// The extracted text is written to a result file and returned as a string.
        /// </summary>
        /// <param name="path_image">The path to the image to process.</param>
        /// <param name="path_result">The path to the file where the extracted text should be saved.</param>
        /// <returns>The extracted text from the image after processing by IronTesseract.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the specified image file is not found.</exception>
        /// <exception cref="IOException">Thrown if the result file cannot be written.</exception>
        /// <remarks>
        /// IronOCR is a paid library. You may need a valid license for full functionality.
        /// </remarks>
        static string OCRIronTesseract(string path_image, string path_result)
        {
            var ocr = new IronTesseract();

            var result = ocr.Read(path_image);

            File.WriteAllText(path_result, result.Text);

            Console.WriteLine($"Text extracted from {path_image}: {result.Text}");

            return result.Text;
        }

        /// <summary>
        /// Performs a TF-IDF similarity comparison between a reference text (from a file) and a text to compare.
        /// The similarity is calculated using cosine similarity between their TF-IDF feature vectors.
        /// </summary>
        /// <param name="referencePath">The file path of the reference text.</param>
        /// <param name="textToCompare">The text to compare with the reference text.</param>
        /// <remarks>
        /// This method loads the texts, applies text featurization using TF-IDF, and then calculates the cosine similarity 
        /// between their feature vectors to determine the similarity percentage.
        /// </remarks>
        static double CalculateTFIDFSimilarity(string referencePath, string textToCompare)
        {
            var context = new MLContext();

            var referenceText = new TextData { Text = NormalizeText(File.ReadAllText(referencePath)) };
            var comparisonText = new TextData { Text = NormalizeText(textToCompare) };

            var data = new[] { referenceText, comparisonText };
            var trainData = context.Data.LoadFromEnumerable(data);

            var pipeline = context.Transforms.Text.FeaturizeText("Features", nameof(TextData.Text));

            var transformedData = pipeline.Fit(trainData).Transform(trainData);

            var features = transformedData.GetColumn<float[]>("Features").ToArray();

            var similarity = CalculateCosineSimilarity(features[0], features[1]);

            return Math.Round(similarity,2)*100;
        }

        /// <summary>
        /// Calculates the cosine similarity between two vectors.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The cosine similarity score between the two vectors.</returns>
        /// <remarks>
        /// The cosine similarity ranges from -1 to 1, where 1 means the vectors are identical, 
        /// 0 means they are orthogonal, and -1 means they are opposite.
        /// </remarks>
        static double CalculateCosineSimilarity(float[] vec1, float[] vec2)
        {
            var dotProduct = vec1.Zip(vec2, (x, y) => x * y).Sum();
            var magnitudeVec1 = Math.Sqrt(vec1.Sum(x => x * x));
            var magnitudeVec2 = Math.Sqrt(vec2.Sum(x => x * x));

            return dotProduct / (magnitudeVec1 * magnitudeVec2);
        }


        static void Main(string[] args)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\.."));

            string memePath = Path.Combine(projectRoot, "OCR_Items", "test_image.jpeg");
            string memeTraductionPath = Path.Combine(projectRoot, "OCR_Items", "test_image.txt");
            string resultMemePath = Path.Combine(projectRoot, "OCR_CSharp", @"result_ocr_csharp\result_test_image.txt");
            string ocr1Path = Path.Combine(projectRoot, "OCR_Items", "test_OCR_1.jpg");
            string ocr1TraductionPath = Path.Combine(projectRoot, "OCR_Items", "test_OCR_trad_1.txt");
            string resultOcr1Path = Path.Combine(projectRoot, "OCR_CSharp", @"result_ocr_csharp\result_test_OCR_1.txt");
            string ocr2Path = Path.Combine(projectRoot, "OCR_Items", "test_OCR_2.jpg");
            string ocr2TraductionPath = Path.Combine(projectRoot, "OCR_Items", "test_OCR_trad_2.txt");
            string resultOcr2Path = Path.Combine(projectRoot, "OCR_CSharp", @"result_ocr_csharp\result_test_OCR_2.txt");

            OCRTesseract(memePath, resultMemePath);
            OCRTesseract(ocr1Path, resultOcr1Path);
            OCRTesseract(ocr2Path, resultOcr2Path);

            double similarity1 = CalculateTFIDFSimilarity(memeTraductionPath, File.ReadAllText(resultMemePath));
            double similarity2 = CalculateTFIDFSimilarity(ocr1TraductionPath, File.ReadAllText(resultOcr1Path));
            double similarity3 = CalculateTFIDFSimilarity(ocr2TraductionPath, File.ReadAllText(resultOcr2Path));

            Console.WriteLine($"Similarity between reference and OCR result (meme): {similarity1}%");
            Console.WriteLine($"Similarity between reference and OCR result (OCR 1): {similarity2}%");
            Console.WriteLine($"Similarity between reference and OCR result (OCR 2): {similarity3}%");
        }
    }

}
