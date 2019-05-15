using System;
using System.Configuration;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Configuration
{
    public class AppConfiguration : IConfiguration
    {
        public DataStoreType DataStoreType
        {
            get
            {
                return (DataStoreType) Enum.Parse(typeof(DataStoreType), 
                    ConfigurationManager.AppSettings["DataStoreType"]); 
            }
        }
    }
}