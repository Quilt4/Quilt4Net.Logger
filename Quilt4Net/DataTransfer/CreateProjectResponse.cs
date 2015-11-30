using System;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class CreateProjectResponse
    {
        public Guid ProjectKey { get; set; }
        public string Name { get; set; }
        public string ProjectApiKey { get; set; }
    }
}