using Employment.Web.Mvc.Infrastructure.Interfaces.JobSeeker;
using Employment.Web.Mvc.Infrastructure.Models.JobSeeker;
using Intech.Search.IQGlobal;
using Intech.Search.Models.JobSeeker;
using Intech.Search.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Web.Mvc.Infrastructure.Mappers;


namespace Employment.Web.Mvc.Infrastructure.Services.JobSeeker
{
    /// <summary>
    /// The implementation of Jobseeker Search service.
    /// </summary>
    public class JobSeekerSearchService : IJobseekerSearchService
    {

        // TODo: add Job seeker search functions.

        // Usage

        /// <summary>
        /// Searches for JobSeeker records.
        /// </summary>
        /// <param name="text">Text entered in Search box.</param>
        /// <returns>Collection of Jobseeker search models.</returns>
        public IList<JobSeekerSearchModel> SearchJobSeeker(string text = null) //IList<JobSeekerSearchModel>
        {

            // TODO: Derive model from the 'search' text.


            bool hitMax;
            int errorCode;
            string errorMsg;
            IQJsEntity jsEntity = new IQJsEntity();

            if (!string.IsNullOrEmpty(text))
            {
                long id = 0;
                if (Int64.TryParse(text, out id))
                {
                    jsEntity.Id = text;
                }
                else
                {
                    var separatedValues = text.Split(new char[] { ' ' });
                    if (separatedValues.Length > 0)
                    {
                        jsEntity.FirstName = separatedValues[0] ?? "John";
                        if(separatedValues.Length > 1)
                        { 
                                jsEntity.LastName = separatedValues[1];
                        }
                        if(separatedValues.Length > 2)
                        { 
                                jsEntity.Gender = separatedValues[2];
                        }                          
                    }
                    else
                    {
                        jsEntity.FirstName = text;
                    }
                }
            }
            else
            {
                jsEntity.LastName = "Smith";
                jsEntity.LastName = "Smith";
            }
             

            IList<IQJsEntity> result = IQOfficeProcess.MatchRecordJs(IQGlobalSetting.JS_MATCH_SPEC,
                        jsEntity, out hitMax, out errorCode, out errorMsg);

            IList<JobSeekerSearchModel> results = new List<JobSeekerSearchModel>();

            foreach (var record in result)
            {
                JobSeekerSearchModel jskSearchModel = new JobSeekerSearchModel
                {
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    MiddleName = record.MiddleName,
                    Salutation = record.Salutation,
                    Type = record.Type,
                    Id = record.Id,
                    Gender = record.Gender,
                    DOB = record.DOB
                };
                results.Add(jskSearchModel);
            }
            return results;
        }


        public IList<JobSeekerSearchModel> SearchJobSeeker(JobSeekerSearchModel model)
        {
            bool hitMax = true;
            string errorMsg = string.Empty;
            int errorCode = 0;

            IQJsEntity jskModel = model.ToIQJsEntity();

            IList<IQJsEntity> result = IQOfficeProcess.MatchRecordJs(IQGlobalSetting.JS_MATCH_SPEC, jskModel, out hitMax, out errorCode, out errorMsg);

            IList<JobSeekerSearchModel> results = new List<JobSeekerSearchModel>();

            foreach (var record in result)
            {                
                results.Add(record.ToJobSeekerSearchModel());
            }

            return results;
        }
    }
}
