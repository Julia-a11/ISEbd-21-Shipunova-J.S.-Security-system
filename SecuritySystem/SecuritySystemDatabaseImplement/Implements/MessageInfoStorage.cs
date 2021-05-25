using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemDatabaseImplement.Implements
{
    public class MessageInfoStorage : IMessageInfoStorage
    {
        public List<MessageInfoViewModel> GetFullList()
        {
            using (var context = new SecuritySystemDatabase())
            {
                return context.MessageInfoes
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<MessageInfoViewModel> GetFilteredList(MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                var messageInfoes = context.MessageInfoes
                    .Where(rec => (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (!model.ClientId.HasValue && rec.DateDelivery.Date == model.DateDelivery.Date) ||
                    (!model.ClientId.HasValue && model.PageNumber.HasValue) || 
                    (model.ClientId.HasValue && rec.ClientId == model.ClientId && model.PageNumber.HasValue));

                if (model.PageNumber.HasValue)
                {
                    messageInfoes = messageInfoes.Skip(model.StringsCountOnPage * (model.PageNumber.Value - 1))
                        .Take(model.StringsCountOnPage);
                }
               
                return messageInfoes.Select(CreateModel).ToList();
            }
        }

        public void Insert(MessageInfoBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                MessageInfo element = context.MessageInfoes.FirstOrDefault(rec => rec.MessageId == model.MessageId);
                if(element != null)
                {
                    return;
                }

                context.MessageInfoes.Add(new MessageInfo
                {
                    MessageId = model.MessageId,
                    ClientId = model.ClientId,
                    SenderName = model.FromMailAddress,
                    DateDelivery = model.DateDelivery,
                    Subject = model.Subject,
                    Body = model.Body
                });
                context.SaveChanges();
            }
        }

        public MessageInfoViewModel CreateModel(MessageInfo messageInfo)
        {
            return new MessageInfoViewModel
            {
                MessageId = messageInfo.MessageId,
                SenderName = messageInfo.SenderName,
                DateDelivery = messageInfo.DateDelivery,
                Subject = messageInfo.Subject,
                Body = messageInfo.Body
            };
        }
    }
}