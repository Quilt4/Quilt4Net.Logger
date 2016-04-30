using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class RegisterIssueCommand : ActionCommandBase
    {
        private readonly IIssueHandler _issueHandler;

        public RegisterIssueCommand(IIssueHandler issueHandler)
            : base("Register", "Register issue")
        {
            _issueHandler = issueHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            Guid? domainKey = null;
            var dictionary = new Dictionary<string, string>
                {
                    { "scanMode", "A" },
                    { "resume", true.ToString() },
                    { "farmName", "B" },
                    { "domainKey", (domainKey ?? Guid.Empty).ToString() }
                };

            Quilt4Net.Singleton.Configuration.Instance.AllowMultipleInstances = true;
            Quilt4Net.Singleton.Issue.Instance.Register("Starting detective.", MessageIssueLevel.Information, data: dictionary);

            //try
            //{
            //    try
            //    {
            //        try
            //        {
            //            throw new InvalidOperationException("Some issue!");
            //        }
            //        catch (Exception exception)
            //        {
            //            exception.AddData("A", "A1");
            //            _issueHandler.Register(exception);
            //            throw new InvalidOperationException("Next level", exception);
            //        }
            //    }
            //    catch (Exception exception)
            //    {
            //        exception.AddData("A", "A1");
            //        _issueHandler.Register(exception);
            //        throw new InvalidOperationException("Outer!", exception);
            //    }
            //}
            //catch (Exception exception)
            //{
            //    exception.AddData("B", "B1");
            //    _issueHandler.Register(exception);
            //    throw;
            //}

            //Task.Run(() => DoRegisterIssue());
            //Task.Run(() => DoRegisterIssue());
            //Task.Run(() => DoRegisterIssue());
            //Task.Run(() => DoRegisterIssue());
            //Task.Run(() => DoRegisterIssue());

            return true;
        }

        private void DoRegisterIssue()
        {
            var response = _issueHandler.Register("Some warning!", MessageIssueLevel.Warning);
            if (response.IsSuccess)
                OutputInformation("Issue registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
            else
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");
        }
    }
}