using System;

namespace RealProxyExample
{
    /// <summary>
    /// object for logging must delivery from MarshalByRefObject
    /// </summary>
    public class Baby:MarshalByRefObject
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public string Eat(string foodName)
        {
            return $"Baby eat {foodName}";
        }

        public string Sleep()
        {
            return "baby is sleeping";
        }

        public void CallException()
        {
            throw new InvalidOperationException("throw exception");
        }
    }
}