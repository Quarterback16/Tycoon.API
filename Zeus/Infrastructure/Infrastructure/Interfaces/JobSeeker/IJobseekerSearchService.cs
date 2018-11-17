using Employment.Web.Mvc.Infrastructure.Models.JobSeeker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Interfaces.JobSeeker
{
    /// <summary>
    /// Represents a service for Job Seeker search.
    /// </summary>
    public interface IJobseekerSearchService
    {
        /// <summary>
        /// Searches for JobSeeker records.
        /// </summary>
        /// <param name="text">Text entered in Search box.</param>
        /// <returns>Collection of Jobseeker search models.</returns>
        IList<JobSeekerSearchModel> SearchJobSeeker(string text = null);


        /// <summary>
        /// Searches for JobSeeker records.
        /// </summary>
        /// <param name="model">Instance of <see cref="JobSeekerSearchModel"/>.</param>
        /// <returns>Collection of Jobseeker search models.</returns>
        IList<JobSeekerSearchModel> SearchJobSeeker(JobSeekerSearchModel model);



    }
}
