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
﻿using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    public class ObservationSearchQueryBuilder : QueryBuilder
    {
        public override List<dynamic> Execute(List<SynapseSearchTerm> synapseSearchTerms)
        {
            string[] observationTypeIds = {
                "e2d6d5fa-2a38-448f-9e06-9a3e6b532bb5", //Diastolic Blood Pressure
                "83a4b253-5599-43d2-a377-9f8001e7dbac", // Height
                "f05f980e-e29c-48ed-9cb7-51d47221e6a6",// Heart Rate
                "7e554e57-63fe-4924-a49b-646a6c6ef5ce",// Respiratory Rate
                "af5dc4a2-3e40-4e09-9d4f-3ddb137158de",// Systolic Blood Pressure
                "d76e8bd1-158e-45e3-b4b0-d526e7268005",// Temperature
                "71d6a339-7d9e-4054-99d6-683da95331dc",// Weight
                "d8c07866-6c4a-468d-b832-47427fe4a805",// Glucose
                "ddf588a5-1ae8-4417-ab58-502ddf808de5"//AVPU
            };

            var baseQuery = (from observation in this.dbContext.entitystorematerialised_CoreObservation
                             join observationEvent in this.dbContext.entitystorematerialised_CoreObservationevent
                             on observation.ObservationeventId equals observationEvent.ObservationeventId
                             join person in this.dbContext.entitystorematerialised_CorePerson on
                             observationEvent.PersonId equals person.PersonId
                             join personIden in this.dbContext.entitystorematerialised_CorePersonidentifier on
                             person.PersonId equals personIden.PersonId
                             join observationType in this.dbContext.entitystorematerialised_MetaObservationtype
                             on observation.ObservationtypeId equals observationType.ObservationtypeId
                             where observationTypeIds.Contains(observationType.ObservationtypeId) && (observation.Method != "258083009 | Visual estimation (qualifier value)" || observation.Method == null)
                             select new
                             {
                                 observationData = observation,
                                 observationEventData = observationEvent,
                                 personData = person,
                                 personIdData = personIden,
                                 observationTypeData = observationType
                             });

            var searchOp = new GenericSearchOpProcessor();

            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

            var matResults = withSearchClause
                .OrderByDescending((entity) => entity.observationData.Createdtimestamp)
                .Select(entity => new
                {
                    observatoins = entity.observationData,
                    observationevents = entity.observationEventData,
                    observationType = entity.observationTypeData,
                    patientIdentifer = entity.personIdData
                })
                .ToList();

            if (matResults.IsCollectionValid())
            {
                var observations = matResults.Select(m => m.observatoins).Distinct().ToList();
                //var observationType = matResults.Select(m => m.observationType).Distinct().ToList();
                //var observationevents = matResults.Select(m => m.observationevents).Distinct().ToList();
                var patientIdentifier = matResults.Select(m => m.patientIdentifer).FirstOrDefault();

                var dictionary = observations
                      .GroupBy(item => item.ObservationeventId)
                      .ToDictionary(grp => grp.Key, grp => grp.ToList());

                List<string> keys = dictionary.Keys.ToList();

                observations.Clear();

                foreach (string key in keys)
                {
                    List<entitystorematerialised_CoreObservation> obs = dictionary[key];

                    var observationSystolic = obs?.Find(obx => obx.ObservationtypeId == "af5dc4a2-3e40-4e09-9d4f-3ddb137158de");

                    var observationDiastolic = obs?.Find(obx => obx.ObservationtypeId == "e2d6d5fa-2a38-448f-9e06-9a3e6b532bb5");

                    if (observationSystolic != null)
                    {
                        obs.Remove(observationSystolic);
                    }

                    if (observationDiastolic != null)
                    {
                        obs.Remove(observationDiastolic);
                    }

                    if (observationSystolic != null)
                    {
                        obs.Add(observationSystolic);
                    }

                    if (observationDiastolic != null)
                    {
                        obs.Add(observationDiastolic);
                    }

                    dictionary[key] = obs;

                    observations.AddRange(dictionary[key]);
                }

                return new List<dynamic> { observations, patientIdentifier };
            }

            return null;
        }
    }
}
