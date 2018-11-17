using Employment.Web.Mvc.Infrastructure.Models.JobSeeker;
using Employment.Web.Mvc.Infrastructure.ValueResolvers;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker;
using Intech.Search.Models.JobSeeker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Mappers
{
    /// <summary>
    /// Represents a Mapper class to carry out mappings in JobSeeker search.
    /// </summary>
    public static class JobSeekerSearchMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerable<AjaxJobseekerSearchViewModel> ToAjaxJobseekerSearchViewModelList(this IList<JobSeekerSearchModel> data)
        {
            if(data != null)
            { 
                var modelList = new List<AjaxJobseekerSearchViewModel>();

                foreach(var record in data)
                {
                    AjaxJobseekerSearchViewModel model = record.ToAjaxJobseekerSearchModel();

                    modelList.Add(model);
                }

                return modelList as IEnumerable<AjaxJobseekerSearchViewModel>;
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AjaxJobseekerSearchViewModel ToAjaxJobseekerSearchModel(this JobSeekerSearchModel model)
        {
            if(model != null)
            {
                AjaxJobseekerSearchViewModel ajaxModel = new AjaxJobseekerSearchViewModel
                {
                    DOB = model.DOB,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    Id = model.Id,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Salutation = model.Salutation,
                    SingleLineJobseekerSearch = JobseekerSearchModelToSingleLineValueResolver.Resolve(model), // model.Id + " " + model.Salutation + " " + model.FirstName + " "+ " " + model.MiddleName + model.LastName,
                    Type = model.Type
                };

                return ajaxModel;
            }

            return null;

        }



        public static IQJsEntity ToIQJsEntity(this JobSeekerSearchModel model)
        {
            if(model != null)
            {
                return new IQJsEntity
                {
                    DOB = model.DOB,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    Id = model.Id,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    PreferredName = model.PreferedName,
                    Status = model.Status,
                    Type = model.Type,
                    
                };
            }
            return null;
        }


        public static JobSeekerSearchModel ToJobSeekerSearchModel(this IQJsEntity record)
        {
            if(record == null)
            {
                return null;
            }

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

            return jskSearchModel;
        }
    }
}
