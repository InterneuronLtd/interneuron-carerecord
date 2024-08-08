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

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.AutoMapperProfiles
{
    public class ObservationResultMapperProfile : Profile
    {
        public ObservationResultMapperProfile()
        {
            CreateMap<entitystorematerialised_CoreResult, Observation>()
                //.ForMember(dest => dest.Meta, opt => opt.MapFrom(src => GetMeta(src)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ResultId))
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => GetResultIdentifiers(src)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => GetResultCode(src)))
                .ForMember(dest => dest.Effective, opt =>
                {
                    opt.PreCondition(src => src.Observationdatetime.HasValue);
                    opt.MapFrom(src => GetEffective(src));
                })
                .ForMember(dest => dest.Status, opt =>
                {
                    opt.PreCondition(src => !src.Observationresultstatus.IsEmpty());
                    opt.MapFrom(src => GetStatus(src));
                })
                .ForMember(dest => dest.Performer, opt => opt.MapFrom(GetPerformer))
                .ForMember(dest => dest.ReferenceRange, opt => opt.MapFrom(GetReferenceRange))
                .ForMember(dest => dest.Interpretation, opt =>
                {
                    opt.PreCondition(src => !src.Abnormalflag.IsEmpty());
                    opt.MapFrom(src => GetInterpretation(src));
                })
                .ForMember(dest => dest.Value, opt =>
                {
                    opt.PreCondition(src => !src.Observationvalue.IsNull());
                    opt.MapFrom(src => GetValue(src));
                });

            CreateMap<entitystorematerialised_CorePersonidentifier, Observation>()
                .ForMember(dest => dest.Subject, opt => opt.MapFrom<ObservationResultSubjectResolver>());

            CreateMap<entitystorematerialised_CoreOrder, Observation>()
                .ForMember(dest => dest.Issued, opt =>
                {
                    opt.PreCondition(cnd => cnd.Statuschangedatetime.HasValue);
                    opt.MapFrom(src => src.Statuschangedatetime);
                });
        }

        private Meta GetMeta(entitystorematerialised_CoreResult src)
        {
            if (src.IsNull() || !src.Createdtimestamp.HasValue) return null;

            return new Meta
            {
                VersionId = $"{src.Sequenceid}",
                LastUpdated = src.Createdtimestamp
            };
        }

        private Element GetValue(entitystorematerialised_CoreResult result)
        {
            if (int.TryParse(result.Observationvalue, out int intValue))
            {
                Quantity valueQuantity = new Quantity();

                valueQuantity.Code = result.Unitscode;
                valueQuantity.Value = Math.Round(Convert.ToDecimal(intValue), 2);
                valueQuantity.Unit = result.Unitstext;

                return valueQuantity;
            }
            else if (bool.TryParse(result.Observationvalue, out bool boolValue))
            {
                return new FhirBoolean(Convert.ToBoolean(boolValue));
            }
            else if (decimal.TryParse(result.Observationvalue, out decimal decimalValue))
            {
                Quantity valueQuantity = new Quantity();

                valueQuantity.Code = result.Unitscode;
                valueQuantity.Value = Math.Round(Convert.ToDecimal(decimalValue), 2);
                valueQuantity.Unit = result.Unitstext;

                return valueQuantity;
            }
            else
            {
                return new FhirString(result.Observationvalue);
            }
        }

        private List<CodeableConcept> GetInterpretation(entitystorematerialised_CoreResult result)
        {
            var interpretations = new List<CodeableConcept>();

            CodeableConcept code = new CodeableConcept();

            if (result.Abnormalflag.IsEmpty()) return interpretations;

            if (result.Abnormalflag.ToLower() == "true")
            {
                code.Text = "Abnormal";
            }
            else
            {
                code.Text = "Normal";
            }
            interpretations.Add(code);
            return interpretations;
        }

        private List<ResourceReference> GetPerformer(entitystorematerialised_CoreResult src, Observation obs)
        {
            var referenceResources = new List<ResourceReference>();

            if (obs.Performer.IsCollectionValid())
                obs.Performer.Each(per => referenceResources.Add(per));

            ResourceReference performer = new ResourceReference
            {
                Reference = "Practitioner",
                Display = src.Author
            };

            referenceResources.Add(performer);
            return referenceResources;
        }

        private List<Observation.ReferenceRangeComponent> GetReferenceRange(entitystorematerialised_CoreResult result, Observation obs)
        {
            var referenceRanges = new List<Observation.ReferenceRangeComponent>();

            if (obs.ReferenceRange.IsCollectionValid())
                obs.ReferenceRange.Each(refRange => referenceRanges.Add(refRange));

            Observation.ReferenceRangeComponent referenceRange = new Observation.ReferenceRangeComponent();

            referenceRange.Low = new Quantity
            {
                Value = Decimal.TryParse(result.Referencerangelow, out Decimal refLow) ? Math.Round(refLow, 2) : default(decimal?)
            };

            referenceRange.High = new Quantity
            {
                Value = Decimal.TryParse(result.Referencerangehigh, out Decimal refHigh) ? Math.Round(refHigh, 2) : default(decimal?)
            };

            referenceRanges.Add(referenceRange);
            return referenceRanges;
        }

        private ObservationStatus GetStatus(entitystorematerialised_CoreResult coreObservationResult)
        {
            if (coreObservationResult.Observationresultstatus.IsEmpty()) return default(ObservationStatus);

            switch (coreObservationResult.Observationresultstatus.ToLower())
            {
                case "amended":
                    return ObservationStatus.Amended;

                case "cancelled":
                    return ObservationStatus.Cancelled;

                case "corrected":
                    return ObservationStatus.Corrected;

                case "enteredinerror":
                    return ObservationStatus.EnteredInError;

                case "final":
                    return ObservationStatus.Final;

                case "preliminary":
                    return ObservationStatus.Preliminary;

                case "registered":
                    return ObservationStatus.Registered;

                default:
                    return ObservationStatus.Unknown;
            }

            return default(ObservationStatus); ;
        }

        private FhirDateTime GetEffective(entitystorematerialised_CoreResult coreObservationResult)
        {
            if (coreObservationResult.Observationdatetime.HasValue)
            {
                var elem = new FhirDateTime(coreObservationResult.Observationdatetime.Value.Year, coreObservationResult.Observationdatetime.Value.Month, coreObservationResult.Observationdatetime.Value.Day, coreObservationResult.Observationdatetime.Value.Hour, coreObservationResult.Observationdatetime.Value.Minute, coreObservationResult.Observationdatetime.Value.Second, TimeSpan.Zero);
                elem.ElementId = "effectiveDateTime";
                return elem;
            }
            return null;
        }

        private CodeableConcept GetResultCode(entitystorematerialised_CoreResult coreObservationResult)
        {
            var obsCodes = new CodeableConcept();

            Coding obsCode = new Coding();

            obsCode.Code = coreObservationResult.Observationidentifiercode;
            obsCode.Display = coreObservationResult.Observationidentifiertext;

            obsCodes.Coding.Add(obsCode);

            return obsCodes;
        }

        private List<Identifier> GetResultIdentifiers(entitystorematerialised_CoreResult coreObservationResult)
        {
            var identifiers = new List<Identifier>();

            Identifier id = new Identifier
            {
                Value = coreObservationResult.Contextkey
            };

            identifiers.Add(id);

            return identifiers;
        }
    }

    public class ObservationResultSubjectResolver : IValueResolver<entitystorematerialised_CorePersonidentifier, Observation, ResourceReference>
    {
        private IServiceProvider _provider;
        private string _defaultHospitalRefNo;

        public ObservationResultSubjectResolver(IServiceProvider provider)
        {
            this._provider = provider;
            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
        }
        public ResourceReference Resolve(entitystorematerialised_CorePersonidentifier personIdentifier, Observation destination, ResourceReference destMember, ResolutionContext context)
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
