﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPolicyFrontEnd.Helpers;
using PerfectPolicyFrontEnd.Models.QuestionModels;
using PerfectPolicyFrontEnd.Models.OptionModels;
using PerfectPolicyFrontEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPolicyFrontEnd.Controllers
{
    public class OptionController : Controller
    {
        private readonly IApiRequest<Option> _apiRequest;
        private readonly IApiRequest<Question> _apiQuestionRequest;

        private readonly string optionController = "Option";

        public OptionController(IApiRequest<Option> apiRequest, IApiRequest<Question> apiQuestionRequest)
        {
            _apiRequest = apiRequest;
            _apiQuestionRequest = apiQuestionRequest;
        }
        private bool isNotAuthenticated()
        {
            return !HttpContext.Session.Keys.Any(c => c.Equals("Token"));
        }

        // GET: OptionController
        // Display ALL Options
        public ActionResult Index()
        {
            List<Option> options = _apiRequest.GetAll(optionController);
            return View(options);
        }
        /// <summary>
        /// Return a filtered list (based on the quizID) of Questions to the index view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult OptionsForQuestion(int id)
        {
            List<Option> options = _apiRequest.GetAllForParentId(optionController, "OptionsForQuestionID", id);
            return View("Index", options);
        }

        // GET: OptionController/Details/5
        public ActionResult Details(int id)
        {
            Option option = _apiRequest.GetSingle(optionController, id);
            return View(option);
        }

        // GET: OptionController/Create
        public ActionResult Create()
        {
            var questions = _apiQuestionRequest.GetAll("Question");

            var questionDropDownListModel = questions.Select(c => new SelectListItem
            {
                Text = c.QuestionTopic,
                Value = c.QuestionID.ToString()
            }).ToList();

            ViewData.Add("questionDDL", questionDropDownListModel);

            return View();
        }

        // POST: OptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OptionCreate option)
        {
            try
            {
                Option createdOption = new Option()
                {
                    OptionText = option.OptionText,
                    OptionOrderByLetter = option.OptionOrderByLetter,
                    OptionOrderByNumber = option.OptionOrderByNumber,
                    Answer = option.Answer,
                    QuestionID = option.QuestionID
                };

                _apiRequest.Create(optionController, createdOption);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: OptionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OptionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            //if (isNotAuthenticated())
            //{
            //    return RedirectToAction("Login", "Auth");
            //}

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OptionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OptionController/Delete/5
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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

