namespace OysterVPNLibrary.Responses
{
    using OysterVPNModel;
    using System;
    using System.Collections.Generic;

    public class LoginResponse : Response
    {
        public int id = 0;
        public string token = "";
        public UserInfo userinfo = new UserInfo();
        public Product product = new Product();
        //public Server[] servers;
        public List<Server> servers;
    }
}

