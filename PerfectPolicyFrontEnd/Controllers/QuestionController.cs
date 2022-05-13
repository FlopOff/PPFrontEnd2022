using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPolicyFrontEnd.Helpers;
using PerfectPolicyFrontEnd.Models.QuestionModels;
using PerfectPolicyFrontEnd.Models.QuizModels;
using PerfectPolicyFrontEnd.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPolicyFrontEnd.Controllers
{
    public class QuestionController : Controller
    {

        private readonly IApiRequest<Question> _apiRequest;
        private readonly IApiRequest<Quiz> _apiQuizRequest;

        private readonly string questionController = "Question";

        private IWebHostEnvironment _environment;


        public QuestionController(IApiRequest<Question> apiRequest, IApiRequest<Quiz> apiQuizRequest, IWebHostEnvironment environment)
        {
            _apiRequest = apiRequest;
            _apiQuizRequest = apiQuizRequest;
            _environment = environment;
        }

        private bool isNotAuthenticated()
        {
            return !HttpContext.Session.Keys.Any(c => c.Equals("Token"));
        }

        // GET: QuestionController
        // Display ALL Questions
        public ActionResult Index()
        {
            List<Question> questions = _apiRequest.GetAll(questionController);
            return View(questions);
        }
        /// <summary>
        /// Return a filtered list (based on the quizID) of Questions to the index view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult QuestionsForQuiz(string id)
        {

            List <Question> questions = _apiRequest.GetAllForParentId(questionController, "QuestionsForQuizTitle", id);
            return View("Index", questions);
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            Question question = _apiRequest.GetSingle(questionController, id);
            return View(question);
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            var quizs = _apiQuizRequest.GetAll("Quiz");

            var quizDropDownList = quizs.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Title
            }).ToList();

            ViewData.Add("quizDDL", quizDropDownList);

            return View();
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionCreate question)
        {
            try
            { 
                Question createdQuestion = new Question()
                {
                    QuestionTopic = question.QuestionTopic,
                    QuestionText = question.QuestionText,
                    QuestionImage = question.QuestionImage,
                    Title = question.Title
                    
                };

            _apiRequest.Create(questionController, createdQuestion);
           
            return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


       // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            Question question = _apiRequest.GetSingle(questionController, id);

            return View(question);
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
           
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            Question question = _apiRequest.GetSingle(questionController, id);

            return View(question);
        }


        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            //if (isNotAuthenticated())
            //{
            //    return RedirectToAction("Login", "Auth");
            //}                    

            try
            {

                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _apiRequest.Delete(questionController, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string folderRoot = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads");

            string filePath = Path.Combine(folderRoot, file.FileName);

            try
            {
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { success = true, message = "File Uploaded" });
            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
        }
    }
}
