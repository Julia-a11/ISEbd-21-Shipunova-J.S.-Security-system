using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecuritySystemBusinessLogic.BindingModels
{
    [DataContract]
    public class ClientBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string ClientFIO { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

    }
}
