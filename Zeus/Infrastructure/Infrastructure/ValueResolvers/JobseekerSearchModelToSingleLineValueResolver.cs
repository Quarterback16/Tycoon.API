using Employment.Web.Mvc.Infrastructure.Models.JobSeeker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    public static class JobseekerSearchModelToSingleLineValueResolver
    {

        public static string Resolve(JobSeekerSearchModel model)
        {
            if(string.IsNullOrWhiteSpace(model.FirstName))
            {
                return string.Empty;
            }

            if(string.IsNullOrWhiteSpace(model.MiddleName))
            {
                return model.Id + " " + model.FirstName + " " + model.LastName;
            }
            if(string.IsNullOrEmpty(model.Gender))
            {
                return string.Format("{0} {1} {2} {3} {4}", model.Id, model.Salutation, model.FirstName, model.MiddleName, model.LastName);
            }

            return string.Format("{0} {1} {2} {3} {4} {5}", model.Id, model.Salutation, model.FirstName, model.MiddleName, model.LastName, model.Gender == "M" ? "(Male)" : model.Gender == "F" ? "(Female)" : model.Gender);

        }
    }
}
