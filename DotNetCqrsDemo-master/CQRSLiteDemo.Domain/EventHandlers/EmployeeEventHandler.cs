using AutoMapper;
using CQRSlite.Events;
using CQRSLiteDemo.Domain.Events.Employees;
using CQRSLiteDemo.Domain.ReadModel;
using CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces;


namespace CQRSLiteDemo.Domain.EventHandlers
{
    public class EmployeeEventHandler : IEventHandler<EmployeeCreatedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeEventHandler(
           IMapper mapper, 
           IEmployeeRepository employeeRepo)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
        }

        public void Handle(EmployeeCreatedEvent message)
        {
            EmployeeRM employee = _mapper.Map<EmployeeRM>(message);
            _employeeRepo.Save(employee);
        }
    }
}
