namespace Employment.Web.Mvc.Service.Interfaces.Notification
{
    public  class SMSModel
    {
        public long JobseekerID { get; set; }

        public string ContractType { get; set; }

        public long MessageId { get; set; }

        public string Phone { get; set; }
        
        public string Message { get; set; }
    }
}
