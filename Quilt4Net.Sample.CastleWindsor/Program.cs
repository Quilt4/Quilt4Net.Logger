using System;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Sample.CastleWindsor
{
    static class Program
    {
        private static void Main(string[] args)
        {
            var container = new WindsorContainer();

            container.Install(FromAssembly.This());

            var client = container.Resolve<IQuilt4NetClient>();
            //client.Client.WebApiRequestEvent += WebApiClientWebApiRequestEvent;
            //client.Client.WebApiResponseEvent += WebApiClient_WebApiResponseEvent;
            //client.Client.AuthorizationChangedEvent += WebApiClient_AuthorizationChangedEvent;

            var business1 = container.Resolve<ISomeBusiness1>();
            var business2 = container.Resolve<ISomeBusiness2>();

            try
            {
                business1.Execute();
                business2.Execute();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
            }

            //business1.Dispose();
            container.Dispose();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void WebApiClient_AuthorizationChangedEvent(object sender, Core.Events.AuthorizationChangedEventArgs e)
        {
            Console.WriteLine("WebApiClient_AuthorizationChangedEvent: " + e.UserName);
        }

        private static void WebApiClientWebApiRequestEvent(object sender, WebApiRequestEventArgs e)
        {
            Console.WriteLine("REQUEST: (" + e.Path + ")" + e.ContentData);
        }

        private static void WebApiClient_WebApiResponseEvent(object sender, WebApiResponseEventArgs e)
        {
            if (!e.IsSuccess)
            {
                Console.WriteLine("> " + e.Request.OperationType + ": " + e.Request.Path + " --> " + (e.IsSuccess ? "Success" : "Fail: " + e.Exception.Message));
            }
            else
            {
                Console.WriteLine("RESPONSE (" + e.Request.Path + "): " + e.ContentData);
            }
        }
    }

    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .Where(Component.IsInSameNamespaceAs<SomeBusiness1>())
                .WithService.DefaultInterfaces()
                .LifestyleTransient());

            //container.Register(Component.For<IConfiguration>().ImplementedBy(typeof(Quilt4Net.Configuration)).LifestyleSingleton());
            //container.Register(Component.For<IQuilt4NetClient>().ImplementedBy(typeof(Quilt4Net.Quilt4Client)).LifestyleSingleton());
            //container.Register(Component.For<ISessionHandler>().ImplementedBy(typeof(Quilt4Net.SessionHandler)).LifestyleSingleton());
            //container.Register(Component.For<IIssueHandler>().ImplementedBy(typeof(Quilt4Net.IssueHandler)).LifestyleSingleton());

            //container.Register(Classes.FromAssemblyContaining(typeof(Quilt4Client))
            //    .Where(Component.IsInSameNamespaceAs<IssueHandler>())
            //    .WithService.FromInterface(typeof(IIssueHandler))
            //    .LifestyleTransient());

            //TODO: Using this should fail the execution (since two clients are created in the same runner)
            container.Register(Classes.FromAssemblyContaining(typeof(Quilt4Client))
                .Where(Component.IsInSameNamespaceAs<IssueHandler>())
                .WithService.DefaultInterfaces()
                //.LifestyleTransient());
                .LifestyleSingleton());
        }
    }

    public class SomeBusiness2 : ISomeBusiness2
    {
        private readonly IIssueHandler _issueHandler;

        public SomeBusiness2(IIssueHandler issueHandler)
        {
            _issueHandler = issueHandler;
        }

        public void Execute()
        {
            _issueHandler.Register("Mimimal message");
            //_issueHandler.Register("Maximal message", MessageIssueLevel.Information, "Bob Loblaw", new Dictionary<string, string> { { "A", "1" } });

            Console.WriteLine("Executing some more stuff...");
        }
    }

    public class SomeBusiness1 : ISomeBusiness1
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly IIssueHandler _issueHandler;

        public SomeBusiness1(ISessionHandler sessionHandler, IIssueHandler issueHandler)
        {
            _sessionHandler = sessionHandler;
            _sessionHandler.SessionRegistrationStartedEvent += _sessionHandler_SessionRegistrationStartedEvent;
            _sessionHandler.SessionRegistrationCompletedEvent += _sessionHandler_SessionRegistrationCompletedEvent;
            _sessionHandler.SessionEndStartedEvent += _sessionHandler_SessionEndStartedEvent;
            _sessionHandler.SessionEndCompletedEvent += _sessionHandler_SessionEndCompletedEvent;
            _issueHandler = issueHandler;
            _issueHandler.IssueRegistrationStartedEvent += _issueHandler_IssueRegistrationStartedEvent;
            _issueHandler.IssueRegistrationCompletedEvent += _issueHandler_IssueRegistrationCompletedEvent;
        }

        private void _sessionHandler_SessionEndCompletedEvent(object sender, Core.Events.SessionEndCompletedEventArgs e)
        {
            Console.WriteLine(">");
        }

        private void _sessionHandler_SessionEndStartedEvent(object sender, Core.Events.SessionEndStartedEventArgs e)
        {
            Console.WriteLine("|");
        }

        private void _sessionHandler_SessionRegistrationCompletedEvent(object sender, Core.Events.SessionRegistrationCompletedEventArgs e)
        {
            Console.WriteLine("|");
        }

        private void _sessionHandler_SessionRegistrationStartedEvent(object sender, Core.Events.SessionRegistrationStartedEventArgs e)
        {
            Console.WriteLine("<");
        }

        private void _issueHandler_IssueRegistrationCompletedEvent(object sender, Core.Events.IssueRegistrationCompletedEventArgs e)
        {
            Console.WriteLine("z");
        }

        private void _issueHandler_IssueRegistrationStartedEvent(object sender, Core.Events.IssueRegistrationStartedEventArgs e)
        {
            Console.WriteLine("a");
        }

        public void Execute()
        {
            _sessionHandler.Register();

            try
            {
                try
                {
                    try
                    {
                        var task1 = Task.Run(() =>
                            {
                                System.Threading.Thread.Sleep(500);
                                throw new InvalidOperationException("First");
                            });
                        var task2 = Task.Run(() =>
                            {
                                System.Threading.Thread.Sleep(500);
                                throw new InvalidOperationException("Second");
                            });

                        Task.WaitAll(task1, task2);
                    }
                    catch (Exception exception)
                    {
                        throw new InvalidOperationException("Some inner exception.", exception);
                    }
                }
                catch (Exception exception)
                {
                    exception.AddData("A", "1").AddData("B", "2");
                    throw new InvalidOperationException("Some exception", exception);
                }
            }
            catch (Exception exception)
            {
                exception.AddData("B", "22").AddData("C", "3");
                _issueHandler.Register(exception, ExceptionIssueLevel.Error, "Bob Loblaw");
            }

            Console.WriteLine("Executing some stuff...");
        }
    }

    public interface ISomeBusiness1
    {
        void Execute();
    }

    public interface ISomeBusiness2
    {
        void Execute();
    }
}