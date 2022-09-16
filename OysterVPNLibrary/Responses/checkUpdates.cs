namespace OysterVPNLibrary.Responses
{
    using System;

    public class checkUpdates : Response
    {
        public string version;
        public string url;
        public bool isUpdateAvailable = false;
    }
}

