using System;

namespace com.thejpaproject.avoptions.configurations

{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message)
        {}
    }
}
