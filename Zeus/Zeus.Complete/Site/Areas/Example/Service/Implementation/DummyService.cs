using System;
using System.Collections.Generic;

using System.Linq;
using Employment.Web.Mvc.Area.Example.Service.Interfaces;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Example.Service.Implementation
{
    /// <summary>
    /// Defines a service for interacting with Employer data.
    /// </summary>
    /// <remarks>
    /// This is for example purposes only. A service should only ever exist in the Service projects.
    /// </remarks>
    public class DummyService : Infrastructure.Services.Service, IDummyService
    {
        private IEnumerable<DummyModel> DummyData;
        private readonly IEnumerable<SortModel> SortData;
        private readonly Random random = new Random();

        private const int TotalRecords = 250;
        private const int MainframePageSize = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="DummyService" /> class.
        /// </summary>
        /// <param name="client">Client for interacting with WCF services.</param>
        /// <param name="cacheService">Cache service for interacting with cached data.</param>
        public DummyService(IClient client,  ICacheService cacheService) : base(client,  cacheService)
        {
            DummyData = GenerateDummyData();
            SortData = GenerateSortData();
        }

        public long Add(string name, DateTime? date, string emailAddress)
        {
            var model = CreateDummyModel(DummyData.Count() + 1);

            model.Name = name;
            model.Date = date;
            model.EmailAddress = emailAddress;

            var list = DummyData.ToList();

            list.Add(model);

            DummyData = list;

            return model.DummyID;
        }

        public void Edit(long id, string name, DateTime? date, string emailAddress)
        {
            var model = Get(id);

            model.Name = name;
            model.Date = date;
            model.EmailAddress = emailAddress;

            var list = DummyData.Where(m => m.DummyID != id).ToList();

            list.Add(model);

            DummyData = list;
        }

        public DummyModel Get(long ID)
        {
            return DummyData.FirstOrDefault(d => d.DummyID == ID);
        }

        /// <summary>
        /// Finds all dummies that match the criteria.
        /// </summary>
        /// <param name="startsWith">Find dummy data that starts with the specified string.</param>
        /// <returns>A collection of <see cref="DummyModel" /> that match the criteria.</returns>
        public IEnumerable<DummyModel> FindAll(string startsWith)
        {
            if (!string.IsNullOrEmpty(startsWith))
            {
                return DummyData.Where(m => m.Name.StartsWith(startsWith, StringComparison.InvariantCultureIgnoreCase));
            }

            return DummyData;
        }

        /// <summary>
        /// Gets all the records that match the name.
        /// </summary>
        /// <param name="name">Specified string for search.</param>
        /// <returns>A collection of <see cref="SortModel"/> that match the criteria.</returns>
        public IEnumerable<SortModel> GetAllForSorting(string name)
        {
            if(!String.IsNullOrEmpty(name))
            {
                return SortData.Where(m => m.Address.ToLower().Contains(name.ToLower()));
            }
            return SortData;
        }

        /// <summary>
        /// Finds dummies that match the criteria. Basic sumulation of mainframe paging.
        /// </summary>
        /// <param name="startsWith">Find dummy data that starts with the specified string.</param>
        /// <param name="nextSequenceID">Starting ID of next sequence (used by mainframe for retrieving next page).</param>
        /// <returns>A collection of <see cref="DummyModel" /> that match the criteria.</returns>
        public DummiesModel Find(string startsWith, long nextSequenceID)
        {
            var model = new DummiesModel();

            // The following filtering and paging would actually be done in mainframe
            var data = DummyData;

            // Filter data
            if (!string.IsNullOrEmpty(startsWith))
            {
                data = data.Where(m => m.Name.StartsWith(startsWith, StringComparison.InvariantCultureIgnoreCase));
            }

            // Page data
            int skip = 0;
            if (nextSequenceID > 0)
            {
                foreach (var d in data)
                {
                    if (d.DummyID != nextSequenceID)
                    {
                        skip++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            model.Dummies = data.Skip(skip).Take(MainframePageSize);

            // Determine the starting sequence ID of the next page
            var nextSequence = data.Skip(skip + MainframePageSize).Take(1).FirstOrDefault();

            if (nextSequence != null)
            {
                model.NextSequenceID = nextSequence.DummyID;
            }

            return model;
        }

        private List<string> dictionary = new List<string> { "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "duis", "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", "velit", "esse", "molestie", "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", "vero", "eros", "et", "accumsan", "et", "iusto", "odio", "dignissim", "qui", "blandit", "praesent", "luptatum", "zzril", "delenit", "augue", "duis", "dolore", "te", "feugait", "nulla", "facilisi", "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat", "volutpat", "ut", "wisi", "enim", "ad", "minim", "veniam", "quis", "nostrud", "exerci", "tation", "ullamcorper", "suscipit", "lobortis", "nisl", "ut", "aliquip", "ex", "ea", "commodo", "consequat", "duis", "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", "velit", "esse", "molestie", "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", "vero", "eros", "et", "accumsan", "et", "iusto", "odio", "dignissim", "qui", "blandit", "praesent", "luptatum", "zzril", "delenit", "augue", "duis", "dolore", "te", "feugait", "nulla", "facilisi", "nam", "liber", "tempor", "cum", "soluta", "nobis", "eleifend", "option", "congue", "nihil", "imperdiet", "doming", "id", "quod", "mazim", "placerat", "facer", "possim", "assum", "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat", "volutpat", "ut", "wisi", "enim", "ad", "minim", "veniam", "quis", "nostrud", "exerci", "tation", "ullamcorper", "suscipit", "lobortis", "nisl", "ut", "aliquip", "ex", "ea", "commodo", "consequat", "duis", "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", "velit", "esse", "molestie", "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "at", "accusam", "aliquyam", "diam", "diam", "dolore", "dolores", "duo", "eirmod", "eos", "erat", "et", "nonumy", "sed", "tempor", "et", "et", "invidunt", "justo", "labore", "stet", "clita", "ea", "et", "gubergren", "kasd", "magna", "no", "rebum", "sanctus", "sea", "sed", "takimata", "ut", "vero", "voluptua", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum" };

        /// <summary>
        /// Get random lorem ipsum words.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string GetWords(int number)
        {
            var words = new List<string>();

            var random = new Random();

            for (int i = 0; i <= number; i++)
            {
                var word = dictionary.OrderBy(m => m).Skip(random.Next(dictionary.Count - 1)).Take(1).FirstOrDefault();

                if (word != null)
                {
                    words.Add(word);
                }
            }

            return string.Join(" ", words);
        }

        private IEnumerable<DummyModel> GenerateDummyData()
        {
            var models = new List<DummyModel>();

            for (int i = 1; i <= TotalRecords; i++)
            {
                var model = new DummyModel();

                model.DummyID = i;
                model.Name = GetWords(random.Next(1, 2)) + " --- " + i;
                model.Description = GetWords(random.Next(3, 5)) + " --- " + i;
                if (i % 10 == 0)
                {
                    model.Date = null;
                    model.DateTime = null;
                    model.Currency = null;
                }
                else
                {
                    model.Date = new DateTime(random.Next(1970, 2001), random.Next(10, 12), random.Next(1, 29));
                    model.DateTime = new DateTime(random.Next(1970, 2001), random.Next(10, 12), random.Next(1, 29),
                                                  random.Next(0, 23), random.Next(0, 59), random.Next(0, 59));
                    model.Currency = random.Next(1, 1000);
                }
                
                
                model.EmailAddress = GetWords(random.Next(3, 5)) + "@" + GetWords(random.Next(1, 2)) + ".com" + " --- " + i;
                model.Time = new DateTime(1970, 1,1, random.Next(0,23), random.Next(0,59), random.Next(0,59));
                model.Url = "http://www." + GetWords(random.Next(3, 5)) + "." + GetWords(1) + " --- " + i;
                model.Decimal1 = (float) random.NextDouble();
                models.Add(CreateDummyModel(i));
            }

            return models.OrderBy(m => m.DummyID);
        }

        private DummyModel CreateDummyModel(int i)
        {
            var model = new DummyModel();

            model.DummyID = i;
            model.Name = GetWords(random.Next(1, 2)) + " --- " + i;
            model.Description = GetWords(random.Next(3, 5)) + " --- " + i;
            if (i % 10 == 0)
            {
                model.Date = null;
                model.DateTime = null;
                model.Currency = null;
            }
            else
            {
                model.Date = new DateTime(random.Next(1970, 2001), random.Next(10, 12), random.Next(1, 29));
                model.DateTime = new DateTime(random.Next(1970, 2001), random.Next(10, 12), random.Next(1, 29),
                                              random.Next(0, 23), random.Next(0, 59), random.Next(0, 59));
                model.Currency = random.Next(1, 1000);
            }


            model.EmailAddress = GetWords(random.Next(3, 5)) + "@" + GetWords(random.Next(1, 2)) + ".com" + " --- " + i;
            model.Time = new DateTime(1970, 1, 1, random.Next(0, 23), random.Next(0, 59), random.Next(0, 59));
            model.Url = "http://www." + GetWords(random.Next(3, 5)) + "." + GetWords(1) + " --- " + i;
            model.Decimal1 = (float)random.NextDouble();

            return model;
        }

        private IEnumerable<SortModel> GenerateSortData()
        {
            var models = new List<SortModel>();
            var random = new Random();
            for (int i = 0; i < TotalRecords; i++)
            {
                var singleModel = new SortModel
                                      {
                                          SortingID = i,
                                          Address = GetWords(random.Next(1, 4)) + "----" + i,
                                          HtmlContent =
                                              "<a href=\"" + i + "\" title= '" + i + "' >" + GetWords(2) + "</a>",
                                          ImageUrl = "http://localhost:7000/content/layout/favicon.gif"
                                      };
                if(i % 10 == 0)
                {
                    singleModel.Date = null;
                    singleModel.Duration = null;
                }
                else
                {
                    singleModel.Date = new DateTime(random.Next(1000,2001), random.Next(1,12), random.Next(1, 28));
                    singleModel.Duration = (int) ((DateTime.MaxValue - singleModel.Date).Value.TotalDays);
                }
                singleModel.LargeTextArea = GetWords(random.Next(1, 20));
                singleModel.Password = GetWords(random.Next(1, 5));
                singleModel.Time = DateTime.Now.AddDays(1);
                singleModel.PhoneNumber = random.Next(1, 12) + " " + random.Next(10000, 100000);
                

                models.Add((singleModel));
            }
            return models;
        }


    }
}
