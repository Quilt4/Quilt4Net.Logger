namespace Tharga.Quilt4Net.DataTransfer
{
    public class ProjectResponse
    {
        public string Name { get; set; }
        public ProjectInfo Info { get; set; }
        public ApplicationResponse[] Applications { get; set; }
        public VersionResponse[] Versions { get; set; }
        public IssueTypeResponse[] IssueTypes { get; set; }
        public IssueResponse[] Issues { get; set; }
        public SessionResponse[] Sessions { get; set; }
        public UserResponse[] Users { get; set; }
        public UserHandleResponse[] UserHandles { get; set; }
        public MachineResponse[] Machines { get; set; }
    }
}