using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Tests
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public async void When_getting_project_with_one_issue()
        {
            //Arrange
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            var client = new Client(webApiClientMock.Object);
            Guid sessionKey = Guid.NewGuid();
            var applicationName1 = "App1";
            var versionName1 = "1.0.0.0";
            var issueTypeMessage1 = "ABC";
            var userName = "BobLoblaw";
            var userHandleName = "ABC123";
            var projectResponse = new ProjectResponse
            {
                Applications = new[] { new ApplicationResponse { Name = applicationName1 } },
                Versions = new[] { new VersionResponse { ApplicationName = applicationName1, Name = versionName1 }, },
                IssueTypes = new[] { new IssueTypeResponse { ApplicationName = applicationName1, VersionName = versionName1, Message = issueTypeMessage1 } },
                Issues = new[] { new IssueResponse { ApplicationName = applicationName1, VersionName = versionName1, IssueTypeMessage = issueTypeMessage1, IssueTime = DateTime.UtcNow, SessionKey = sessionKey }, },
                Sessions = new[] { new SessionResponse { ApplicationName = applicationName1, SessionKey = sessionKey, VersionName = versionName1, UserName = userName, UserHandleName = userHandleName}, },
                Users = new[] {new UserResponse { UserName = userName }, },
                UserHandles = new[] { new UserHandleResponse { Name = userHandleName }, }
            };
            webApiClientMock.Setup(x => x.ExecuteGet<Guid, ProjectResponse>("project", It.IsAny<Guid>())).Returns(() => Task.FromResult(projectResponse));
            var projectId = Guid.NewGuid();

            //Act
            var proj = await client.Project.GetAsync(projectId);

            //Assert
            Assert.That(proj, Is.Not.Null);
            Assert.That(proj.Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications, Is.Not.Null);
            Assert.That(proj.Applications, Is.Not.Empty);
            Assert.That(proj.Applications.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues.Count(), Is.EqualTo(1));
        }

        [Test]
        public async void When_getting_project_with_two_issues()
        {
            //Arrange
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            var client = new Client(webApiClientMock.Object);
            Guid sessionKey = Guid.NewGuid();
            var applicationName1 = "App1";
            var versionName1 = "1.0.0.0";
            var issueTypeMessage1 = "ABC";
            var userName = "BobLoblaw";
            var projectResponse = new ProjectResponse
            {
                Applications = new[] { new ApplicationResponse { Name = applicationName1 } },
                Versions = new[] { new VersionResponse { ApplicationName = applicationName1, Name = versionName1 }, },
                IssueTypes = new[] { new IssueTypeResponse { ApplicationName = applicationName1, VersionName = versionName1, Message = issueTypeMessage1 } },
                Issues = new[]
                {
                    new IssueResponse { ApplicationName = applicationName1, VersionName = versionName1, IssueTypeMessage = issueTypeMessage1, IssueTime = DateTime.UtcNow, SessionKey = sessionKey },
                    new IssueResponse { ApplicationName = applicationName1, VersionName = versionName1, IssueTypeMessage = issueTypeMessage1, IssueTime = DateTime.UtcNow, SessionKey = sessionKey },
                },
                Sessions = new[] { new SessionResponse { ApplicationName = applicationName1, SessionKey = sessionKey, VersionName = versionName1, UserName = userName }, },
                Users = new[] { new UserResponse { UserName = userName }, },
                UserHandles = new UserHandleResponse[] { }
            };
            webApiClientMock.Setup(x => x.ExecuteGet<Guid, ProjectResponse>("project", It.IsAny<Guid>())).Returns(() => Task.FromResult(projectResponse));
            var projectId = Guid.NewGuid();

            //Act
            var proj = await client.Project.GetAsync(projectId);

            //Assert
            Assert.That(proj, Is.Not.Null);
            Assert.That(proj.Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications, Is.Not.Null);
            Assert.That(proj.Applications, Is.Not.Empty);
            Assert.That(proj.Applications.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues.Count(), Is.EqualTo(2));
        }

        [Test]
        public async void When_getting_project_with_two_issues_in_different_sessions()
        {
            //Arrange
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            var client = new Client(webApiClientMock.Object);
            var sessionKey1 = Guid.NewGuid();
            var sessionKey2 = Guid.NewGuid();
            var applicationName1 = "App1";
            var versionName1 = "1.0.0.0";
            var issueTypeMessage1 = "ABC";
            var userName = "BobLoblaw";
            var projectResponse = new ProjectResponse
            {
                Applications = new[] { new ApplicationResponse { Name = applicationName1 } },
                Versions = new[] { new VersionResponse { ApplicationName = applicationName1, Name = versionName1 }, },
                IssueTypes = new[] { new IssueTypeResponse { ApplicationName = applicationName1, VersionName = versionName1, Message = issueTypeMessage1 } },
                Issues = new[]
                {
                    new IssueResponse { ApplicationName = applicationName1, VersionName = versionName1, IssueTypeMessage = issueTypeMessage1, IssueTime = DateTime.UtcNow, SessionKey = sessionKey1 },
                    new IssueResponse { ApplicationName = applicationName1, VersionName = versionName1, IssueTypeMessage = issueTypeMessage1, IssueTime = DateTime.UtcNow, SessionKey = sessionKey2 },
                },
                Sessions = new[]
                {
                    new SessionResponse { ApplicationName = applicationName1, SessionKey = sessionKey1, VersionName = versionName1, UserName = userName },
                    new SessionResponse { ApplicationName = applicationName1, SessionKey = sessionKey2, VersionName = versionName1, UserName = userName },
                },
                Users = new[] { new UserResponse { UserName = userName }, },
                UserHandles = new UserHandleResponse[] { },
            };
            webApiClientMock.Setup(x => x.ExecuteGet<Guid, ProjectResponse>("project", It.IsAny<Guid>())).Returns(() => Task.FromResult(projectResponse));
            var projectId = Guid.NewGuid();

            //Act
            var proj = await client.Project.GetAsync(projectId);

            //Assert
            Assert.That(proj, Is.Not.Null);
            Assert.That(proj.Sessions.Count(), Is.EqualTo(2));
            Assert.That(proj.Applications, Is.Not.Null);
            Assert.That(proj.Applications, Is.Not.Empty);
            Assert.That(proj.Applications.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Sessions.Count(), Is.EqualTo(2));
            Assert.That(proj.Applications.First().Versions, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().Sessions.Count(), Is.EqualTo(2));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Sessions.Count(), Is.EqualTo(2));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues.Count(), Is.EqualTo(2));
        }

        [Test]
        public async void When_getting_project_with_two_issues_in_different_sessions_and_different_applications()
        {
            //Arrange
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            var client = new Client(webApiClientMock.Object);
            var sessionKey1 = Guid.NewGuid();
            var sessionKey2 = Guid.NewGuid();
            var applicationName1 = "App1";
            var applicationName2 = "App2";
            var versionName1 = "1.0.0.0";
            var versionName2 = "1.0.0.1";
            var issueTypeMessage1 = "ABC";
            var userName1 = "BobLoblaw";
            var userName2 = "Reapadda";
            var projectResponse = new ProjectResponse
            {
                Applications = new[]
                {
                    new ApplicationResponse { Name = applicationName1 },
                    new ApplicationResponse { Name = applicationName1 },
                },
                Versions = new[]
                {
                    new VersionResponse { ApplicationName = applicationName1, Name = versionName1 },
                    new VersionResponse { ApplicationName = applicationName2, Name = versionName2 },
                },
                IssueTypes = new[] { new IssueTypeResponse { ApplicationName = applicationName1, VersionName = versionName1, Message = issueTypeMessage1 } },
                Issues = new[]
                {
                    new IssueResponse { ApplicationName = applicationName1, VersionName = versionName1, IssueTypeMessage = issueTypeMessage1, IssueTime = DateTime.UtcNow, SessionKey = sessionKey1 },
                    new IssueResponse { ApplicationName = applicationName2, VersionName = versionName2, IssueTypeMessage = issueTypeMessage1, IssueTime = DateTime.UtcNow, SessionKey = sessionKey2 },
                },
                Sessions = new[]
                {
                    new SessionResponse { ApplicationName = applicationName1, SessionKey = sessionKey1, VersionName = versionName1, UserName = userName1 },
                    new SessionResponse { ApplicationName = applicationName2, SessionKey = sessionKey2, VersionName = versionName1, UserName = userName2 },
                },
                Users = new[] { new UserResponse { UserName = userName1 }, new UserResponse { UserName = userName2 }, },
                UserHandles = new UserHandleResponse[] { },
            };
            webApiClientMock.Setup(x => x.ExecuteGet<Guid, ProjectResponse>("project", It.IsAny<Guid>())).Returns(() => Task.FromResult(projectResponse));
            var projectId = Guid.NewGuid();

            //Act
            var proj = await client.Project.GetAsync(projectId);

            //Assert
            Assert.That(proj, Is.Not.Null);
            Assert.That(proj.Sessions.Count(), Is.EqualTo(2));
            Assert.That(proj.Sessions.First().Project, Is.Not.Null);
            //Assert.That(proj.Sessions.First().Application, Is.Not.Null);
            Assert.That(proj.Applications, Is.Not.Null);
            Assert.That(proj.Applications, Is.Not.Empty);
            Assert.That(proj.Applications.Count(), Is.EqualTo(2));
            Assert.That(proj.Applications.First().Sessions.Count(), Is.EqualTo(2));
            Assert.That(proj.Applications.First().Versions, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().Sessions.Count(), Is.EqualTo(2));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Sessions.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Null);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues, Is.Not.Empty);
            Assert.That(proj.Applications.First().Versions.First().IssueTypes.First().Issues.Count(), Is.EqualTo(1));
            Assert.That(proj.Applications.Last().Versions.Last().IssueTypes.Last().Issues.Count(), Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class SessionTests
    {
        [Test]
        public void x()
        {
            //Arrange
            //var session = new Session("c16afb009e283eec78a5e3048315123a");            
            //session.Authorize("5eaf7bcb45ade283641e761307b561756ba5fb7acb70d6e295d5edc62095632b");

            //var me = session.Members.Me();
            //var boards = session.Projects.ForMember(me);

            //Act
            //var response = session.Register();

            //Assert
            //Assert.That(response, Is.Not.Null);
        }
    }
}