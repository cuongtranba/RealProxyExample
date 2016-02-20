using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace RealProxyExample
{
    class LoggingProxy
    {
        public static T Wrap<T>(T target)
        {
            var objectProxy=new ObjectProxy<T>(target);
            objectProxy.BeforeExecute += ObjectProxy_BeforeExecute;
            objectProxy.AfterExecute += ObjectProxy_AfterExecute;
            objectProxy.ErrorExecute += ObjectProxy_ErrorExecute;

            return (T)objectProxy.GetTransparentProxy();
        }

        private static void ObjectProxy_ErrorExecute(object sender, Exception e)
        {
            Console.WriteLine(e.Message);
        }

        private static void ObjectProxy_AfterExecute(object sender, ReturnMessage e)
        {
            Console.WriteLine($"(after execute) method name:{e.MethodName} - return value :{e.ReturnValue}");
        }

        private static void ObjectProxy_BeforeExecute(object sender, IMethodCallMessage e)
        {
            Console.WriteLine($"(before execute) calling method name:{e.MethodName} - parameters value:{(e.InArgs.Any() ? e.InArgs[0] : string.Empty )}");
        }
    }

    class ObjectProxy<T> : RealProxy
    {
        private readonly T decorated;

        public ObjectProxy(T decorated)
            : base(typeof(T))
        {
            this.decorated = decorated;

        }

        public event EventHandler<ReturnMessage> AfterExecute;

        public event EventHandler<IMethodCallMessage> BeforeExecute;

        public event EventHandler<Exception> ErrorExecute;

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            OnBeforeExecute(methodCall);
            try
            {
                var result = methodInfo.Invoke(decorated, methodCall.InArgs);
                var returnMessage = new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                OnAfterExecute(returnMessage);
                return returnMessage;
            }
            catch (TargetInvocationException invocationException)
            {
                var exception = invocationException.InnerException;
                OnErrorExecute(exception);
                return new ReturnMessage(exception, methodCall);
            }
        }

        private void OnAfterExecute(ReturnMessage methodCall)
        {
            AfterExecute?.Invoke(this, methodCall);
        }

        private void OnBeforeExecute(IMethodCallMessage methodCall)
        {
            BeforeExecute?.Invoke(this, methodCall);
        }

        private void OnErrorExecute(Exception methodCall)
        {
            ErrorExecute?.Invoke(this, methodCall);
        }
    }

}
