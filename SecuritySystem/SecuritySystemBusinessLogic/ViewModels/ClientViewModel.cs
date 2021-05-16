using SecuritySystemBusinessLogic.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace SecuritySystemBusinessLogic.ViewModels
{
    [DataContract]
    public class ClientViewModel
    {
        [Column(title: "Номер", width: 100)]
        [DataMember]
        public int Id { get; set; }

        [Column(title: "ФИО клиента", width: 150)]
        [DataMember]
        public string ClientFIO { get; set; }

        [Column(title: "Логин", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string Email { get; set; }

        [Column(title: "Пароль", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string Password { get; set; }
    }
}
