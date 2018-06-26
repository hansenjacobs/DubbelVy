using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Dubbelvy.Models;

namespace Dubbelvy.App_Start
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<AuditTemplate, AuditTemplateViewModel>();
            CreateMap<AuditTemplateViewModel, AuditTemplate>();

            CreateMap<AuditSection, AuditSectionViewModel>();
            CreateMap<AuditSection, AuditSectionViewModel>();

            CreateMap<AuditElement, AuditElementViewModel>();
            CreateMap<AuditElementViewModel, AuditElement>();

            CreateMap<AuditElementChoice, AuditElementChoiceViewModel>();
            CreateMap<AuditElementChoiceViewModel, AuditElementChoice>();

            CreateMap<Audit, AuditViewModel>();
            CreateMap<AuditViewModel, Audit>();
        }

        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(m => m.AddProfile<MappingProfile>());
        }
    }
}