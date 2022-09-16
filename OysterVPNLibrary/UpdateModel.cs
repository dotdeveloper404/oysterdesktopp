namespace OysterVPNLibrary
{
    using System;
    using System.Runtime.CompilerServices;

    internal class UpdateModel
    {
        public string Error { get; set; }

        public string Message { get; set; }

        public System.Version Version { get; set; }

        public string Url { get; set; }
    }
}

