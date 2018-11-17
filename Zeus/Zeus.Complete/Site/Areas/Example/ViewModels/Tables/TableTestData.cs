using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Example.ViewModels
{
    public class FooData
    {

        #region Data String

        private string jsonData =
"[{\"JobseekerId\":\"54050005\",\"Surname\":\"Stark\",\"FirstName\":\"Tony\",\"BirthDate\":\"1969/05/03\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 25/06/2014\",\"ContactNumber\":\"457294257\",\"PlacementStatus\":\"Suspended\",\"Postcode\":\"5631\",\"Suburb\":\"CUMMINS\"}," +
"{\"JobseekerId\":\"146120005\",\"Surname\":\"Allen\",\"FirstName\":\"Barry\",\"BirthDate\":\"1975/12/19\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 29/05/2014\",\"ContactNumber\":\"312312312\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"2612\",\"Suburb\":\"REID\"}," +
"{\"JobseekerId\":\"200514019\",\"Surname\":\"Banner\",\"FirstName\":\"Robert\",\"BirthDate\":\"1972/07/03\",\"NextAppointmentDate\":\"8/09/2014 0:00\",\"AllowanceType\":\"STP\",\"EPPStatus\":\"No IPP\",\"ContactNumber\":\"\",\"PlacementStatus\":\"Pending\",\"Postcode\":\"5605\",\"Suburb\":\"TUMBY BAY\"}," +
"{\"JobseekerId\":\"200553019\",\"Surname\":\"Rogers\",\"FirstName\":\"Steven\",\"BirthDate\":\"1990/01/01\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 22/08/2014\",\"ContactNumber\":\"232132132\",\"PlacementStatus\":\"Suspended\",\"Postcode\":\"5605\",\"Suburb\":\"TUMBY BAY\"}," +
"{\"JobseekerId\":\"216090005\",\"Surname\":\"Barton\",\"FirstName\":\"Clinton\",\"BirthDate\":\"1953/03/05\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 21/11/2013\",\"ContactNumber\":\"886255005\",\"PlacementStatus\":\"Suspended\",\"Postcode\":\"5671\",\"Suburb\":\"PORT KENNY\"}," +
"{\"JobseekerId\":\"246500001\",\"Surname\":\"Maximoff\",\"FirstName\":\"Wanda\",\"BirthDate\":\"1972/08/15\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"\",\"EPPStatus\":\"Approved 11/07/2014\",\"ContactNumber\":\"423313686\",\"PlacementStatus\":\"Exited\",\"Postcode\":\"5607\",\"Suburb\":\"COFFIN BAY\"}," +
"{\"JobseekerId\":\"481441709\",\"Surname\":\"Whitman\",\"FirstName\":\"Dane\",\"BirthDate\":\"1961/10/31\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 15/04/2014\",\"ContactNumber\":\"886881951\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"5605\",\"Suburb\":\"TUMBY BAY\"}," +
"{\"JobseekerId\":\"583143409\",\"Surname\":\"Romanova\",\"FirstName\":\"Natalia\",\"BirthDate\":\"1954/12/06\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 4/06/2014\",\"ContactNumber\":\"421469947\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"5641\",\"Suburb\":\"KIMBA\"}," +
"{\"JobseekerId\":\"685075609\",\"Surname\":\"McCoy\",\"FirstName\":\"Henry\",\"BirthDate\":\"1951/01/27\",\"NextAppointmentDate\":\"25/07/2014 0:00\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 23/06/2014\",\"ContactNumber\":\"886854385\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"5607\",\"Suburb\":\"WANGARY\"}," +
"{\"JobseekerId\":\"687424309\",\"Surname\":\"Danvers\",\"FirstName\":\"Carol\",\"BirthDate\":\"1952/04/14\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 15/10/2013\",\"ContactNumber\":\"447160571\",\"PlacementStatus\":\"Suspended\",\"Postcode\":\"5605\",\"Suburb\":\"TUMBY BAY\"}," +
"{\"JobseekerId\":\"725561909\",\"Surname\":\"Wayne\",\"FirstName\":\"Bruce\",\"BirthDate\":\"1996/01/18\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"YAL\",\"EPPStatus\":\"No IPP\",\"ContactNumber\":\"\",\"PlacementStatus\":\"Exited\",\"Postcode\":\"2612\",\"Suburb\":\"REID\"}," +
"{\"JobseekerId\":\"735670005\",\"Surname\":\"\",\"FirstName\":\"Oliver\",\"BirthDate\":\"1970/01/23\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"No IPP\",\"ContactNumber\":\"437423238\",\"PlacementStatus\":\"Exited\",\"Postcode\":\"5606\",\"Suburb\":\"PORT LINCOLN\"}," +
"{\"JobseekerId\":\"746910005\",\"Surname\":\"Urich\",\"FirstName\":\"Phillip\",\"BirthDate\":\"1969/12/02\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 20/02/2014\",\"ContactNumber\":\"886275061\",\"PlacementStatus\":\"Suspended\",\"Postcode\":\"5641\",\"Suburb\":\"KIMBA\"}," +
"{\"JobseekerId\":\"772119309\",\"Surname\":\"Parker\",\"FirstName\":\"Peter\",\"BirthDate\":\"1964/12/26\",\"NextAppointmentDate\":\"23/07/2014 0:00\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 2/07/2014\",\"ContactNumber\":\"886843548\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"5607\",\"Suburb\":\"NORTH SHIELDS\"}," +
"{\"JobseekerId\":\"807920001\",\"Surname\":\"Kent\",\"FirstName\":\"Clark\",\"BirthDate\":\"1951/09/06\",\"NextAppointmentDate\":\"31/07/2014 0:00\",\"AllowanceType\":\"\",\"EPPStatus\":\"Approved 3/07/2014\",\"ContactNumber\":\"323213214\",\"PlacementStatus\":\"Exited\",\"Postcode\":\"5652\",\"Suburb\":\"WUDINNA\"}," +
"{\"JobseekerId\":\"840334609\",\"Surname\":\"Wagner\",\"FirstName\":\"Kurt\",\"BirthDate\":\"1949/08/03\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"\",\"EPPStatus\":\"Approved 1/04/2014\",\"ContactNumber\":\"886292313\",\"PlacementStatus\":\"Exited\",\"Postcode\":\"5602\",\"Suburb\":\"COWELL\"}," +
"{\"JobseekerId\":\"874783809\",\"Surname\":\"Marie\",\"FirstName\":\"Anna\",\"BirthDate\":\"1993/07/09\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"YAL\",\"EPPStatus\":\"Approved 20/06/2014\",\"ContactNumber\":\"437916239\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"5607\",\"Suburb\":\"HAWSON\"}," +
"{\"JobseekerId\":\"914136809\",\"Surname\":\"Garrick\",\"FirstName\":\"Jay\",\"BirthDate\":\"1991/05/10\",\"NextAppointmentDate\":\"4/08/2014 0:00\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 17/03/2014\",\"ContactNumber\":\"886285068\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"5640\",\"Suburb\":\"CLEVE\"}," +
"{\"JobseekerId\":\"982840005\",\"Surname\":\"Troy\",\"FirstName\":\"Donna\",\"BirthDate\":\"1952/05/21\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 8/07/2014\",\"ContactNumber\":\"886462095\",\"PlacementStatus\":\"Commenced\",\"Postcode\":\"5601\",\"Suburb\":\"IRON KNOB\"}," +
"{\"JobseekerId\":\"1002879709\",\"Surnam:\":\"Scott\",\"FirstName\":\"Alan\",\"BirthDate\":\"1952/09/11\",\"NextAppointmentDate\":\"\",\"AllowanceType\":\"NSA\",\"EPPStatus\":\"Approved 3/02/2014\",\"ContactNumber\":\"427751739\",\"PlacementStatus\":\"Suspended\",\"Postcode\":\"5607\",\"Suburb\":\"WANILLA\"}]";

        #endregion

        private TestData testData = null;

        public FooData()
        {
            var t = JsonConvert.DeserializeObject<List<GridRowData>>(this.jsonData);


            testData = new TestData() 
            {
                GridRows = JsonConvert.DeserializeObject<List<GridRowData>>(this.jsonData)
                
            };

            //testData.GridRows.RemoveAt(999);
            //testData.GridRows.RemoveAt(998);

        }

        public IEnumerable<GridRowData> AllGridData(int pageNumber, int numItemsPerPage)
        {
            if (testData == null)
                return new List<GridRowData>();

            int skip = --pageNumber;
            if (skip == -1)
                skip = 0;

            return testData.GridRows
                .OrderBy(r => r.JobseekerId)
                .Skip(skip * numItemsPerPage)
                .Take(numItemsPerPage);
        }

        public IEnumerable<GridRowData> SelectGridData(int pageNumber, int numItemsPerPage, Func<GridRowData, bool> predicate)
        {
            if(testData == null)
                return new List<GridRowData>();

            int skip = pageNumber--;
            if(skip == -1)
                skip = 0;

            return testData.GridRows
                .Where(predicate)
                .OrderBy(r => r.Surname)
                .Skip(skip * numItemsPerPage)
                .Take(numItemsPerPage);
        }


    }


    ///////////////////////////
    // JSON test data classes
    ///////////////////////////
    public class TestData
    {
        public List<GridRowData> GridRows { get; set; }
    }

    public class GridRowData
    {

        public string JobseekerId { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NextAppointment { get; set; }
        public string Allowance { get; set; }
        public string EPPStatus { get; set; }
        public string ContactNumber { get; set; }
        public string Placement { get; set; }
        public string Status { get; set; }
        public string Postcode { get; set; }
        public string Suburb { get; set; }

    }

}












