﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPolicyFrontEnd.Helpers;
using PerfectPolicyFrontEnd.Models;
using PerfectPolicyFrontEnd.Models.QuizModels;
using PerfectPolicyFrontEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPolicyFrontEnd.Controllers
{
    public class QuizController : Controller
    {
        private readonly IApiRequest<Quiz> _apiRequest;

        private readonly string quizController = "Quiz";

        public QuizController(IApiRequest<Quiz> apiRequest)
        {
            _apiRequest = apiRequest;
        }


        [HttpPost]
        public ActionResult Filter(Company company)
        {
            ViewBag.Name = company.CompanyName;
            return RedirectToAction("Index", company);

        }

        //[HttpPost]
        //public IActionResult QuizFilter(IFormCollection collection, Quiz quiz)
        //{
        //    ViewBag.Title = quiz.QuizTitle;
        //    var result = collection["quizDDL"].ToString();
        //    return RedirectToAction("Index", new { filter = result });
        //}

        public ActionResult Index(Quiz quiz)
        {

            var quizList = _apiRequest.GetAll(quizController);

            Company company = new Company();
            ViewBag.Name = company.CompanyName;
            ;

            return View(quizList);
        }
        
        private bool isAuthenticated()
        {
            return HttpContext.Session.Keys.Any(c => c.Equals("Token"));
        }


        // GET: QuizController/Details/5
        public ActionResult Details(int id)
        {
            Quiz quiz = _apiRequest.GetSingle(quizController, id);

            return View(quiz);
        }

        // GET: QuizController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuizController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuizCreate quiz)
        {
            try
            {
                Quiz createdQuiz = new Quiz()
                {
                    Title = quiz.Title,
                    Topic = quiz.Topic,
                    Creator = quiz.Creator,
                    DateCreated = quiz.DateCreated,
                    PassingPercentage = quiz.PassingPercentage
                };

                _apiRequest.Create(quizController, createdQuiz);


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            Quiz quiz = _apiRequest.GetSingle(quizController, id);

            return View(quiz);
        }

        // POST: QuizController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Quiz quiz)
        {
            try
            {
                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _apiRequest.Edit(quizController, quiz, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizController/Delete/5
        public ActionResult Delete(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            Quiz quiz = _apiRequest.GetSingle(quizController, id);

            return View(quiz);
        }

        // POST: QuizController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _apiRequest.Delete(quizController, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }               
        }

        [HttpPost]
        public IActionResult FilterCreator(IFormCollection collection)
        {
            
            string filterText = collection["companyName"];


            var creatorList = _apiRequest.GetAll(quizController);

            var filteredList = creatorList.Where(c => c.Creator.ToLower().Contains(filterText.ToLower())).ToList();

            return View("Index", filteredList);
        }
    }
}
