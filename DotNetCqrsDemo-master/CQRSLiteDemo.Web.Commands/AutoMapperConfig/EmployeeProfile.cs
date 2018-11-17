﻿using AutoMapper;
using CQRSLiteDemo.Domain.Commands;
using CQRSLiteDemo.Domain.Events.Employees;
using CQRSLiteDemo.Domain.ReadModel;
using CQRSLiteDemo.Web.Commands.Requests.Employees;
using System;

namespace CQRSLiteDemo.Web.Commands.AutoMapperConfig
{
   public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeRequest, CreateEmployeeCommand>()
                .ConstructUsing(x => new CreateEmployeeCommand(Guid.NewGuid(), x.EmployeeID, x.FirstName, x.LastName, x.DateOfBirth, x.JobTitle));

            CreateMap<EmployeeCreatedEvent, EmployeeRM>()
                .ForMember(dest => dest.AggregateID, opt => opt.MapFrom(src => src.Id));
        }
    }
}