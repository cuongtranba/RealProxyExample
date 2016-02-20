# RealProxyExample

one way to implement logging function by use RealProxy

Ref : 
1. http://blog.benoitblanchon.fr/realproxy/
2. http://hintdesk.com/c-introduction-to-aspect-oriented-programming-with-realproxy/

```
        //(before execute) calling method name:set_Age - parameters value:2
        //(after execute) method name:set_Age - return value :
        //(before execute) calling method name:Eat - parameters value:Banana
        //(after execute) method name:Eat - return value :Baby eat Banana
        //(before execute) calling method name:Sleep - parameters value:
        //(after execute) method name:Sleep - return value :baby is sleeping
        //(before execute) calling method name:CallException - parameters value:
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
```
