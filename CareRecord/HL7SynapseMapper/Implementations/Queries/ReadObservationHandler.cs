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
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Interneuron.CareRecord.Infrastructure.Domain;
using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.CareRecord.Repository;
using Interneuron.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interneuron.CareRecord.HL7SynapseService.Implementations
{
    public class ReadObservationHandler : BaseReadQueryHandler
    {
        private const string SearchKeyIdentifier = "search_observation";

        private IServiceProvider _provider;
        private IReadOnlyRepository<entitystorematerialised_CoreObservation> _coreObservationRepo;
        private IFHIRParam _fhirParam;

        public ReadObservationHandler(IServiceProvider provider, IMapper mapper, IReadOnlyRepository<entitystorematerialised_CoreObservation> coreObservationRepo, IGenericSearchRepository genericSearchRepo) : base(provider, mapper, genericSearchRepo)
        {
            this._provider = provider;
            this._coreObservationRepo = coreObservationRepo;
        }
        public override ResourceData Handle(IFHIRParam fhirParam)
        {
            this._fhirParam = fhirParam;

            base.OnNoSearchResults = () => NoResultsHandler(fhirParam);

            return base.Handle(fhirParam);
        }

        private ResourceData NoResultsHandler(IFHIRParam fhirParam)
        {
            var resourceData = new ResourceData(fhirParam);

            var storeCoreObservationResult = CheckInEntityStore(fhirParam);

            if (storeCoreObservationResult == null || storeCoreObservationResult.ObservationId == null) return resourceData;

            if (storeCoreObservationResult.Recordstatus == 2) // Observation Result in Deleted State
            {
                resourceData.Resource = null;
                resourceData.DeletedDate = storeCoreObservationResult.Createddate;
                resourceData.IsDeleted = true;

                return resourceData;
            }

            return resourceData;
        }

        private entitystorematerialised_CoreObservation CheckInEntityStore(IFHIRParam fhirParam)
        {
            return _coreObservationRepo.ItemsAsReadOnly.Where(e => e.ObservationId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();

        }

        public override string GetSarchEntityIdentifier()
        {
            return SearchKeyIdentifier;
        }

        public override List<SynapseSearchTerm> GetSynapseSearchTerms()
        {
            var searchTerms = new List<SynapseSearchTerm>
            {
                new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", DefaultSearchExpressionProvider.EqualsOperator, this._defaultHospitalRefNo, new DefaultSearchExpressionProvider()),

                new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", DefaultSearchExpressionProvider.EqualsOperator, _fhirParam.ResourceId, new DefaultSearchExpressionProvider())
            };

            return searchTerms;
        }
    }
}
