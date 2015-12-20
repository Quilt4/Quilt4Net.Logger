using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class Issue : IIssue
    {
        private readonly Lazy<ISession> _session;
        private readonly IWebApiClient _webApiClient;
        private readonly IConfiguration _configuration;

        internal Issue(Lazy<ISession> session, IWebApiClient webApiClient, IConfiguration configuration)
        {
            _session = session;
            _webApiClient = webApiClient;
            _configuration = configuration;
        }

        public event EventHandler<IssueRegistrationStartedEventArgs> IssueRegistrationStartedEvent;
        public event EventHandler<IssueRegistrationCompletedEventArgs> IssueRegistrationCompletedEvent;

        public async Task<IssueResponse> RegisterAsync(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            var issueData = PrepareIssueData(message, issueLevel, userHandle, data);
            return await RegisterEx(false, issueData);
        }

        public void RegisterStart(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            var issueData = PrepareIssueData(message, issueLevel, userHandle, data);
            Task.Run(() => RegisterEx(false, issueData));
        }

        public IssueResponse Register(string message, MessageIssueLevel issueLevel, string userHandle = null, IDictionary<string, string> data = null)
        {
            var issueData = PrepareIssueData(message, issueLevel, userHandle, data);
            var respnse = Task.Run(async () => await RegisterEx(false, issueData)).Result;
            return respnse;
        }

        public async Task<IssueResponse> RegisterAsync(Exception exception, ExceptionIssueLevel issueLevel = Issue.ExceptionIssueLevel.Error, string userHandle = null)
        {
            var issueData = PrepareIssueData(exception, issueLevel, userHandle);
            var respnse = await RegisterEx(true, issueData);
            return respnse;
        }

        public void RegisterStart(Exception exception, ExceptionIssueLevel issueLevel = Issue.ExceptionIssueLevel.Error, string userHandle = null)
        {
            var issueData = PrepareIssueData(exception, issueLevel, userHandle);
            Task.Run(() => RegisterEx(false, issueData));
        }

        public IssueResponse Register(Exception exception, ExceptionIssueLevel issueLevel = Issue.ExceptionIssueLevel.Error, string userHandle = null)
        {
            var issueData = PrepareIssueData(exception, issueLevel, userHandle);
            var respnse = Task.Run(async () => await RegisterEx(false, issueData)).Result;
            return respnse;
        }

        private async Task<IssueResponse> RegisterEx(bool doThrow, IssueRequest request)
        {
            //TODO: Use a Mutex here

            var response = new IssueResponse();

            try
            {
                request.SessionKey = _session.Value.GetSessionKey();

                OnIssueRegistrationStartedEvent(new IssueRegistrationStartedEventArgs(request));

                await _webApiClient.CreateAsync("Client/Issue", request);
                //TODO: Wait for response from server here. (We should get a Ticket here)
            }
            catch (Exception exception)
            {
                response.SetException(exception);

                if (doThrow)
                    throw;
            }
            finally
            {
                response.SetCompleted();
                OnIssueRegistrationCompletedEvent(new IssueRegistrationCompletedEventArgs(request, response));
            }

            return response;
        }

        private IssueRequest PrepareIssueData(Exception exception, ExceptionIssueLevel issueLevel, string userHandle)
        {
            var issueType = CreateIssueTypeData(exception, issueLevel);

            var issueData = new IssueRequest
            {
                ProjectApiKey = _configuration.ProjectApiKey,
                Data = exception.Data.Cast<DictionaryEntry>().Where(x => x.Value != null).ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()),
                UserHandle = userHandle,
                ClientTime = DateTime.UtcNow,
                IssueKey = Guid.NewGuid(),
                IssueThreadKey = HandleIssueThreadGuid(exception),
                IssueType = issueType,
            };

            return issueData;
        }

        private IssueTypeData CreateIssueTypeData(Exception exception, ExceptionIssueLevel issueLevel)
        {
            var issueType = new IssueTypeData
            {
                Message = exception.Message,
                IssueLevel = issueLevel.ToIssueLevel(_configuration),
                Inner = exception.InnerException != null ? CreateIssueTypeData(exception, issueLevel) : null,
                StackTrace = exception.StackTrace,
                Type = exception.GetType().ToString(),
            };
            return issueType;
        }

        private IssueRequest PrepareIssueData(string message, MessageIssueLevel issueLevel, string userHandle, IDictionary<string, string> data)
        {            
            var issueType = new IssueTypeData
            {
                Message = message,
                IssueLevel = issueLevel.ToIssueLevel(_configuration),
                Inner = null,
                StackTrace = null,
                Type = null,
            };

            var issueData = new IssueRequest
            {
                ProjectApiKey = _configuration.ProjectApiKey,
                Data = data,
                UserHandle = userHandle,
                ClientTime = DateTime.UtcNow,
                IssueKey = Guid.NewGuid(),
                IssueThreadKey = null, //TODO: Provide Issue Thread Key
                IssueType = issueType,              
            };

            return issueData;
        }

        public enum MessageIssueLevel
        {
            Information,
            Warning,
            Error,
        }

        public enum ExceptionIssueLevel
        {
            Warning,
            Error,
        }

        private static Guid HandleIssueThreadGuid(Exception exception)
        {
            var refItg = Guid.NewGuid();

            if (exception == null) return refItg;

            if (!exception.Data.Contains("IssueThreadGuid"))
            {
                exception.Data.Add("IssueThreadGuid", refItg);
            }
            else
            {
                Guid result;
                if (Guid.TryParse(exception.Data["IssueThreadGuid"].ToString(), out result))
                {
                    refItg = result;
                }
                else
                {
                    //NOTE: When there is a general message/warning event. Fire this information.
                    //Provided IssueThreadGuid cannot be parsed as Guid. Apply a new valid value.
                    exception.Data["IssueThreadGuid"] = refItg;
                }
            }

            return refItg;
        }

        protected virtual void OnIssueRegistrationStartedEvent(IssueRegistrationStartedEventArgs e)
        {
            IssueRegistrationStartedEvent?.Invoke(this, e);
        }

        protected virtual void OnIssueRegistrationCompletedEvent(IssueRegistrationCompletedEventArgs e)
        {
            IssueRegistrationCompletedEvent?.Invoke(this, e);
        }
    }

    internal static class IssueLevelExtension
    {
        internal static IssueLevel ToIssueLevel(this Issue.ExceptionIssueLevel issueLevel, IConfiguration configuration)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw new ExpectedIssues(configuration).GetException(ExpectedIssues.CannotParseIssueLevelException).AddData("IssueLevel", issueLevel);

            return il;
        }

        internal static IssueLevel ToIssueLevel(this Issue.MessageIssueLevel issueLevel, IConfiguration configuration)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw new ExpectedIssues(configuration).GetException(ExpectedIssues.CannotParseIssueLevelMessage).AddData("issueLevel", issueLevel);

            return il;
        }
    }

    public static class ExceptionExtensions
    {
        public static T AddData<T>(this T item, object key, object value) where T : Exception
        {
            if (item.Data.Contains(key)) item.Data.Remove(key);
            item.Data.Add(key, value);
            return item;
        }
    }
}