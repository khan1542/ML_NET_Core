using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using MLNET_Console_App;
using MLNET_NET_Core.Models;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace MLNET_NET_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly PredictionEnginePool<MLModel.ModelInput, MLModel.ModelOutput> _predictionImage;

        public HomeController(ILogger<HomeController> logger, PredictionEnginePool<MLModel.ModelInput, MLModel.ModelOutput> predictionImage)
        {
            _logger = logger;
            _predictionImage = predictionImage;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(null);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            using MemoryStream ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);

            var bytes = ms.ToArray();

            MLModel.ModelInput sampleImage = new MLModel.ModelInput()
            {
                ImageSource = bytes
            };

            var predictImage = _predictionImage.Predict(sampleImage);

            var labels = ModelLabel.GetLabels();

            for (int i = 0; i < predictImage.Score.Length; i++)
            {
                var label = labels.FirstOrDefault(x => x.Id == i);

                if (label != null) label.Score = predictImage.Score[i];
            }

            labels = labels.OrderByDescending(x => x.Score).Take(2).ToList();

            return View(labels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}