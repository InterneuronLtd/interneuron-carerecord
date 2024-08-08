 //Interneuron synapse

//Copyright(C) 2024 Interneuron Limited

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
﻿using AutoMapper;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using static Hl7.Fhir.Model.Procedure;


namespace Interneuron.CareRecord.HL7SynapseService.AutoMapMapperProfiles
{
    public class ProcedureMapperProfile : Profile
    {
        public ProcedureMapperProfile()
        {
            CreateMap<entitystorematerialised_CoreProcedure, Procedure>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OperationId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => GetProcedureCode(src)))
                .ForMember(dest => dest.Performed, opt =>
                {
                    opt.PreCondition(src => src.Proceduredate.HasValue);
                    opt.MapFrom(src => GetProcedureDate(src));
                });
            //.ForMember(dest => dest.Performer, opt => {
            //    opt.PreCondition(src => src.Performedby.IsNotEmpty());
            //    opt.MapFrom(src => GetPerformer(src));
            //});


            CreateMap<entitystorematerialised_CorePersonidentifier, Procedure>()
                .ForMember(dest => dest.Subject, opt =>
                {
                    opt.MapFrom<ProcedureSubjectResolver>();
                });

            CreateMap<entitystorematerialised_CoreOperation, Procedure>()
                .ForMember(dest => dest.Performed, opt => opt.MapFrom((src, dest) => GetOperationDate(src, dest)))
                .ForMember(dest => dest.ReasonCode, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatus(src.Reasontext)));

            CreateMap<entitystorematerialised_CoreNote, Procedure>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom<ProcedureNoteResolver>());

        }

        private List<PerformerComponent> GetPerformer(entitystorematerialised_CoreProcedure src)
        {
            var performers = new List<PerformerComponent>();

            var performer = new PerformerComponent();
            performer.Actor = new ResourceReference();
            performer.Actor.Display = src.Performedby;

            performers.Add(performer);

            return performers;
        }

        private FhirDateTime GetProcedureDate(entitystorematerialised_CoreProcedure src)
        {
            if (src.Proceduredate.HasValue)
            {
                var elem = new FhirDateTime(src.Proceduredate.Value.Year, src.Proceduredate.Value.Month, src.Proceduredate.Value.Day);
                return elem;
            }
            return null;
        }

        private FhirDateTime GetOperationDate(entitystorematerialised_CoreOperation src, Procedure proc)
        {
            if (src.Start.HasValue && proc.Id == src.OperationId)
            {
                var elem = new FhirDateTime(src.Start.Value.Year, src.Start.Value.Month, src.Start.Value.Day);
                return elem;
            }
            else if (proc.Performed.IsNotNull())
            {
                return new FhirDateTime(proc.Performed.ToString());
            }
            return null;
        }

        private object GetProcedureCode(entitystorematerialised_CoreProcedure procedure)
        {
            var procCodes = new CodeableConcept();

            Coding obsCode = new Coding();

            obsCode.Code = procedure.Code;
            obsCode.Display = procedure.Name;

            procCodes.Coding.Add(obsCode);

            return procCodes;
        }

        private EventStatus? GetStatus(string status)
        {
            if (!status.IsNotEmpty()) return null;

            return status.ToUpper() switch
            {
                "ON-HOLD" => EventStatus.Suspended,
                //"STOPPED" => EventStatus.,
                "ENTERED - IN - ERROR" => EventStatus.EnteredInError,
                "COMPLETED" => EventStatus.Completed,
                "IN - PROGRESS" => EventStatus.InProgress,
                //"NOT - DONE" => EventStatus.NotDone,
                "PREPARATION" => EventStatus.Preparation,
                _ => null,
            };
        }

        private Meta GetMeta(entitystorematerialised_CoreProcedure src)
        {
            if (src.IsNull() || !src.Createdtimestamp.HasValue) return null;

            return new Meta
            {
                VersionId = $"{src.Sequenceid}",
                LastUpdated = src.Createdtimestamp
            };
        }
    }

    internal class ProcedureNoteResolver : IValueResolver<entitystorematerialised_CoreNote, Procedure, List<Annotation>>
    {
        public List<Annotation> Resolve(entitystorematerialised_CoreNote coreNote, Procedure destination, List<Annotation> destMember, ResolutionContext context)
        {
            var notes = new List<Annotation>();

            if (destination.Note.IsCollectionValid())
            {
                destination.Note.Each(note => {
                    if (destination.Id == coreNote.Parentid)
                    {
                        note.Text = coreNote.Comment.Replace("<NTE.3.1>", " ").Replace("</NTE.3.1>", ". ");
                    }
                    notes.Add(note);
                });
            }
            else
            {
                var note = new Annotation();

                note.Text = coreNote.Comment.Replace("<NTE.3.1>", " ").Replace("</NTE.3.1>", ". ");

                notes.Add(note);
            }

            return notes;
        }
    }

    public class ProcedureSubjectResolver : IValueResolver<entitystorematerialised_CorePersonidentifier, Procedure, ResourceReference>
    {
        private IServiceProvider _provider;
        private string _defaultHospitalRefNo;

        public ProcedureSubjectResolver(IServiceProvider provider)
        {
            this._provider = provider;
            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
        }

        public ResourceReference Resolve(entitystorematerialised_CorePersonidentifier personIdentifier, Procedure destination, ResourceReference destMember, ResolutionContext context)
        {
            var subject = new ResourceReference
            {
                Reference = $"Patient/{personIdentifier.Idnumber}"
            };

            return subject;
        }

        private string GetDefaultHospitalRefNo()
        {
            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

            return careRecordConfig.GetValue<string>("HospitalNumberReference");
        }
    }
}
