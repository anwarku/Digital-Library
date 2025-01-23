﻿using AutoMapper;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Mapper
{
    public class MappingProfile
    {
    }

    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile() 
        {
            CreateMap<BorrowedTransactionDto, Transaction>();
        }
    }

    public class EfToDtoMappingProfile : Profile 
    {
        public EfToDtoMappingProfile() 
        {
            CreateMap<Transaction, BorrowedTransactionDto>();
        }
    }
}
