using System;

namespace RealProxyExample
{
    /// <summary>
    /// ref : http://blog.benoitblanchon.fr/realproxy/
    /// ref : http://hintdesk.com/c-introduction-to-aspect-oriented-programming-with-realproxy/
    /// </summary>
    class MainClass
    {
        static void Main(string[] args)
        {
            var baby = LoggingProxy.Wrap(new Baby());
            baby.Age = 2;
            baby.Eat("Banana");
            baby.Sleep();
            try
            {
                baby.CallException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
