using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IArchiveInfo
    {
        int VersionCount { get; }
        int SessionCount { get; }
        int IssueTypeCount { get; }
        int IssueCount { get; }
    }

    public interface IArchive
    {
        IArchiveInfo Info { get; }
        void Load();
    }

    public interface IEnvironment
    {
        string Name { get; }
        IColor Color { get; }
    }

    public interface IColor
    {
        string Name { get; }
    }

    public interface IIssuelevel
    {
        string Name { get; }
    }

    public interface IMachine
    {
        string Name { get; }
        IDictionary<string, string> Data { get; }

        //Shortcuts
        ISession[] Sessions { get; }
        IUser[] Users { get; }
        IUserHandle[] UserHandles { get; }
        IIssue[] Issues { get; }
        IIssueType[] IssueTypes { get; }
        IVersion[] Versions { get; }
        //IVersion[] ArchivedVersions { get; }
        IApplication[] Applications { get; }
        IProject[] Projects { get; }
    }

    public interface IUser
    {
        string Name { get; }

        //Shortcuts
        ISession[] Sessions { get; }
        IMachine[] Users { get; }
        IIssue[] Issues { get; }
        IIssueType[] IssueTypes { get; }
        IVersion[] Versions { get; }
        //IVersion[] ArchivedVersions { get; }
        IApplication[] Applications { get; }
        //IProject[] Projects { get; }
    }

    public interface IUserHandle
    {
        string Name { get; }
    }

    public interface ISession
    {
        Guid SessionKey { get; }
        DateTime StartTime { get; }
        DateTime? EndTime { get; }
        IEnvironment Environment { get; }
        string CallerIpAddress { get; }
        TimeSpan Duration { get; }
        IUser User { get; }
        IUserHandle UserHandle { get; }
        IMachine Machine { get; }

        //Shortcut / Up-links
        //IIssue[] Issues { get; }
        //IIssueType[] IssueTypes { get; }
        //IVersion Version { get; }
        //IApplication Application { get; }
        IProject Project { get; }
    }

    public interface IIssueType
    {
        string Message { get; }
        IStackTrace StackTrace { get; }
        string Type { get; }
        string ResponseMessage { get; }
        int Ticket { get; }
        IIssuelevel Level { get; }
        IIssueType InnerIssue { get; }

        IEnumerable<IIssue> Issues { get; }

        //Up-links
        IVersion Version { get; }
        IApplication Application { get; }
        IProject Project { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
    }

    public interface IVersion
    {
        string Name { get; }
        DateTime? BuildTime { get; }
        string SupportToolkit { get; }

        IEnumerable<IIssueType> IssueTypes { get; }

        //Up-links
        IApplication Application { get; }
        IProject Project { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
        IIssue[] Issues { get; }
    }

    public interface IApplication
    {
        string Name { get; }

        IEnumerable<IVersion> Versions { get; }
        //IVersion[] ArchivedVersions { get; }
        IArchive Archive { get; }

        //Up-links
        IProject Project { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
        IIssue[] Issues { get; }
        IIssueType[] IssueTypes { get; }
    }

    public interface IProject
    {
        string Name { get; }
        IProjectInfo Info { get; }

        IEnumerable<IApplication> Applications { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
        IIssue[] Issues { get; }
        IIssueType[] IssueTypes { get; }
        IVersion[] Versions { get; }
        //IVersion[] ArchivedVersions { get; }
    }

    public interface IProjectInfo
    {
        Guid ProjectId { get; }
        string Name { get; }
        int VersionCount { get; }
        int SessionCount { get; }
        int IssueTypeCount { get; }
        int IssueCount { get; }
        string DashboardColor { get; }
    }

    public interface IWebApiClient
    {
        Task<TResult> ExecuteGet<T, TResult>(string controller, T id);
        Task<IEnumerable<TResult>> ExecuteGetList<TResult>(string controller);
        Task ExecuteCreateCommandAsync<T>(string controller, T data);
        Task ExecuteCommandAsync<T>(string controller, string action, T data);
        Task<TResult> ExecuteQueryAsync<T, TResult>(string controller, string action, T data);
    }
}