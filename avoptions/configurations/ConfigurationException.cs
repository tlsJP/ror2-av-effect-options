using System;

namespace com.thejpaproject.avoptions.configurations

{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string message, Exception ex) : base(message, ex)
        { }
    }
}
