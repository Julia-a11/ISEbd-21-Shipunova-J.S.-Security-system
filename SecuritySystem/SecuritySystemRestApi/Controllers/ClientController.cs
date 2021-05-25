﻿using Microsoft.AspNetCore.Mvc;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecuritySystemRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientLogic _clientLogic;

        private readonly MailLogic _mailLogic;

        private readonly int _passwordMaxLength = 50;

        private readonly int _passwordMinLength = 10;


        public ClientController(ClientLogic logic, MailLogic mailLogic)
        {
            _clientLogic = logic;
            _mailLogic = mailLogic;
        }

        [HttpGet]
        public ClientViewModel Login(string login, string password)
        {
            return _clientLogic.Read(new ClientBindingModel
            {
                Email = login,
                Password = password
            })?[0];
        }

        [HttpGet]
        public List<MessageInfoViewModel> GetMessages(int clientId, int pageNumber) => _mailLogic.Read(new MessageInfoBindingModel
        {
            ClientId = clientId,
            PageNumber = pageNumber
        });

        [HttpPost]
        public void Register(ClientBindingModel model)
        {
            CheckData(model);
            _clientLogic.CreateOrUpdate(model);
        }

        [HttpPost]
        public void UpdateData(ClientBindingModel model)
        {
            CheckData(model);
            _clientLogic.CreateOrUpdate(model);
        }

        private void CheckData(ClientBindingModel model)
        {
            if (!Regex.IsMatch(model.Email, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
            {
                throw new Exception("В качестве логина должна быть указана почта");
            }
            if (model.Password.Length > _passwordMaxLength || model.Password.Length < _passwordMinLength ||
                !Regex.IsMatch(model.Password, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль длиной от {_passwordMinLength} до {_passwordMaxLength} " +
                    $"должен состоять из цифр, букв и небуквенных символов");
            }
        }
    }
}
