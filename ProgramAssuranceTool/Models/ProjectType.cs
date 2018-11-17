using System.Collections.Generic;

namespace ProgramAssuranceTool.Models
{
    public class ProjectType
    {
        public List<ProjectTypeDetail> ProjectByOrganisation { get; set; }

        public List<ProjectTypeDetail> ProjectByType { get; set; }

        public List<ProjectTypeDetail> ProjectByESA { get; set; }

        public List<ProjectTypeDetail> ProjectByState { get; set; }

        public List<ProjectTypeDetail> ProjectByNational { get; set; }

        public ProjectType()
        {
            ProjectByOrganisation = new List<ProjectTypeDetail>();
            ProjectByType = new List<ProjectTypeDetail>();
            ProjectByESA = new List<ProjectTypeDetail>();
            ProjectByState = new List<ProjectTypeDetail>();
            ProjectByNational = new List<ProjectTypeDetail>();
        }
    }

    public class ProjectTypeDetail
    {
        public string Type { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int ProjectCount { get; set; }

        public int ReviewCount { get; set; }
    }
}